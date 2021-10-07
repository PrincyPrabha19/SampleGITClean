using System.Collections.Generic;

namespace Dominator.Domain
{
    public interface IProfileNameRepository
    {
        Dictionary<string, string> PredefinedProfileNameList { get; set; }
        Dictionary<string, string> CustomProfileNameList { get; set; }
        IProfileDiscovery ProfileDiscovery { get; set; }

        void RefreshProfileNameLists();
        void RefreshPredefinedProfileNameList();
        void RefreshCustomProfileNameList();
        string GetPredefinedProfilePath(string profileName);
        string GetCustomProfilePath(string profileName);
        string GenerateNewCustomProfileName(string profileNameFormat);
        string UpdateProfileName(string oldProfileName, string newProfileName);
        string CheckIfProfileExists(string profileName);
    }
}