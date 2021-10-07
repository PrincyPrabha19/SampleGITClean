using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileRepositoryClass : ProfileRepository
    {
        #region Public Properties
        public ProfileReader ProfileReader { get; set; }
        public ProfileCreator ProfileCreator { get; set; }
        public ProfileWriter ProfileWriter { get; set; }
        #endregion

        #region ProfileRepository Members
        public Profile CurrentProfile { get; private set; }

        private ObservableCollection<Profile> profiles;
        public ObservableCollection<Profile> Profiles
        {
            get
            {
                if (profiles == null)
                    ReadProfiles();

                return profiles;
            }
            set { profiles = value; }
        }

        public void NewProfile(string profileName)
        {
            var profile = ProfileCreator.New(profileName);
            Profiles.Add(profile);
            SetCurrentProfile(profile);
        }

        public void SaveProfile()
        {
            ProfileWriter.Write(Profiles, ProfileReader.FilePath);
        }

        public void ReadProfilesFromDisk()
        {
            ReadProfiles();
        }

        public void RefreshProfiles()
        {
            if (profiles != null)
                profiles.Clear();

            ReadProfiles();
        }

        public void SetCurrentProfile(Profile profile)
        {
            CurrentProfile = profile;
        }

        public void DeleteCurrentProfile()
        {
            try
            {
                if (File.Exists(CurrentProfile.GameShortcutPath))
                    File.Delete(CurrentProfile.GameShortcutPath);
            }
            catch
            {                
            }

            Profiles.Remove(CurrentProfile);

            SetCurrentProfile(null);           
        }

        public bool ValidateNewProfileName(string profileName, bool excludeCurrentProfile = false)
        {
            return !String.IsNullOrEmpty(profileName) && 
                GetProfile(profileName, excludeCurrentProfile ? CurrentProfile : null) == null;
        }

        public Profile GetProfile(int id)
        {
            return Profiles.FirstOrDefault(p => p.Id == id);
        }

        public Profile GetProfile(string profileName, Profile profile = null)
        {
            return Profiles.FirstOrDefault(p => (profile == null || p != profile) && String.Compare(p.Name, profileName, true) == 0);
        }
        #endregion

        #region Public Methods
        public virtual void ReadProfiles()
        {
            profiles = ProfileReader.Read();
        }

        public string GetProfileReaderFilePath()
        {
            if (ProfileReader != null)
                return ProfileReader.FilePath;

            return String.Empty;
        }
        #endregion
    }
}