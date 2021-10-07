using System;
using System.Diagnostics;
using System.IO;
using Dominator.Domain.Classes.Factories;

namespace EncryptDecrypt
{
    public class MemoryProtectionSample
    {
        public static void Main(string[] args)
        {
            if (args.Length <= 1 || (args[0] != "/e" && args[0] != "/d"))
            {
                Console.WriteLine("Invalid arguments: EncryptDecrypt [/e/d] <file>");
                Console.ReadLine();
                return;
            }
            
            if (args[0] == "/d")
                RunDecrypt(args[1]);
            else
                RunEncrypt(args[1]);

            Console.ReadLine();
        }

        public static void RunDecrypt(string filename)
        {
            if (!File.Exists(filename)) return;

            try
            {
                byte[] userData;
                using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var br = new BinaryReader(fs))
                    {
                        userData = new byte[fs.Length];
                        br.Read(userData, 0, userData.Length);
                        br.Close();
                    }

                    fs.Close();
                }

                var em = EncryptionFactory.NewCryptoManager();
                var result = em.DecryptData(ref userData);
                if (!result)
                {
                    Console.WriteLine("Unable to decrypt file, check Dominator Windows Service is running!");
                    return;
                }

                var outputDir = Path.GetDirectoryName(filename);
                var outputFilename = Path.GetFileNameWithoutExtension(filename) + "_DECRYPTED";
                var outputFileExt = Path.GetExtension(filename);

                var tempFilename = Path.Combine(outputDir, outputFilename + outputFileExt);
                using (var fs = new FileStream(tempFilename, FileMode.Create))
                {
                    using (var bw = new BinaryWriter(fs))
                    {
                        bw.Write(userData);
                        bw.Close();
                    }

                    fs.Close();
                }

                Console.WriteLine($"Decrypted file to: {tempFilename}");

                var tempOutputXml = Path.GetTempFileName() + ".xml";
                File.Copy(tempFilename, tempOutputXml);
                Process.Start(@"iexplore.exe", tempOutputXml);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
        }

        public static void RunEncrypt(string filename)
        {
            if (!File.Exists(filename)) return;

            try
            {
                byte[] userData;
                using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    using (var br = new BinaryReader(fs))
                    {
                        userData = new byte[fs.Length];
                        br.Read(userData, 0, userData.Length);
                        br.Close();
                    }

                    fs.Close();
                }

                var em = EncryptionFactory.NewCryptoManager();
                var result = em.EncryptData(ref userData);
                if (!result)
                {
                    Console.WriteLine("Unable to encrypt file, check Dominator Windows Service is running!");
                    return;
                }

                var outputDir = Path.GetDirectoryName(filename);
                var outputFilename = Path.GetFileNameWithoutExtension(filename) + "_ENCRYPTED";
                var outputFileExt = Path.GetExtension(filename);

                var tempFilename = Path.Combine(outputDir, outputFilename + outputFileExt);
                using (var fs = new FileStream(tempFilename, FileMode.Create))
                {
                    using (var bw = new BinaryWriter(fs))
                    {
                        bw.Write(userData);
                        bw.Close();
                    }

                    fs.Close();
                }

                Console.WriteLine($"Encrypted file to: {tempFilename}");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
        }

        //public static void RunDecrypt(string filename)
        //{
        //    if (!File.Exists(filename)) return;

        //    try
        //    {
        //        byte[] userData;
        //        using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
        //        {
        //            using (var br = new BinaryReader(fs))
        //            {
        //                userData = new byte[fs.Length];
        //                br.Read(userData, 0, userData.Length);
        //                br.Close();
        //            }

        //            fs.Close();
        //        }

        //        var em = EncryptionFactory.NewCryptoManager();
        //        var result = em.DecryptData(ref userData);
        //        if (!result)
        //        {
        //            Console.WriteLine("Unable to decrypt file, check Dominator Windows Service is running!");
        //            return;
        //        }

        //        var tempFilename = Path.GetTempFileName() + ".xml";
        //        using (var fs = new FileStream(tempFilename, FileMode.Create))
        //        {
        //            using (var bw = new BinaryWriter(fs))
        //            {
        //                bw.Write(userData);
        //                bw.Close();                            
        //            }

        //            fs.Close();
        //        }

        //        Process.Start(@"iexplore.exe", tempFilename);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("ERROR: " + e.Message);
        //    }
        //}

        //private static ProfileData readProfile(string path)
        //{
        //    ProfileData profileData = null;

        //    try
        //    {
        //        XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProfileData));
        //        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
        //            profileData = (ProfileData)xmlSerializer.Deserialize(fileStream);
        //    }
        //    catch (Exception e)
        //    {
        //    }

        //    return profileData;
        //}

        //private static bool writeProfile(ProfileData profileData, string path)
        //{
        //    try
        //    {
        //        XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProfileData));
        //        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
        //            xmlSerializer.Serialize(fileStream, profileData);
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //    }

        //    return false;
        //}

        //public static void RunEncrypt()
        //{
        //    try
        //    {

        //        var profileData = readProfile(@"C:\Temp\OC2_AlienwareAuroraR5_1.0.opp");

        //        byte[] toEncrypt;
        //        IFormatter formatter = new BinaryFormatter();
        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            formatter.Serialize(stream, profileData);
        //            toEncrypt = stream.ToArray();
        //        }

        //        var em = EncryptionFactory.NewCryptoManager();
        //        var result = em.EncryptData(ref toEncrypt);

        //        var fs = new FileStream(@"C:\Temp\OC1_Alienware15_1.0.opp.txt", FileMode.Create);
        //        var bw = new BinaryWriter(fs);
        //        bw.Write(toEncrypt);
        //        bw.Close();
        //        fs.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("ERROR: " + e.Message);
        //    }
        //}

        //public static void RunDecrypt()
        //{
        //    try
        //    {
        //        var profileData = new ProfileData();
        //        var fs = new FileStream(@"C:\Temp\OC1_Alienware15_1.0.opp.txt", FileMode.Open, FileAccess.Read);
        //        var br = new BinaryReader(fs);
        //        var userData = new byte[fs.Length];
        //        if (br.Read(userData, 0, userData.Length) > 0)
        //        {
        //            var em = EncryptionFactory.NewCryptoManager();
        //            var result = em.DecryptData(ref userData);

        //            IFormatter formatter = new BinaryFormatter();
        //            using (MemoryStream stream = new MemoryStream(userData))
        //            {
        //                profileData = formatter.Deserialize(stream) as ProfileData;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("ERROR: " + e.Message);
        //    }
        //}

        //public static void EncryptInMemoryData(byte[] Buffer, MemoryProtectionScope Scope)
        //{
        //    if (Buffer.Length <= 0)
        //        throw new ArgumentException("Buffer");
        //    if (Buffer == null)
        //        throw new ArgumentNullException("Buffer");


        //    // Encrypt the data in memory. The result is stored in the same same array as the original data.
        //    ProtectedMemory.Protect(Buffer, Scope);

        //}

        //public static void DecryptInMemoryData(byte[] Buffer, MemoryProtectionScope Scope)
        //{
        //    if (Buffer.Length <= 0)
        //        throw new ArgumentException("Buffer");
        //    if (Buffer == null)
        //        throw new ArgumentNullException("Buffer");


        //    // Decrypt the data in memory. The result is stored in the same same array as the original data.
        //    ProtectedMemory.Unprotect(Buffer, Scope);

        //}

        //public static byte[] CreateRandomEntropy()
        //{
        //    // Create a byte array to hold the random value.
        //    byte[] entropy = new byte[16];

        //    // Create a new instance of the RNGCryptoServiceProvider.
        //    // Fill the array with a random value.
        //    new RNGCryptoServiceProvider().GetBytes(entropy);

        //    // Return the array.
        //    return entropy;
        //}

        //public static int EncryptDataToStream(byte[] Buffer, byte[] Entropy, DataProtectionScope Scope, Stream S)
        //{
        //    if (Buffer.Length <= 0)
        //        throw new ArgumentException("Buffer");
        //    if (Buffer == null)
        //        throw new ArgumentNullException("Buffer");
        //    if (Entropy.Length <= 0)
        //        throw new ArgumentException("Entropy");
        //    if (Entropy == null)
        //        throw new ArgumentNullException("Entropy");
        //    if (S == null)
        //        throw new ArgumentNullException("S");

        //    int length = 0;

        //    // Encrypt the data in memory. The result is stored in the same same array as the original data.
        //    byte[] encrptedData = ProtectedData.Protect(Buffer, Entropy, Scope);

        //    // Write the encrypted data to a stream.
        //    if (S.CanWrite && encrptedData != null)
        //    {
        //        S.Write(encrptedData, 0, encrptedData.Length);

        //        length = encrptedData.Length;
        //    }

        //    // Return the length that was written to the stream. 
        //    return length;

        //}

        //public static byte[] DecryptDataFromStream(byte[] Entropy, DataProtectionScope Scope, Stream S, int Length)
        //{
        //    if (S == null)
        //        throw new ArgumentNullException("S");
        //    if (Length <= 0)
        //        throw new ArgumentException("Length");
        //    if (Entropy == null)
        //        throw new ArgumentNullException("Entropy");
        //    if (Entropy.Length <= 0)
        //        throw new ArgumentException("Entropy");



        //    byte[] inBuffer = new byte[Length];
        //    byte[] outBuffer;

        //    // Read the encrypted data from a stream.
        //    if (S.CanRead)
        //    {
        //        S.Read(inBuffer, 0, Length);

        //        outBuffer = ProtectedData.Unprotect(inBuffer, Entropy, Scope); 
        //    }
        //    else
        //    {
        //        throw new IOException("Could not read the stream.");
        //    }

        //    // Return the length that was written to the stream. 
        //    return outBuffer;

        //}
    }
}