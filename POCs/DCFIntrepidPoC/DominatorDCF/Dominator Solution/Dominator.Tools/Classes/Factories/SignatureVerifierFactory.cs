namespace Dominator.Tools.Classes.Factories
{
    public static class SignatureVerifierFactory
    {
        private static ISignatureVerifier signatureVerifierInstance;

        public static ISignatureVerifier NewSignatureVerifier()
        {
            return signatureVerifierInstance ?? (signatureVerifierInstance = new SignatureVerifier());
        }
    }
}
