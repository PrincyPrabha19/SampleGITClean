namespace Dominator.Tools
{
    public interface ISignatureVerifier
    {
        bool VerifySignature(string filePath);
    }
}