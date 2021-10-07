using System;
using Microsoft.Win32;

namespace Dominator.ServiceModel.Classes.Helpers
{
    public static class BIOSSupportRegistryHelper
    {
        public static bool ReadBIOSInitStatus(out int value)
        {
            return readRegistryKeyValue("BIOSInitStatus", out value);
        }

        public static bool ReadCPUOCStatus(out int value)
        {
            return readRegistryKeyValue("BIOSCPUOCStatus", out value);
        }

        public static bool ReadOCUIBIOSControlStatus(out int value)
        {
            return readRegistryKeyValue("BIOSOCUIControlStatus", out value);
        }

        public static bool ReadOCFailsafeFlagStatus(out int value)
        {
            return readRegistryKeyValue("BIOSFailsafeStatus", out value);
        }

        public static bool WriteBIOSInitStatus(int value)
        {
            return writeRegistryKeyValue("BIOSInitStatus", value);
        }

        public static bool WriteCPUOCStatus(int value)
        {
            return writeRegistryKeyValue("BIOSCPUOCStatus", value);
        }

        public static bool WriteOCUIBIOSControlStatus(int value)
        {
            return writeRegistryKeyValue("BIOSOCUIControlStatus", value);
        }

        public static bool WriteOCFailsafeFlagStatus(int value)
        {
            return writeRegistryKeyValue("BIOSFailsafeStatus", value);
        }

        private static bool readRegistryKeyValue(string registryKey, out int value)
        {
            value = 0;

            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(@"SOFTWARE\Alienware\OC Controls", false))
                {
                    value = Convert.ToInt32(key?.GetValue(registryKey, 0));
                    return true;
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        private static bool writeRegistryKeyValue(string registryKey, int value)
        {
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.CreateSubKey(@"SOFTWARE\Alienware\OC Controls"))
                {
                    key?.SetValue(registryKey, value);
                    return true;
                }
            }
            catch (Exception)
            {
            }

            return false;
        }
    }
}
