using System;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Classes.Monitoring;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;
using Dominator.Tools.Classes.Security;

namespace Dominator.Domain.Classes.Factories
{
    public static class MonitorFactory
    {
        private static readonly ILogger logger = LoggerFactory.LoggerInstance;

        public static IMonitor<decimal> NewCPUMonitor()
        {
            return getMonitorRemoteInstance();
        }

        private static IMonitorManager monitorManager;
        public static IMonitorManager NewMonitorManager()
        {
            return monitorManager ?? (monitorManager = new MonitorManager());
        }

        private static IMonitor<decimal> getMonitorRemoteInstance()
        {
            if (!DominatorWindowsServiceHelper.IsRunning())
                return null;

            var baseUri = URIManager.GetURI(typeof(CPUMonitor));
            var channelFactory = new ClientAgentFactory<IMonitor<decimal>>(baseUri);

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
 