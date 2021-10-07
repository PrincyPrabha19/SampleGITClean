using System;
using Dominator.Domain.Classes.Factories;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;

namespace Dominator.Domain.Classes.Security
{
    public class CryptoManager : ICryptoManager
    {
        #region Properties
        private readonly ILogger logger = LoggerFactory.LoggerInstance;
        private IEncryptionService encryptionInstance;
        #endregion

        #region Methods
        public void Initialize()
        {
            initializeEncryptionInstance();
        }

        public bool EncryptData(ref byte[] userData)
        {
            initializeEncryptionInstance();
            return encryptionInstance?.EncryptData(ref userData) ?? false;
        }

        public bool DecryptData(ref byte[] encryptedData)
        {
            initializeEncryptionInstance();
            return encryptionInstance?.DecryptData(ref encryptedData) ?? false;
        }

        private void initializeEncryptionInstance()
        {
            if (encryptionInstance == null)
                encryptionInstance = EncryptionFactory.NewEncryptionFactory();
            else
            {
                try
                {
                    encryptionInstance.Ping();
                }
                catch (Exception e)
                {
                    logger?.WriteError("CryptoManager.initializeEncryptionInstance", null, e.ToString());
                }
            }
        }
        #endregion
    }
}