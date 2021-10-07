using System;

namespace Dominator.Domain.Classes.Security
{
    public class CryptoManager2 : ICryptoManager
    {
        public IEncryptionManager EncryptionManager { get; set; }

        public bool EncryptData(ref byte[] userData)
        {
            if (userData == null) return false;

            try
            {
                if (EncryptionManager?.EncryptData(ref userData) ?? false)
                    return true;
            }
            catch (Exception e)
            {
            }

            return false;
        }

        public bool DecryptData(ref byte[] encryptedData)
        {
            if (encryptedData == null) return false;

            try
            {
                if (EncryptionManager?.DecryptData(ref encryptedData) ?? false)
                    return true;
            }
            catch (Exception e)
            {
            }

            return false;
        }
    }

    public interface ICryptoManager
    {
        IEncryptionManager EncryptionManager { get; set; }
        bool EncryptData(ref byte[] userData);
        bool DecryptData(ref byte[] encryptedData);
    }
}
