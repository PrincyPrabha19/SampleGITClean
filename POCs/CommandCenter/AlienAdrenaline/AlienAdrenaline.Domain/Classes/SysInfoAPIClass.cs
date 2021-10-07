using System;
using System.IO;
using System.Management;
using System.Reflection;
using Microsoft.Win32;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class SysInfoAPIClass : SysInfoAPI
    {
        private SysInfoAPIClass()
        {            
        }

        static SysInfoAPIClass()
        {
            readWMIInfo();
        }

        private static SysInfoAPI instance = new SysInfoAPIClass();
        public static void Plug(SysInfoAPI mockedInstance)
        {
            instance = mockedInstance;
        }

        public static void UnPlug(SysInfoAPI info)
        {
            instance = new SysInfoAPIClass();
        }

        public static Platform Platform { get { return instance.platform; } }
        private Platform backedPlatform;
        public Platform platform
        {
            get
            {
                return backedPlatform != Platform.Unknown ?
                    backedPlatform : backedPlatform = platformFrom(PCSystemType);
            }
        }

        private Platform platformFrom(ushort pcSystemType)
        {
            switch (pcSystemType)
            {
                case 1:
                case 3:
                case 4:
                    return Platform.Desktop;
                case 2: 
                    return Platform.Mobile;
                default: 
                    return Platform.Unknown;
            }
        }

        private static void readWMIInfo()
        {
            var searcher = new ManagementObjectSearcher(
                "SELECT PCSystemType, Manufacturer FROM Win32_ComputerSystem"
            );

            var collection = searcher.Get();
            if (collection.Count == 1)
            {
                try
                {
                    foreach (var o in searcher.Get())
                    {
                        var item = (ManagementObject)o;
                        PCSystemType = (ushort) item["PCSystemType"];
                        PCManufacturer = item["Manufacturer"].ToString();
                        return;
                    }
                }
                catch { }
            }
        }

        private static ushort PCSystemType { get; set; }
        public static string PCManufacturer { get; set; }

        public static string PCModel
        {
            get
            {
                try
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Alienware\\Alienware AlienFX", false);
                    if (key != null)
                    {
                        object valueTemp = key.GetValue("Model");
                        if ((valueTemp != null) && ((valueTemp as string) != null) && (valueTemp.ToString() != ""))
                            return valueTemp.ToString();
                    }
                }
                catch { }

                return Platform == Platform.Mobile ? "default" : "default_desktop";
            }
        }        

        private static string installationPath = String.Empty;
        public static string InstallationPath
        {
            get
            {
                if (String.IsNullOrEmpty(installationPath))
                {
                    try
                    {
                        RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Alienware\\Command Center\\", false);
                        if (key != null)
                        {
                            object valueTemp = key.GetValue("S1_KEY");
                            if ((valueTemp != null) && ((valueTemp as string) != null) && (valueTemp.ToString() != ""))
                                installationPath = Path.GetDirectoryName(valueTemp.ToString());
                        }
                    }
                    catch { }
                }

                return installationPath; //@"c:\program files\alienware\command center\";
            }
        }

        public static string AssemblyPath { get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); } }

        public static OS OS { get { return instance.os; } }
        public OS os
        {
            get
            {
                if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1) return OS.Win7;
                if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 2) return OS.Win8;
                if (Environment.OSVersion.Version.Major == 6) return OS.Vista;

                return OS.Unknown;
            }
        }
    }
}
