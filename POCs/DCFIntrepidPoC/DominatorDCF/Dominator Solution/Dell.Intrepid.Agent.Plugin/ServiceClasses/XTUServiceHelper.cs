using System.ServiceModel;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes;
using Dominator.Tools.Classes.Security;

namespace Dominator.Dell.Intrepid.Agent.Plugin
{
    public static class XTUServiceHelper
    {
        private static ServiceHost serviceHost;

        public static void StartWCFService()
        {
            serviceHost = WcfServiceUtil.StandupServiceHost(typeof(XTUService), typeof(IXTUService));
        }

        public static void StopWCFService()
        {
            serviceHost.Close();
        }
    }
}
