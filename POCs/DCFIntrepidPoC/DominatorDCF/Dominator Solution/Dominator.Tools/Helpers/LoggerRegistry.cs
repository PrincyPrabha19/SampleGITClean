using System;
using Microsoft.Win32;

namespace Dominator.Tools.Helpers
{
    public static class LoggerRegistry
    {
        public static bool IsFileLogEnabled()
        {
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(@"SOFTWARE\Alienware\OC Controls", false))
                {
                    if (key != null)
                        return Convert.ToBoolean(key.GetValue("IsFileLogEnabled", false));
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        public static bool IsEventLogEnabled()
        {
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(@"SOFTWARE\Alienware\OC Controls", false))
                {
                    if (key != null)
                        return Convert.ToBoolean(key.GetValue("IsEventLogEnabled", false));
                }
            }
            catch (Exception)
            {
            }

            return false;
        }
    }
}
