using System;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Dominator.Domain.Classes.Security
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed),
     ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single/*, AddressFilterMode = AddressFilterMode.Any*/)]
    public class EncryptionService : IEncryptionService
    {
        public string Ping()
        {
            return DateTime.Now.ToString("u");
        }

        public bool EncryptData(ref byte[] userData)
        {
            try
            {
                userData = ProtectedData.Protect(userData, null, DataProtectionScope.CurrentUser);
                return true;
            }
            catch (Exception e)
            {
            }

            return false;
        }

        public bool DecryptData(ref byte[] encryptedData)
        {
            try
            {
                encryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
                return true;
            }
            catch (Exception e)
            {
            }

            return false;
        }
    }
}