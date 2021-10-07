using System.Collections.Generic;

namespace Server
{
    public interface IAlienFXDeviceDiscoveryService
    {
        DeviceSetupInfoReader RegistryDeviceSetupInfoReader { get; set; }
        DeviceSetupInfoWriter RegistryDeviceSetupInfoWriter { get; set; }
        ModelProvider ModelProvider { get; set; }
        IEnumerable<AlienFXDeviceSetupInfo> AllDevices { get; }

        IEnumerable<AlienFXDeviceSetupInfo> DiscoverDevices();
    }
}