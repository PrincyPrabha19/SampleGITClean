using System;
using Dominator.Domain.Classes.Security;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;
using Dominator.Tools.Classes.Security;

namespace Dominator.Domain.Classes.Factories
{
    public static class EncryptionFactory
    {
        private static readonly ILogger logger = LoggerFactory.LoggerInstance;

        public static IEncryptionService NewEncryptionFactory()
        {
            return getEncryptionRemoteInstance();
        }

        private static ICryptoManager cryptoManager;
        public static ICryptoManager NewCryptoManager()
        {
            if (cryptoManager == null)
            {
                cryptoManager = new CryptoManager();
                cryptoManager.Initialize();
            }

            return cryptoManager;
        }

        private static IEncryptionService getEncryptionRemoteInstance()
        {
            if (!DominatorWindowsServiceHelper.IsRunning())
                return null;

            var baseUri = URIManager.GetURI(typeof(EncryptionService));
            var channelFactory = new ClientAgentFactory<IEncryptionService>(baseUri);

            try
            {
                var channel = channelFactory.CreateChannel();
                channel.Ping();
                return channel;
            }
            catch (Exception e)
            {
                logger?.WriteError("Unable to Ping remoting service", "Check 'DominatorWindowsService' is installed and running", e.StackTrace);
            }

            return null;
        }
    }
}
 