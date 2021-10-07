using System.Collections.Generic;
using System.IO;

namespace Dominator.Domain.Classes.Profiles
{
    public class ProfileNameRepository : IProfileNameRepository
    {
        public Dictionary<string, string> PredefinedProfileNameList { get; set; }
        public Dictionary<string, string> CustomProfileNameList { get; set; }
        public IProfileDiscovery ProfileDiscovery { get; set; }

        public void RefreshProfileNameLists()
        {
            RefreshPredefinedProfileNameList();
            RefreshCustomProfileNameList();
        }

        public void RefreshPredefinedProfileNameList()
        {
            PredefinedProfileNameList = ProfileDiscovery?.DiscoverPredefinedProfiles();
        }

        public void RefreshCustomProfileNameList()
        {
            CustomProfileNameList = ProfileDiscovery?.DiscoverCustomProfiles();
        }

        public string GetPredefinedProfilePath(string profileName)
        {
            if (PredefinedProfileNameList.ContainsKey(profileName))
                return PredefinedProfileNameList[profileName];
            return null;
        }

        public string GetCustomProfilePath(string profileName)
        {
            if (profileName == null) return null;
            RefreshCustomProfileNameList();
            if (CustomProfileNameList.ContainsKey(profileName))
                return CustomProfileNameList[profileName];
            return null;
        }

        public string GenerateNewCustomProfileName(string profileNameFormat)
        {
            string newCustomProfileName = string.Empty;
            var profilesPathProvider = new ProfilesPathProvider();

            var i = 1;
            do
            {
                var profileName = string.Format(profileNameFormat, i);
                if (!PredefinedProfileNameList.ContainsKey(profileName) && !CustomProfileNameList.ContainsKey(profileName))
                {
                    var profilePath = Path.Combine(profilesPathProvider.ProfilesPath, $"{profileName}.{profilesPathProvider.getCustomProfileExtension()}");
                    if (!File.Exists(profilePath))
                        newCustomProfileName = profileName;
                }
            }
            while (string.IsNullOrEmpty(newCustomProfileName) && i++ < 10);

            return newCustomProfileName;
        }

        public string UpdateProfileName(string oldProfileName, string newProfileName)
        {
            var profilesPathProvider = new ProfilesPathProvider();
            
            var newfilePath = Path.Combine(profilesPathProvider.ProfilesPath, $"{newProfileName}.{profilesPathProvider.getCustomProfileExtension()}");
            var oldfilePath = GetCustomProfilePath(oldProfileName);
            if(!File.Exists(oldfilePath)) return newfilePath;
            if (File.Exists(newfilePath)) return oldfilePath;           
            File.Move(oldfilePath, newfilePath);
            return newfilePath;
        }

        public string CheckIfProfileExists(string profileName)
        {
         var customProfileList = ProfileDiscovery?.DiscoverCustomProfiles();
         var newProfileName = profileName;
         var i = 0;
         while(customProfileList != null && customProfileList.ContainsKey(newProfileName))
           {
               i++;
               newProfileName += "(" + i + ")";
           }
         return i>0?profileName + "(" + i + ")":profileName;
        }
    }
}