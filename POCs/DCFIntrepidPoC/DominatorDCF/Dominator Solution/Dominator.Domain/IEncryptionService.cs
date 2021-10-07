using System.ServiceModel;

namespace Dominator.Domain
{
    [ServiceContract]
    public interface IEncryptionService
    {
        [OperationContract]
        string Ping();

        [OperationContract]
        bool EncryptData(ref byte[] userData);

        [OperationContract]
        bool DecryptData(ref byte[] encryptedData);
    }
}