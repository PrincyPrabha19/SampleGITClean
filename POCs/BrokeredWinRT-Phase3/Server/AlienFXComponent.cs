using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Server
{
    public sealed class AlienFXComponent
    {
        private IAlienFXDeviceDiscoveryService discoveryServiceInstance;

        #region Methods
        public AlienFXDeviceSetupInfo DiscoveDevices()
        {
            try
            {
                if (discoveryServiceInstance == null)
                    discoveryServiceInstance = new ObjectFactory().NewAlienFXDeviceDiscovery();

                var result = discoveryServiceInstance.DiscoverDevices();
                log($"results: {result.Count()}");
                var alienFXDeviceSetupInfos = result as AlienFXDeviceSetupInfo[] ?? result.ToArray();
                log($"alienFXDeviceSetupInfos: {alienFXDeviceSetupInfos.Length}");
                return alienFXDeviceSetupInfos[0];
            }
            catch(Exception ex)
            {
                log($"Error: {ex}");
                discoveryServiceInstance = null;
            }

            return AFXSetupInfoHelper.Empty;
        }


        private void log(string msg)
        {
            if (!Directory.Exists(@"c:\temp\")) return;

            using (var sw = new StreamWriter(new FileStream(@"c:\temp\apicalls.txt", FileMode.Append)))
                sw.WriteLine(msg);
        }
        #endregion
    }
}
