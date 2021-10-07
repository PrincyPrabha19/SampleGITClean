using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.BIOSSupport;
using Dominator.Tools.Classes.Security;
using System.ServiceModel;

namespace Dominator.Dell.Intrepid.Agent.Plugin
{
	public static class BIOSSupportHelper
    {
        private static ServiceHost serviceHost;

        public static void StartWCFService()
        {
#if BIOS_REGISTRY_SIMULATION
            serviceHost = WcfServiceUtil.StandupServiceHost(typeof(BIOSSupportAPIWrapperTest), typeof(IBIOSSupportAPIWrapper));
#else
            serviceHost = WcfServiceUtil.StandupServiceHost(typeof(BIOSSupportAPIWrapper), typeof(IBIOSSupportAPIWrapper));
#endif
        }

        public static void StopWCFService()
        {
            serviceHost.Close();
        }
    }
}