
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes.Factories;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileServiceClass : ProfileService
    {
        #region ProfileService Properties
        public ProfileRepository ProfileRepository { get; set; }
        public ProfileActionCreator ProfileActionCreator { get; set; }
        public Profile Profile { get; private set; }
        public ProfileAction CurrentProfileAction { get; private set; }

        public GameModeProfileActions GameModeProfileActions
        {
            get { return Profile.GameModeProfileActions; }
            set { Profile.GameModeProfileActions = value; }
        }
        #endregion

        #region ProfileService Methods
        public void Initialize()
        {
            ProfileRepository = ProfileRepositoryFactory.ProfileRepository;
            ProfileActionCreator = new ProfileActionCreatorClass();
        }

        public void SetCurrentProfile(Profile profile)
        {
            Profile = profile;
            ProfileRepository.SetCurrentProfile(profile);
        }

        public void SetCurrentProfileAction(ProfileAction profileAction)
        {
            CurrentProfileAction = profileAction;
        }

        public void SetProfileActionInfoApplication(string applicationName, string applicationPath, Guid guid)
        {
            var profileActionInfo = GetProfileActionInfo(guid) as ProfileActionInfoApplicationClass;
            if (profileActionInfo != null)
            {
                profileActionInfo.ApplicationName = applicationName;
                profileActionInfo.ApplicationPath = applicationPath;

                return;
            }

            //throw new InvalidCastException("Invalid cast to ProfileActionInfoApplicationClass");
        }

        public void SetProfileActionInfoGameApplication(string applicationName, string applicationPath, string applicationRealPath, Guid guid)
        {
            var profileActionInfo = GetProfileActionInfo(guid) as ProfileActionInfoGameApplicationClass;
            if (profileActionInfo != null)
            {
                profileActionInfo.ApplicationName = applicationName;
                profileActionInfo.ApplicationPath = applicationPath;
                profileActionInfo.ApplicationRealPath = applicationRealPath;

                return;
            }

            //throw new InvalidCastException("Invalid cast to ProfileActionInfoApplicationClass");
        }

        public void SetProfileActionInfoAdditionalApplication(string applicationName, string applicationPath, bool launchIfNotOpen, Guid guid)
        {
            var profileActionInfo = GetProfileActionInfo(guid) as ProfileActionInfoAdditionalApplicationClass;
            if (profileActionInfo != null)
            {
                profileActionInfo.ApplicationName = applicationName;
                profileActionInfo.ApplicationPath = applicationPath;
                profileActionInfo.LaunchIfNotOpen = launchIfNotOpen;

                return;
            }

            //throw new InvalidCastException("Invalid cast to ProfileActionInfoAdditionalApplicationClass");
        }

        public void SetProfileActionInfoAudioOutput(string audioDeviceId, string audioDeviceName, Guid guid)
        {
            var profileActionInfo = GetProfileActionInfo(guid) as ProfileActionInfoAudioOutputClass;
            if (profileActionInfo != null)
            {
                profileActionInfo.AudioDeviceId = audioDeviceId;
                profileActionInfo.AudioDeviceName = audioDeviceName;

                return;
            }

            //throw new InvalidCastException("Invalid cast to ProfileActionInfoAudioOutputClass");
        }

        public void SetProfileActionInfoWebLinks(ObservableCollection<string> urls, bool enableTabbedBrowsing, Guid guid)
        {
            var profileActionInfo = GetProfileActionInfo(guid) as ProfileActionInfoWebLinksClass;
            if (profileActionInfo != null)
            {
                profileActionInfo.Urls = new ObservableCollection<string>(urls);
                profileActionInfo.EnableTabbedBrowsing = enableTabbedBrowsing;
                return;
            }

            //throw new InvalidCastException("Invalid cast to ProfileActionInfoWebLinksClass");
        }

        public void SetProfileActionInfoPowerPlan(Guid powerPlanId, string powerPlanName, Guid guid)
        {
            var profileActionInfo = GetProfileActionInfo(guid) as ProfileActionInfoPowerPlanClass;
            if (profileActionInfo != null)
            {
                profileActionInfo.PowerPlanId = powerPlanId;
                profileActionInfo.PowerPlanName = powerPlanName;

                return;
            }

            //throw new InvalidCastException("Invalid cast to ProfileActionInfoPowerPlanClass");
        }

        public void SetProfileActionInfoThermal(Guid thermalProfileId, string thermalProfileName, Guid guid)
        {
            var profileActionInfo = GetProfileActionInfo(guid) as ProfileActionInfoThermalClass;
            if (profileActionInfo != null)
            {
                profileActionInfo.ThermalProfileId = thermalProfileId;
                profileActionInfo.ThermalProfileName = thermalProfileName;

                return;
            }

            //throw new InvalidCastException("Invalid cast to ProfileActionInfoPowerPlanClass");
        }

        public void SetProfileActionInfoAlienFX(AlienFXActionType type, string themeName, string themePath, Guid guid)
        {
            var profileActionInfo = GetProfileActionInfo(guid) as ProfileActionInfoAlienFXClass;
            if (profileActionInfo != null)
            {
                profileActionInfo.Type = type;
                profileActionInfo.ThemeName = themeName;
                profileActionInfo.ThemePath = themePath;

                return;
            }

            //throw new InvalidCastException("Invalid cast to ProfileActionInfoPowerPlanClass");
        }

        public void SetProfileActionInfoEnergyBooster(Guid guid)
        {
        }

        public void SetProfileActionInfoPerformanceMonitoring(Guid guid)
        {
        }

        public ProfileActionInfo GetProfileActionInfo(Guid guid)
        {
            var profileAction = GameModeProfileActions.ToList().Find(pa => pa.ProfileActionInfo.Guid == guid);
            if (profileAction != null)
                return profileAction.ProfileActionInfo;

            return null;
        }

        public Profile NewProfile(string name)
        {
            ProfileRepository.NewProfile(name);
            SetCurrentProfile(ProfileRepository.CurrentProfile);
            return Profile;
        }

        public ProfileAction NewProfileAction(string name, GameModeActionType type)
        {
            return ProfileActionCreator.New(name, type,
                EnumHelper.GetAttributeValue<InitializeOnCreationAttributeClass, bool>(type));
        }

        public void AddProfileAction(ProfileAction profileAction)
        {
            GameModeProfileActions.AddProfileAction(profileAction);
        }

        public GameModeProfileActions CloneGameModeProfileActions()
        {
            return GameModeProfileActions.Clone();
        }

        public void ReplaceGameModeProfileActions(GameModeProfileActions gameModeProfileActions)
        {
            GameModeProfileActions.ReplaceProfileActions(gameModeProfileActions);
        }

        public string GetValidGameModeName(string gameModeName, bool excludeCurrentProfile = false)
        {
            if (!String.IsNullOrEmpty(gameModeName))                
                gameModeName = Regex.Replace(gameModeName, @"[\\/:\*\?\<\>\|""]", String.Empty);

            string result = gameModeName;
            if (ProfileRepository != null)
            {
                int i = 0;
                while (!ProfileRepository.ValidateNewProfileName(result, excludeCurrentProfile) && i < 100)
                    result = String.Format("{0} ({1})", gameModeName, ++i);
            }

            return result;
        }
        #endregion
    }
}
