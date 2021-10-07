using System.ServiceModel;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Monitoring;
using Dominator.Tools.Classes.Security;

namespace Dominator.Dell.Intrepid.Agent.Plugin
{
    public static class XTUMonitorHelper
    {
        private static ServiceHost serviceHost;

        public static void StartWCFService()
        {
            serviceHost = WcfServiceUtil.StandupServiceHost(typeof(CPUMonitor), typeof(IMonitor<decimal>));
        }

        public static void StopWCFService()
        {
            serviceHost.Close();
        }
    }
}