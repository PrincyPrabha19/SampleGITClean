using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes.DataSet;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    /// <summary>
    /// Everytime a new field is added to the dataset, the customer current profile may not have it and the program can fail loading the profile,
    /// so the new field "DefaultValue" can be DBNull and the "NullValue" must be specified
    /// </summary>
    public class ProfileReaderClass : ProfilesPathHelper, ProfileReader
    {
        #region ProfileReader Members
        public string FilePath { get { return ProfilesFile; } }

        public ObservableCollection<Profile> Read()
        {
            var profileDataSet = getProfileDataSet(FilePath);
            if (profileDataSet == null)
                return new ObservableCollection<Profile>();

            var profiles = new ObservableCollection<Profile>();
            foreach (ProfileDataSet.ProfileRow profileRow in profileDataSet.Profile.Rows)
                profiles.Add(newProfileFor(profileRow));

            return profiles;
        }

        public Profile ReadProfile(string path)
        {
            var profileData = getProfileDataSet(path);
            if (profileData == null)
                return null;

            var profileRow = profileData.Profile.Rows[0] as ProfileDataSet.ProfileRow;
            if (profileRow == null)
                return null;

            return newProfileFor(profileRow);
        }
        #endregion

        #region Private Methods
        private Profile newProfileFor(ProfileDataSet.ProfileRow profileRow)
        {
            var profile = new ProfileClass
                              {
                                  Id = profileRow.Id,
                                  Name = profileRow.Name,
                                  GameId = profileRow.GameId,                                  
                                  GameTitle = profileRow.GameTitle,
                                  GamePath = profileRow.GamePath,
                                  GameRealPath = profileRow.GameRealPath,
                                  GameInstallPath = profileRow.GameInstallPath,
                                  GameShortcutPath = profileRow.GameShortcutPath,
                                  GameIconPath = profileRow.GameIconPath,
                                  SteamId = profileRow.SteamId,
                                  SteamGamePath = profileRow.SteamGamePath                                  
                              };

            readProfileActionsFor(profileRow, profile);

            return profile;
        }

        private void readProfileActionsFor(ProfileDataSet.ProfileRow profileRow, ProfileClass profile)
        {
            foreach (var profileActionRow in profileRow.GetProfileActionRows())
            {
                var profileAction = new ProfileActionClass();
                profileAction.Id = profileActionRow.Id;
                profileAction.Name = profileActionRow.Name;

                GameModeActionType gameModeActionType;
                if (Enum.TryParse(profileActionRow.Type, true, out gameModeActionType))
                    profileAction.Type = gameModeActionType;                

                profileAction.OrderNo = profileActionRow.OrderNo;

                readProfileActionInfosFor(profileActionRow, profileAction);

                profile.GameModeProfileActions.ProfileActions.Add(profileAction);
            }

            var profileActions = profile.GameModeProfileActions.ProfileActions;
            profile.GameModeProfileActions.ProfileActions = (from pa in profileActions
                                                             orderby pa.OrderNo
                                                             select pa).ToObservableCollection();
        }

        private void readProfileActionInfosFor(ProfileDataSet.ProfileActionRow profileActionRow, ProfileActionClass profileAction)
        {
            switch (profileAction.Type)
            {
                case GameModeActionType.GameApplication:            
                    readProfileActionInfoGameApplicationFor(profileActionRow, profileAction);
                    break;
                case GameModeActionType.Application:
                case GameModeActionType.MediaPlayerApplication:
                case GameModeActionType.FrapsApplication:
                case GameModeActionType.VoIPApplication:
                    readProfileActionInfoApplicationFor(profileActionRow, profileAction);
                    break;
                case GameModeActionType.AdditionalApplication:
                    readProfileActionInfoAdditionalApplicationFor(profileActionRow, profileAction);
                    break;
                case GameModeActionType.AudioOutput:
                    readProfileActionInfoAudioOutputFor(profileActionRow, profileAction);
                    break;
                case GameModeActionType.WebLinks:
                    readProfileActionInfoWebLinkFor(profileActionRow, profileAction);
                    break;
                case GameModeActionType.PowerPlan:
                    readProfileActionInfoPowerPlanFor(profileActionRow, profileAction);
                    break;
                case GameModeActionType.Thermal:
                    readProfileActionInfoThermalFor(profileActionRow, profileAction);
                    break;
                case GameModeActionType.AlienFX:
                    readProfileActionInfoAlienFXFor(profileActionRow, profileAction);
                    break;
                case GameModeActionType.EnergyBooster:
                    readProfileActionInfoEnergyBoosterFor(profileActionRow, profileAction);
                    break;
                case GameModeActionType.PerformanceMonitoring:
                    readProfileActionInfoPerformanceMonitoringFor(profileActionRow, profileAction);
                    break;
            }
        }

        private void readProfileActionInfoGameApplicationFor(ProfileDataSet.ProfileActionRow profileActionRow, ProfileActionClass profileAction)
        {
            foreach (var profileActionInfoRow in profileActionRow.GetProfileActionInfoApplicationRows())
            {
                var profileActionInfo = new ProfileActionInfoGameApplicationClass();
                profileActionInfo.Id = profileActionInfoRow.Id;
                profileActionInfo.ApplicationName = profileActionInfoRow.ApplicationName;
                profileActionInfo.ApplicationPath = profileActionInfoRow.ApplicationPath;
                profileActionInfo.ApplicationRealPath = profileActionInfoRow.ApplicationRealPath;
                profileAction.ProfileActionInfo = profileActionInfo;
            }
        }

        private void readProfileActionInfoApplicationFor(ProfileDataSet.ProfileActionRow profileActionRow, ProfileActionClass profileAction)
        {
            foreach (var profileActionInfoRow in profileActionRow.GetProfileActionInfoApplicationRows())
            {
                var profileActionInfo = new ProfileActionInfoApplicationClass();
                profileActionInfo.Id = profileActionInfoRow.Id;
                profileActionInfo.ApplicationName = profileActionInfoRow.ApplicationName;
                profileActionInfo.ApplicationPath = profileActionInfoRow.ApplicationPath;
                profileAction.ProfileActionInfo = profileActionInfo;
            }
        }

        private void readProfileActionInfoAdditionalApplicationFor(ProfileDataSet.ProfileActionRow profileActionRow, ProfileActionClass profileAction)
        {            
            foreach (var profileActionInfoRow in profileActionRow.GetProfileActionInfoAdditionalApplicationRows())
            {
                var profileActionInfo = new ProfileActionInfoAdditionalApplicationClass();
                profileActionInfo.Id = profileActionInfoRow.Id;
                profileActionInfo.ApplicationName = profileActionInfoRow.ApplicationName;
                profileActionInfo.ApplicationPath = profileActionInfoRow.ApplicationPath;
                profileActionInfo.LaunchIfNotOpen = profileActionInfoRow.LaunchIfNotOpen;
                profileAction.ProfileActionInfo = profileActionInfo;
            }
        }

        private void readProfileActionInfoAudioOutputFor(ProfileDataSet.ProfileActionRow profileActionRow, ProfileActionClass profileAction)
        {
            foreach (var profileActionInfoRow in profileActionRow.GetProfileActionInfoAudioOutputRows())
            {
                var profileActionInfo = new ProfileActionInfoAudioOutputClass();
                profileActionInfo.Id = profileActionInfoRow.Id;                
                profileActionInfo.AudioDeviceId = profileActionInfoRow.AudioDeviceId;
                profileActionInfo.AudioDeviceName = profileActionInfoRow.AudioDeviceName;
                profileAction.ProfileActionInfo = profileActionInfo;
            }
        }

        private void readProfileActionInfoWebLinkFor(ProfileDataSet.ProfileActionRow profileActionRow, ProfileActionClass profileAction)
        {
            var profileActionInfo = new ProfileActionInfoWebLinksClass();
            profileActionInfo.Urls = new ObservableCollection<string>();
            foreach (var profileActionInfoRow in profileActionRow.GetProfileActionInfoWebLinkRows())
            {
                profileActionInfo.Urls.Add(profileActionInfoRow.Url);
                profileActionInfo.EnableTabbedBrowsing &= profileActionInfoRow.EnableTabbedBrowsing;                
            }                
            profileAction.ProfileActionInfo = profileActionInfo;
        }

        private void readProfileActionInfoPowerPlanFor(ProfileDataSet.ProfileActionRow profileActionRow, ProfileActionClass profileAction)
        {
            foreach (var profileActionInfoRow in profileActionRow.GetProfileActionInfoPowerPlanRows())
            {
                var profileActionInfo = new ProfileActionInfoPowerPlanClass();
                profileActionInfo.Id = profileActionInfoRow.Id;
                profileActionInfo.PowerPlanId = profileActionInfoRow.PowerPlanId;
                profileActionInfo.PowerPlanName = profileActionInfoRow.PowerPlanName;
                profileAction.ProfileActionInfo = profileActionInfo;
            }
        }

        private void readProfileActionInfoThermalFor(ProfileDataSet.ProfileActionRow profileActionRow, ProfileActionClass profileAction)
        {
            foreach (var profileActionInfoRow in profileActionRow.GetProfileActionInfoThermalRows())
            {
                var profileActionInfo = new ProfileActionInfoThermalClass();
                profileActionInfo.Id = profileActionInfoRow.Id;
                profileActionInfo.ThermalProfileId = profileActionInfoRow.ThermalProfileId;
                profileActionInfo.ThermalProfileName = profileActionInfoRow.ThermalProfileName;
                profileAction.ProfileActionInfo = profileActionInfo;
            }
        }

        private void readProfileActionInfoAlienFXFor(ProfileDataSet.ProfileActionRow profileActionRow, ProfileActionClass profileAction)
        {
            foreach (var profileActionInfoRow in profileActionRow.GetProfileActionInfoAlienFXRows())
            {
                var profileActionInfo = new ProfileActionInfoAlienFXClass();
                profileActionInfo.Id = profileActionInfoRow.Id;

                AlienFXActionType alienFXActionType;
                if (Enum.TryParse(profileActionInfoRow.Type, true, out alienFXActionType))
                    profileActionInfo.Type = alienFXActionType;

                profileActionInfo.ThemeName = profileActionInfoRow.ThemeName;
                profileActionInfo.ThemePath = profileActionInfoRow.ThemePath;
                profileAction.ProfileActionInfo = profileActionInfo;
            }
        }

        private void readProfileActionInfoEnergyBoosterFor(ProfileDataSet.ProfileActionRow profileActionRow, ProfileActionClass profileAction)
        {
            var profileActionInfo = new ProfileActionInfoEnergyBoosterClass();
            profileActionInfo.Id = profileActionRow.Id;
            profileAction.ProfileActionInfo = profileActionInfo;
        }

        private void readProfileActionInfoPerformanceMonitoringFor(ProfileDataSet.ProfileActionRow profileActionRow, ProfileActionClass profileAction)
        {
            var profileActionInfo = new ProfileActionInfoPerformanceMonitoringClass();
            profileActionInfo.Id = profileActionRow.Id;
            profileAction.ProfileActionInfo = profileActionInfo;
        }


        private ProfileDataSet getProfileDataSet(string path)
        {
            ProfileDataSet data = null;

            try
            {
                if (File.Exists(path))
                {
                    data = new ProfileDataSet();
                    using (var s = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        data.ReadXml(s);
                    }

                    return data;
                }
            }
            catch
            {
                data = null;
            }

            return data;
        }
        #endregion
    }
}