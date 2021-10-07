using System;
using System.Management;
using System.Runtime.InteropServices;
using Dominator.ServiceModel.Enums;
using Dominator.Tools.Enums;
using Dominator.Tools.Helpers;
using Microsoft.Win32;

namespace Dominator.ServiceModel.Classes.SystemInfo
{
    public class PlatformInfoData : IPlatformInfoData
    {
        public string Platform { get; set; }
        public PlatformType PlatformType { get; set; }
        public string Motherboard { get; set; }
        public string BiosVersion { get; set; }
        public string BiosDate { get; set; }
        public bool UEFI { get; set; }
        public OSType OS { get; set; }

        [DllImport("kernel32.dll")]
        public static extern uint GetFirmwareEnvironmentVariableA([MarshalAs(UnmanagedType.LPWStr)] string lpName, [MarshalAs(UnmanagedType.LPWStr)] string lpGuid, IntPtr pBuffer, uint nSize);


        public void Initialize()
        {
#if IS_SYSTEM_INFO_REQUIRED
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
            foreach (var queryObj in searcher.Get())
            {
                try { Motherboard = queryObj["Manufacturer"].ToString(); } catch { }
                try { BiosVersion = queryObj["Version"].ToString(); } catch { }
            }

            UEFI = isUEFIBoot();
#endif
            Platform = getPlatformFromRegistry();
            PlatformType = getPlatformTypeFromWMI();            
            OS = OSHelper.GetOSType();
        }

        private bool isUEFIBoot()
        {
            try
            {
                return GetFirmwareEnvironmentVariableA("", "{00000000-0000-0000-0000-000000000000}", IntPtr.Zero, 0) == 0;
            }
            catch (Exception)
            {                
            }

            return false;
        }

        private string getPlatformFromRegistry()
        {
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS", false))
                {
                    if (key != null)
                    {
                        var obj = key.GetValue("SystemProductName");
                        if (obj != null)
                            return obj.ToString().Replace(" ", "");
                    }
                }
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }

        private static PlatformType getPlatformTypeFromWMI()
        {
            int platformType = 0;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystem");
            foreach (var queryObj in searcher.Get())
            {
                try { platformType = Convert.ToInt32(queryObj["PCSystemType"]); } catch { }
            }

            switch (platformType)
            {
                case 1:
                case 3:
                case 4:
                    return PlatformType.Desktop;
                case 2:
                    return PlatformType.Mobile;
                default:
                    return PlatformType.Unknown;
            }
        }

        public override string ToString()
        {
            return $"Motherboard: {Motherboard}\nBiosVersion: {BiosVersion}\nBiosDate: {BiosDate}\nUEFI: {UEFI}\nOS: {OS}";
        }
    }
}