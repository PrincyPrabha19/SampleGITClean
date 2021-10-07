using System;
using System.ServiceModel;
using Dominator.Tools.AutomaticUpdateService;
using Microsoft.Win32;

namespace Dominator.Tools.Classes.Factories
{
    public static class ServiceSoapFactory
    {
        public static CommandCenterServiceSoap NewCommandCenterServiceSoap()
        {
            string url;
            var testingMode = readTestingModeFromRegistry();
            switch (testingMode)
            {
                case "Test":
                    url = "http://support.alienware.com/commandcentertest/commandcentersrvc.asmx";
                    break;
                default:
                    url = "http://support.alienware.com/commandcenter/commandcentersrvc.asmx";
                    break;
            }

            var serviceSoapClient = new CommandCenterServiceSoapClient(new BasicHttpBinding(), new EndpointAddress(url));
            serviceSoapClient.Endpoint.Behaviors.Add(new HttpUserAgentEndpointBehavior("alienware"));
            serviceSoapClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 5);
            return serviceSoapClient;
        }

        private static string readTestingModeFromRegistry()
        {
            var testingMode = string.Empty;

            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(@"SOFTWARE\Alienware\OC Controls", false))
                {
                    testingMode = key?.GetValue("TestingMode")?.ToString();
                }
            }
            catch (Exception)
            {
            }

            return testingMode;
        }
    }
}
