using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public sealed class AlienFXDeviceDiscoveryServiceClass : IAlienFXDeviceDiscoveryService
    {
        public DeviceSetupInfoReader RegistryDeviceSetupInfoReader { get; set; }
        public DeviceSetupInfoWriter RegistryDeviceSetupInfoWriter { get; set; }

        public ModelProvider ModelProvider { get; set; }

        private IEnumerable<AlienFXDeviceSetupInfo> allDevices;
        public IEnumerable<AlienFXDeviceSetupInfo> AllDevices => allDevices ?? (allDevices = DiscoverDevices());

        public IEnumerable<AlienFXDeviceSetupInfo> DiscoverDevices()
        {
            var installedDevicesInfo = findInstalledDevicesInfo();
            var presentDevicesInfo = findPresentDevicesInfo();

            return discoverDevices(installedDevicesInfo, presentDevicesInfo);
        }

        private IEnumerable<AlienFXDeviceSetupInfo> discoverDevices(IEnumerable<AlienFXDeviceSetupInfo> installedDevicesInfo, List<AlienFXDeviceSetupInfo> presentDevicesInfo)
        {
            return merge(installedDevicesInfo, presentDevicesInfo);
        }


        private List<AlienFXDeviceSetupInfo> findPresentDevicesInfo()
        {
            var presentDevices = new List<AlienFXDeviceSetupInfo>();
            addSytemInfoBasedOnModelTo(presentDevices);
            return presentDevices;
        }

        private void addSytemInfoBasedOnModelTo(List<AlienFXDeviceSetupInfo> presentDevices)
        {
            var deviceInfo = findSystemInfoByModel();
            if (AFXSetupInfoHelper.AreEqual(deviceInfo, AFXSetupInfoHelper.Empty))
                return;

            if (!presentDevices.Exists(entry => entry.ProductId.Equals(deviceInfo.ProductId)))
                presentDevices.Add(deviceInfo);
        }

        private AlienFXDeviceSetupInfo findSystemInfoByModel()
        {
            var model = ModelProvider.FromRegistry;
            if (string.IsNullOrEmpty(model))
                return AFXSetupInfoHelper.Empty;

            switch (model.Replace(" ",""))
            {
                case "Andromeda":
                case "AlienwareX51":
                    var x51Info = AlienFXDeviceSetupInfoFactory.NewAlienFXDeviceSetupInfo(VendorId.ALIENWARE, DeviceId.ANDROMEDA);
                    x51Info.IsPresent = true;
                    return x51Info;
                case "AlienwareX51R3":
                    var x51R3Info = AlienFXDeviceSetupInfoFactory.NewAlienFXDeviceSetupInfo(VendorId.ALIENWARE, DeviceId.X51R3);
                    x51R3Info.IsPresent = true;
                    return x51R3Info;
                case "ASM100":
                    var asm100Info = AlienFXDeviceSetupInfoFactory.NewAlienFXDeviceSetupInfo(VendorId.ALIENWARE, DeviceId.ASM100);
                    asm100Info.IsPresent = true;
                    return asm100Info;
                case "ASM200":
                    var asm200 = AlienFXDeviceSetupInfoFactory.NewAlienFXDeviceSetupInfo(VendorId.ALIENWARE, DeviceId.ASM200);
                    asm200.IsPresent = true;
                    return asm200;
                case "ASM201":
                    var asm201 = AlienFXDeviceSetupInfoFactory.NewAlienFXDeviceSetupInfo(VendorId.ALIENWARE, DeviceId.ASM201);
                    asm201.IsPresent = true;
                    return asm201;
                case "AlienwareAuroraR5":
                    var auroraR5Info = AlienFXDeviceSetupInfoFactory.NewAlienFXDeviceSetupInfo(VendorId.ALIENWARE, DeviceId.AURORA_R5);
                    auroraR5Info.IsPresent = true;
                    return auroraR5Info;
            }

            return AFXSetupInfoHelper.Empty;
        }

        private IEnumerable<AlienFXDeviceSetupInfo> findInstalledDevicesInfo()
        {
            return RegistryDeviceSetupInfoReader.Find();
        }

        private IEnumerable<AlienFXDeviceSetupInfo> merge(IEnumerable<AlienFXDeviceSetupInfo> installedDevices, IEnumerable<AlienFXDeviceSetupInfo> presentDevices)
        {
            var allDevicesInfo = installedDevices;
            foreach (var presentDevice in presentDevices)
                mergeIntoAllDevicesInfo(ref allDevicesInfo, presentDevice);

            return allDevicesInfo;
        }

        private AlienFXDeviceSetupInfo find(IEnumerable<AlienFXDeviceSetupInfo> allDevicesInfo, AlienFXDeviceSetupInfo presentDevice)
        {
            // TODO: productid and vendorid cannot be null!!!!!!!!!!!!!!!!!! and defaul is null
            return allDevicesInfo.FirstOrDefault(entry => entry.ProductId.Equals(presentDevice.ProductId));
        }

        private void mergeIntoAllDevicesInfo(ref IEnumerable<AlienFXDeviceSetupInfo> allDevicesInfo, AlienFXDeviceSetupInfo presentDevice)
        {
            var setupInfo = find(allDevicesInfo, presentDevice);
            if (AFXSetupInfoHelper.AreEqual(setupInfo, AFXSetupInfoHelper.Empty))
                allDevicesInfo = addAsInstalled(allDevicesInfo.ToList(), presentDevice);
            else
                setupInfo.IsPresent = true;
        }

        private AlienFXDeviceSetupInfo mind(List<AlienFXDeviceSetupInfo> allDevicesInfo, AlienFXDeviceSetupInfo presentDevice)
        {
            return allDevicesInfo.Find(entry => entry.ProductId.Equals(presentDevice.ProductId));
        }

        private IEnumerable<AlienFXDeviceSetupInfo> addAsInstalled(List<AlienFXDeviceSetupInfo> deviceInfoList, AlienFXDeviceSetupInfo presentDevice)
        {
            presentDevice.IsInstalled = true;

            deviceInfoList.Add(presentDevice);
            return deviceInfoList;

        }
    }
}