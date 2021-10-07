using System;
using System.IO;
using System.Reflection;
using Dominator.Tools.Helpers;

namespace Dominator.Tools.Classes
{
    public class SignatureVerifier : ISignatureVerifier
    {
        private readonly Logger logger = new Logger();
        private const string SIGNATURE_PUBLICKEY = "Dell.PPG.Auth.Public.snk";

        public bool VerifySignature(string filePath)
        {
            Stream stream = null;
            try
            {
                stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Dominator.Tools.{SIGNATURE_PUBLICKEY}");
                if (stream != null)
                {
                    var result = SignatureVerification.CheckAuthenticationFile(filePath, stream);
                    if (!result)
                        logger.WriteError($"Unable to verify signature of file: {filePath}");
                    return result;
                }

            }
            catch (Exception e)
            {
                logger.WriteError($"Error verifying signature of file: {filePath}", null, e.ToString());
            }
            finally
            {
                stream?.Close();
            }

            return false;
        }
    }
}
