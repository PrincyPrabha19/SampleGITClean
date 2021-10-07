using System;
using Microsoft.Win32;

namespace Dominator.ServiceModel.Classes.Helpers
{
    public static class FastStartupRegistryHelper
    {
        public static bool IsFastStartupEnabled()
        {
            int value;
            if (readRegistryKeyValue("HiberbootEnabled", out value))
                return value > 0;
            return true;
        }

        public static bool SetFastStartup(bool enabled)
        {
            return writeRegistryKeyValue("HiberbootEnabled", Convert.ToInt16(enabled));
        }


        private static bool readRegistryKeyValue(string registryKey, out int value)
        {
            value = 0;

            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Power", false))
                {
                    value = Convert.ToInt32(key?.GetValue(registryKey, 1));
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
                using (var key = hklm.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Power"))
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