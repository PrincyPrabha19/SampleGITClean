using System;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.BIOSSupport;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;
using Dominator.Tools.Classes.Security;

namespace Dominator.Domain.Classes.Factories
{
    public static class BIOSSupportFactory
    {
        private static readonly ILogger logger = LoggerFactory.LoggerInstance;

        private static IBIOSSupportAPIWrapper biosSupportAPIWrapper;
        public static IBIOSSupportAPIWrapper NewBIOSSupport()
        {
            return biosSupportAPIWrapper ?? (biosSupportAPIWrapper = getBIOSSupportAPIWrapperInstance());
        }

        private static IBIOSSupportAPIWrapper getBIOSSupportAPIWrapperInstance()
        {
            if (!DominatorWindowsServiceHelper.IsRunning())
                return null;

#if BIOS_REGISTRY_SIMULATION
            var baseUri = URIManager.GetURI(typeof(BIOSSupportAPIWrapperTest));
#else
            var baseUri = URIManager.GetURI(typeof(BIOSSupportAPIWrapper));
#endif

            var channelFactory = new ClientAgentFactory<IBIOSSupportAPIWrapper>(baseUri);

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