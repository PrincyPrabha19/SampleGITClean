using System.ServiceModel;
using Dominator.Domain;
using Dominator.Domain.Classes.Security;
using Dominator.ServiceModel;
using Dominator.Tools.Classes.Security;

namespace Dominator.Dell.Intrepid.Agent.Plugin
{
	public static class EncryptionHelper
    {
        private static ServiceHost serviceHost;

        public static void StartWCFService()
        {
            serviceHost = WcfServiceUtil.StandupServiceHost(typeof(EncryptionService), typeof(IEncryptionService));
        }

        public static void StopWCFService()
        {
            serviceHost.Close();
        }
    }
}
