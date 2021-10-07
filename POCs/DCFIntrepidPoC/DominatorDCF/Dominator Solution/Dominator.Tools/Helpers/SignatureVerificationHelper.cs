using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Dominator.Tools.Helpers
{
    public class SignatureVerification
    {
        private static byte[] PrivateVerify = new byte[] { 0x2f, 0x45, 0x54, 0x2f };
        private static byte[] PublicVerify = new byte[] { 0x1f, 0xc7, 0xa3, 0x1f };

        public static bool IsPrivateKeyFile(string keyFile)
        {
            if (string.IsNullOrEmpty(keyFile) || !File.Exists(keyFile))
            {
                return false;
            }

            try
            {
                RSAParameters param = new RSAParameters();
                OpenRSAPrivate(keyFile, ref param);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void OpenRSAPrivate(string keyPrivateFile, ref RSAParameters param)
        {
            Stream stream = File.OpenRead(keyPrivateFile);
            using (stream)
            {
                OpenRSAPrivate(stream, ref param);
                stream.Close();
            }
        }

        private static void OpenRSAPrivate(Stream keyPrivateStream, ref RSAParameters param)
        {
            Stream stream = keyPrivateStream;
            using (stream)
            {
                int nLen = 0;
                byte[] temp = new byte[sizeof(int)];
                stream.Read(temp, 0, sizeof(int));

                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i] != PrivateVerify[i])
                    {
                        throw new Exception("data verify failed.");
                    }
                }

                stream.Read(temp, 0, sizeof(int));
                nLen = BitConverter.ToInt32(temp, 0);
                param.D = new byte[nLen];
                stream.Read(param.D, 0, nLen);
                Array.Reverse(param.D);

                stream.Read(temp, 0, sizeof(int));
                nLen = BitConverter.ToInt32(temp, 0);
                param.DP = new byte[nLen];
                stream.Read(param.DP, 0, nLen);

                stream.Read(temp, 0, sizeof(int));
                nLen = BitConverter.ToInt32(temp, 0);
                param.DQ = new byte[nLen];
                stream.Read(param.DQ, 0, nLen);

                stream.Read(temp, 0, sizeof(int));
                nLen = BitConverter.ToInt32(temp, 0);
                param.Exponent = new byte[nLen];
                stream.Read(param.Exponent, 0, nLen);

                stream.Read(temp, 0, sizeof(int));
                nLen = BitConverter.ToInt32(temp, 0);
                param.InverseQ = new byte[nLen];
                stream.Read(param.InverseQ, 0, nLen);
                Array.Reverse(param.InverseQ);

                stream.Read(temp, 0, sizeof(int));
                nLen = BitConverter.ToInt32(temp, 0);
                param.Modulus = new byte[nLen];
                stream.Read(param.Modulus, 0, nLen);

                stream.Read(temp, 0, sizeof(int));
                nLen = BitConverter.ToInt32(temp, 0);
                param.P = new byte[nLen];
                stream.Read(param.P, 0, nLen);

                stream.Read(temp, 0, sizeof(int));
                nLen = BitConverter.ToInt32(temp, 0);
                param.Q = new byte[nLen];
                stream.Read(param.Q, 0, nLen);
            }
        }

        private static void SaveRSAPrivate(string keyPrivateFile, RSAParameters param)
        {
            // create private snk
            Stream stream = File.OpenWrite(keyPrivateFile);
            using (stream)
            {
                byte[] temp = PrivateVerify;
                stream.Write(temp, 0, temp.Length);

                Array.Reverse(param.D);
                temp = BitConverter.GetBytes(param.D.Length);
                stream.Write(temp, 0, temp.Length);
                stream.Write(param.D, 0, param.D.Length);

                temp = BitConverter.GetBytes(param.DP.Length);
                stream.Write(temp, 0, temp.Length);
                stream.Write(param.DP, 0, param.DP.Length);

                temp = BitConverter.GetBytes(param.DQ.Length);
                stream.Write(temp, 0, temp.Length);
                stream.Write(param.DQ, 0, param.DQ.Length);

                temp = BitConverter.GetBytes(param.Exponent.Length);
                stream.Write(temp, 0, temp.Length);
                stream.Write(param.Exponent, 0, param.Exponent.Length);

                Array.Reverse(param.InverseQ);
                temp = BitConverter.GetBytes(param.InverseQ.Length);
                stream.Write(temp, 0, temp.Length);
                stream.Write(param.InverseQ, 0, param.InverseQ.Length);

                temp = BitConverter.GetBytes(param.Modulus.Length);
                stream.Write(temp, 0, temp.Length);
                stream.Write(param.Modulus, 0, param.Modulus.Length);

                temp = BitConverter.GetBytes(param.P.Length);
                stream.Write(temp, 0, temp.Length);
                stream.Write(param.P, 0, param.P.Length);

                temp = BitConverter.GetBytes(param.Q.Length);
                stream.Write(temp, 0, temp.Length);
                stream.Write(param.Q, 0, param.Q.Length);

                stream.Close();
            }
        }

        private static bool SavePublicKey(string publicKeyFile, RSAParameters param)
        {
            // create private snk
            Stream stream = File.OpenWrite(publicKeyFile);
            using (stream)
            {
                byte[] temp = PublicVerify;
                stream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(param.Exponent.Length);
                stream.Write(temp, 0, temp.Length);
                stream.Write(param.Exponent, 0, param.Exponent.Length);

                if (param.Exponent.Length == 3)
                {
                    byte[] a = new byte[] { 0x00, 0x01, 0x02, 0x03, 0xff };

                    temp = BitConverter.GetBytes(a.Length);
                    stream.Write(temp, 0, temp.Length);
                    stream.Write(a, 0, a.Length);
                }

                temp = BitConverter.GetBytes(param.Modulus.Length);
                stream.Write(temp, 0, temp.Length);
                stream.Write(param.Modulus, 0, param.Modulus.Length);

                stream.Close();
            }

            return true;
        }

        private static XmlElement FindSignatureKey(XmlDocument xmlDoc)
        {
            return (XmlElement)xmlDoc.SelectSingleNode("//signatureSetting");
        }

        private static bool UpdateVersion(XmlDocument xmlDoc, string version)
        {
            bool bUpdate = false;

            XmlElement signatureElement = FindSignatureKey(xmlDoc);
            if (signatureElement != null)
            {
                signatureElement.SetAttribute("version", version);
                bUpdate = true;
            }

            return bUpdate;
        }

        public static string GetVersion(string AuthenticationFile)
        {
            string version = string.Empty;

            XmlDocument xmlDoc = LoadXmlFile(AuthenticationFile);
            XmlElement signatureElement = FindSignatureKey(xmlDoc);
            if (signatureElement != null)
            {
                version = signatureElement.GetAttribute("version");
            }

            return version;
        }

        private static XmlDocument LoadXmlFile(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Prohibit;
            settings.XmlResolver = null;
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (XmlReader reader = XmlReader.Create(stream, settings))
                {
                    xmlDoc.PreserveWhitespace = true;
                    xmlDoc.Load(reader);
                    reader.Close();
                }

                stream.Close();
            }

            return xmlDoc;
        }

        private static bool GenerateByKey(string strSourceFile, string strTargetFile, RSA key, string version)
        {
            bool retVal = false;

            XmlDocument xmlDoc = LoadXmlFile(strSourceFile);
            XmlElement signatureElement = FindSignatureKey(xmlDoc);
            if (signatureElement != null)
            {
                signatureElement.RemoveAll();
                if (UpdateVersion(xmlDoc, version))
                {
                    SignedXml signedXml = new SignedXml(xmlDoc);
                    signedXml.SigningKey = key;
                    Reference reference = new Reference();
                    reference.Uri = "";
                    XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                    reference.AddTransform(env);
                    signedXml.AddReference(reference);
                    signedXml.ComputeSignature();
                    XmlElement xmlDigitalSignature = signedXml.GetXml();
                
                    signatureElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
                    xmlDoc.Save(strTargetFile);
                    retVal = true;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Generate a new authentication file
        /// </summary>
        /// <param name="sourceFile">source file</param>
        /// <param name="targetFile">output file</param>
        /// <param name="privateKeyStream">private key stream</param>
        /// <param name="version">version information</param>
        /// <returns>Success return true, otherwise return false</returns>
        public static bool GenerateAuthentication(string sourceFile, string targetFile, Stream privateKeyStream, string version)
        {
            if (string.IsNullOrEmpty(sourceFile) || !File.Exists(sourceFile))
            {
                return false;
            }

            if (string.IsNullOrEmpty(targetFile) || !Directory.Exists(Path.GetDirectoryName(targetFile)))
            {
                return false;
            }

            RSACryptoServiceProvider Key = new RSACryptoServiceProvider();

            try
            {
                RSAParameters param = new RSAParameters();
                OpenRSAPrivate(privateKeyStream, ref param);
                Key.ImportParameters(param);
            }
            catch
            {
                return false;
            }

            if (GenerateByKey(sourceFile, targetFile, Key, version) == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Generate a new authentication file
        /// </summary>
        /// <param name="sourceFile">source file</param>
        /// <param name="targetFile">output file</param>
        /// <param name="privateKeyFile">private key file, if this file is not exist then create new one and also will create public key file</param>
        /// <param name="version">version information</param>
        /// <returns>return error code, success return 0</returns>
        public static int GenerateAuthentication(string sourceFile, string targetFile, string privateKeyFile, string version)
        {
            if (string.IsNullOrEmpty(sourceFile) || !File.Exists(sourceFile))
            {
                return -1;
            }

            if (string.IsNullOrEmpty(targetFile) || !Directory.Exists(Path.GetDirectoryName(targetFile)))
            {
                return -2;
            }

            if (string.IsNullOrEmpty(privateKeyFile) || !Directory.Exists(Path.GetDirectoryName(privateKeyFile)))
            {
                return -3;
            }

            RSACryptoServiceProvider Key = new RSACryptoServiceProvider();

            if (File.Exists(privateKeyFile))
            {
                try
                {
                    RSAParameters param = new RSAParameters();
                    OpenRSAPrivate(privateKeyFile, ref param);
                    Key.ImportParameters(param);
                }
                catch
                {
                    return -4;
                }
            }
            else
            {
                string keyPublicFile = null;
                string keyPublicFileExt = null;
                try
                {
                    RSAParameters param = Key.ExportParameters(true);
                    SaveRSAPrivate(privateKeyFile, param);

                    keyPublicFile = privateKeyFile.Replace("Private", "Public");
                    keyPublicFile = Path.Combine(Path.GetDirectoryName(keyPublicFile), Path.GetFileNameWithoutExtension(keyPublicFile) + ".snk");
                    SavePublicKey(keyPublicFile, param);
                }
                catch
                {
                    if (File.Exists(privateKeyFile))
                    {
                        File.Delete(privateKeyFile);
                    }
                    if (!string.IsNullOrEmpty(keyPublicFile) && File.Exists(keyPublicFile))
                    {
                        File.Delete(keyPublicFile);
                    }
                    if (!string.IsNullOrEmpty(keyPublicFileExt) && File.Exists(keyPublicFileExt))
                    {
                        File.Delete(keyPublicFileExt);
                    }
                    return -6;
                }
            }

            if (GenerateByKey(sourceFile, targetFile, Key, version) == false)
            {
                return -7;
            }

            return 0;
        }


        private static bool CheckAuthenticationByKey(string AuthenticationFile, RSA key)
        {
            XmlDocument xmlDoc = LoadXmlFile(AuthenticationFile);
            SignedXml signedXml = new SignedXml(xmlDoc);

            XmlElement signatureElement = FindSignatureKey(xmlDoc);
            if (signatureElement != null)
            {
                XmlNodeList elementsByTagName = signatureElement.GetElementsByTagName("Signature");
                if (elementsByTagName.Count <= 0)
                {
                    throw new Exception("Signature verification process fails: No signature was found.");
                }
                if (elementsByTagName.Count >= 2)
                {
                    throw new Exception("Signature verification process fails: more than one signature were found.");
                }
                signedXml.LoadXml((XmlElement)elementsByTagName[0]);
                return signedXml.CheckSignature(key);
            }
            else
            {
                throw new Exception("Signature verification process fails: Signature setting element was not found.");
            }
        }

        private static void OpenRSAPublic(Stream keyPublic, ref RSAParameters param)
        {
            param = default(RSAParameters);

            Stream stream = keyPublic;
            using (stream)
            {
                int nLen = 0;
                byte[] temp = new byte[sizeof(int)];
                stream.Read(temp, 0, sizeof(int));

                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i] != PublicVerify[i])
                    {
                        throw new Exception("data verify failed.");
                    }
                }

                stream.Read(temp, 0, sizeof(int));
                nLen = BitConverter.ToInt32(temp, 0);
                if (nLen == 3)
                {
                    param.Exponent = new byte[nLen];
                }
                stream.Read(param.Exponent, 0, param.Exponent.Length);

                if (nLen == 3)
                {
                    // byte[] b = new byte[] { 0x00, 0x01, 0x02, 0x03, 0xff };

                    stream.Read(temp, 0, sizeof(int));
                    nLen = BitConverter.ToInt32(temp, 0);
                    byte[] b = new byte[nLen];
                    stream.Read(b, 0, nLen);
                }

                stream.Read(temp, 0, sizeof(int));
                nLen = BitConverter.ToInt32(temp, 0);
                param.Modulus = new byte[nLen];
                stream.Read(param.Modulus, 0, nLen);
            }
        }

        /// <summary>
        /// verify authentication file
        /// </summary>
        /// <param name="AuthenticationFileName">authentication file</param>
        /// <param name="publicKey">private key stream</param>
        /// <returns>Success return true, otherwise return false</returns>
        public static bool CheckAuthenticationFile(string AuthenticationFileName, Stream publicKey)
        {
            if (string.IsNullOrEmpty(AuthenticationFileName) || !File.Exists(AuthenticationFileName))
            {
                throw new Exception("Authentication file not find: " + AuthenticationFileName);
            }

            try
            {
                RSAParameters param = new RSAParameters();
                OpenRSAPublic(publicKey, ref param);
                RSACryptoServiceProvider Key = new RSACryptoServiceProvider();
                Key.ImportParameters(param);

                if (!CheckAuthenticationByKey(AuthenticationFileName, Key))
                {
                    throw new Exception("Authentication check failed.");
                }
            }
            catch
            {
                throw new Exception("Authentication check failed.");
            }

            return true;
        }

        public static bool CheckVersion(string AuthenticationFile, string version)
        {
            if (string.Compare(GetVersion(AuthenticationFile), version, true) != 0)
            {
                throw new Exception("Validate configure file, version check failed");
            }

            return true;
        }
    }
}
