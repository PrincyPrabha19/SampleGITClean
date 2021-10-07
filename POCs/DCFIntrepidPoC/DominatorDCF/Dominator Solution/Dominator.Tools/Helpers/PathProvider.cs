using System;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using Dominator.Tools.Classes.Factories;

namespace Dominator.Tools.Helpers
{
    public static class PathProvider
    {
        public static string InstallPath => readInstallPathFromRegistry();
        private static readonly ILogger logger = LoggerFactory.LoggerInstance;

        private static string readInstallPathFromRegistry()
        {
            var installPath = getDebugPath();
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(@"SOFTWARE\Alienware\OC Controls", false))
                {
                    var path = Path.GetDirectoryName(key?.GetValue("InstallPath")?.ToString());
                    if (path != null)
                        return path;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return installPath;

        }

        private static string getDebugPath()
        {
            var location = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            return !string.IsNullOrEmpty(location) ? Path.Combine(location, @"..\..\..\Dominator\bin\Debug\") : string.Empty;
        }

    }
}
