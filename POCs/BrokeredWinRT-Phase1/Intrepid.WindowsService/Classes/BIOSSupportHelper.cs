using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Server;

namespace Intrepid.WindowsService.Classes
{
    public static class BIOSSupportHelper
    {
        private static ServiceHost serviceHost;

        public static void StartWCFService()
        {
            Uri baseAddressSystem = new Uri("net.pipe://localhost/IntrepidBIOSSupport");
            serviceHost = new ServiceHost(typeof(BIOSSupport), baseAddressSystem);
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            ServiceEndpoint endpoint = serviceHost.AddServiceEndpoint(typeof(IBIOSSupport), binding, "");
            serviceHost.Open();
        }

        public static void StopWCFService()
        {
            serviceHost.Close();
        }
    }
}