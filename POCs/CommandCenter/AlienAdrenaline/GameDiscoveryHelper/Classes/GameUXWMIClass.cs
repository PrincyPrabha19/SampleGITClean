using System;
using System.Collections.Generic;
using System.Management;
using System.Text.RegularExpressions;

namespace AlienLabs.GameDiscoveryHelper.Classes
{
    public class GameUXWMIClass : GameUXWMI
    {
        public List<GameData> Discover()
        {
            var games = new List<GameData>();

            try
            {
                var connectionOptions = new ConnectionOptions();
                var managementScope = new ManagementScope(@"ROOT\CIMV2\Applications\Games", connectionOptions);

                var objectQuery = new ObjectQuery("SELECT * FROM Game");
                var managementObjectSearcher = new ManagementObjectSearcher(managementScope, objectQuery);
                var objectReturnCollection = managementObjectSearcher.Get();

                foreach (ManagementObject managementObject in objectReturnCollection)
                {
                    games.Add(getGameInfo(managementObject));
                }
            }
            catch (ManagementException e)
            {               
            }

            return games;
        }

        private GameData getGameInfo(ManagementObject managementObject)
        {
            string id = string.Empty;
            if (managementObject["InstanceID"] != null)
                id = managementObject["InstanceID"].ToString();

            string name = string.Empty;
            if (managementObject["Name"] != null)
                name = managementObject["Name"].ToString();

            string installationPath = string.Empty;
            if (managementObject["GameInstallPath"] != null)
                installationPath = managementObject["GameInstallPath"].ToString();

            string configGDFBinaryPath = string.Empty;
            if (managementObject["GDFBinaryPath"] != null)
                configGDFBinaryPath = managementObject["GDFBinaryPath"].ToString();

            string resourceIDForGDFInfo = string.Empty;
            if (managementObject["ResourceIDForGDFInfo"] != null)
                resourceIDForGDFInfo = managementObject["ResourceIDForGDFInfo"].ToString();

            string applicationExePath = String.Empty;
            if (Regex.IsMatch(configGDFBinaryPath, @"\.exe$", RegexOptions.IgnoreCase))
                applicationExePath = configGDFBinaryPath;

            return new GameDataClass()
            {
                Id = new Guid(id),
                Name = name,
                InstallationPath = installationPath,
                ConfigGDFBinaryPath = configGDFBinaryPath,
                ResourceIDForGDFInfo = resourceIDForGDFInfo,
                ApplicationExePath = applicationExePath
            };
        }
    }
}
