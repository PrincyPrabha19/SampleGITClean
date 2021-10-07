using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace LoadingExternalPackage.Domain.Helper
{
    public static class ConfigurationHelper
    {
        public static List<GroupMapping> GetDeviceConfiguration(string deviceName)
        {
            var configs = new List<GroupMapping>();

            try
            {
                var filePath = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
                var json = File.ReadAllText($"{filePath}\\{deviceName}.json");
                configs = JsonConvert.DeserializeObject<List<GroupMapping>>(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return configs;
        }
    }
}
