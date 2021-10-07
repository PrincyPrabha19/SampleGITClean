namespace Dominator.Domain
{
    public interface ICryptoManager
    {
        void Initialize();
        bool EncryptData(ref byte[] userData);
        bool DecryptData(ref byte[] encryptedData);
    }
}