using System.Collections.ObjectModel;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles
{
    public interface ProfileRepository
    {
        ObservableCollection<Profile> Profiles { get; set; }
        Profile CurrentProfile { get; }

        void NewProfile(string profileName);
        void SaveProfile();
        void ReadProfilesFromDisk();
        void RefreshProfiles();
        void SetCurrentProfile(Profile profile);
        void DeleteCurrentProfile();
        bool ValidateNewProfileName(string profileName, bool excludeCurrentProfile = false);
        string GetProfileReaderFilePath();
        Profile GetProfile(int id);
        Profile GetProfile(string profileName, Profile profile = null);       
    }
}