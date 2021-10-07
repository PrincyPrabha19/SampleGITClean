using System;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;
using Dominator.Tools.Classes.Security;

namespace Dominator.ServiceModel.Classes.Factories
{
    public static class ServiceRepository
    {
        private static IXTUService xtuServiceInstance;
        private static readonly ILogger logger = LoggerFactory.LoggerInstance;

        public static IXTUService XTUServiceInstance
        {
            get
            {                
                if (xtuServiceInstance == null)
                {
                    //XTULibraryLoader.TryLoadAssemblies();
                    xtuServiceInstance = getXTUServiceRemoteInstance();
                    xtuServiceInstance?.Initialize();
                }
                else
                {
                    var initializedNeeded = false;

                    try
                    {
                        xtuServiceInstance.Ping();
                    }
                    catch
                    {
                        initializedNeeded = true;
                    }

                    if (!initializedNeeded) return xtuServiceInstance;
                    xtuServiceInstance = getXTUServiceRemoteInstance();
                    xtuServiceInstance?.Initialize();
                }

                return xtuServiceInstance;
            }
        }

        private static IXTUService getXTUServiceRemoteInstance()
        {
            if (!DominatorWindowsServiceHelper.IsRunning())
                return null;

            var baseUri = URIManager.GetURI(typeof(XTUService));
            var channelFactory = new ClientAgentFactory<IXTUService>(baseUri);

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
