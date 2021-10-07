using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes.DataSet;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    /// <summary>
    /// Everytime a new field is added to the dataset, the customer current profile may not have it and the program can fail loading the profile,
    /// so the new field "DefaultValue" can be DBNull and the "NullValue" must be specified
    /// </summary>
    public class ProfileWriterClass : ProfileWriter
    {
        #region ProfileWriter Members
        public void Write(Profile profile, string path)
        {
            var dataSet = new ProfileDataSet();
            createProfileRow(dataSet, profile);
            writeProfileDataSet(dataSet, path);
        }

        public void Write(ObservableCollection<Profile> profiles, string path)
        {
            var dataSet = new ProfileDataSet();

            foreach (var profile in profiles)
                createProfileRow(dataSet, profile);

            writeProfileDataSet(dataSet, path);
        }
        #endregion

        #region Private Methods  
        private void writeProfileDataSet(ProfileDataSet dataSet, string path)
        {
            var directory = System.IO.Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var s = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                dataSet.WriteXml(s);
            }

            var sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            var fileSecurity = File.GetAccessControl(path);
            fileSecurity.AddAccessRule(new FileSystemAccessRule(sid, FileSystemRights.FullControl, AccessControlType.Allow));
            File.SetAccessControl(path, fileSecurity);
        }

        private void createProfileRow(ProfileDataSet dataSet, Profile profile)
        {
            var profileRow = dataSet.Profile.NewProfileRow();
            profileRow.Name = profile.Name;
            profileRow.GameId = profile.GameId;            
            profileRow.GameTitle = profile.GameTitle;
            profileRow.GamePath = profile.GamePath;
            profileRow.GameRealPath = profile.GameRealPath;
            profileRow.GameInstallPath = profile.GameInstallPath;
            profileRow.GameShortcutPath = profile.GameShortcutPath;
            profileRow.GameIconPath = profile.GameIconPath;
            profileRow.SteamId = profile.SteamId;
            profileRow.SteamGamePath = profile.SteamGamePath;

            dataSet.Profile.AddProfileRow(profileRow);

            if (profile.GameModeProfileActions.ProfileActions != null)
                createProfileActions(dataSet, profileRow, profile);
        }

        private void createProfileActions(ProfileDataSet dataSet, ProfileDataSet.ProfileRow profileRow, Profile profile)
        {
            foreach (var profileAction in profile.GameModeProfileActions.ProfileActions)
            {
                var profileActionRow = dataSet.ProfileAction.NewProfileActionRow();
                profileActionRow.Name = profileAction.Name;
                profileActionRow.Type = profileAction.Type.ToString();
                profileActionRow.OrderNo = profileAction.OrderNo;
                profileActionRow.ProfileRow = profileRow;                
                dataSet.ProfileAction.AddProfileActionRow(profileActionRow);

                writeProfileActionInfosFor(dataSet, profileActionRow, profileAction);
            }
        }

        private void writeProfileActionInfosFor(ProfileDataSet dataSet, ProfileDataSet.ProfileActionRow profileActionRow, ProfileAction profileAction)
        {
            switch (profileAction.Type)
            {
                case GameModeActionType.GameApplication: 
                    writeProfileActionInfoGameApplicationFor(dataSet, profileActionRow, profileAction);
                    break;
                case GameModeActionType.Application:                                   
                case GameModeActionType.MediaPlayerApplication:
                case GameModeActionType.FrapsApplication:
                case GameModeActionType.VoIPApplication:
                    writeProfileActionInfoApplicationFor(dataSet, profileActionRow, profileAction);
                    break;
                case GameModeActionType.AdditionalApplication:
                    writeProfileActionInfoAdditionalApplicationFor(dataSet, profileActionRow, profileAction);
                    break;
                case GameModeActionType.AudioOutput:
                    writeProfileActionInfoAudioOutputFor(dataSet, profileActionRow, profileAction);
                    break;
                case GameModeActionType.WebLinks:
                    writeProfileActionInfoWebLinkFor(dataSet, profileActionRow, profileAction);
                    break;
                case GameModeActionType.PowerPlan:
                    writeProfileActionInfoPowerPlanFor(dataSet, profileActionRow, profileAction);
                    break;
                case GameModeActionType.Thermal:
                    writeProfileActionInfoThermalFor(dataSet, profileActionRow, profileAction);
                    break;
                case GameModeActionType.AlienFX:
                    writeProfileActionInfoAlienFXFor(dataSet, profileActionRow, profileAction);
                    break;
                case GameModeActionType.EnergyBooster:
                    writeProfileActionInfoEnergyBoosterFor(dataSet, profileActionRow, profileAction);
                    break;
                case GameModeActionType.PerformanceMonitoring:
                    writeProfileActionInfoPerformanceMonitoringFor(dataSet, profileActionRow, profileAction);
                    break;
            }
        }

        private void writeProfileActionInfoGameApplicationFor(ProfileDataSet dataSet, ProfileDataSet.ProfileActionRow profileActionRow, ProfileAction profileAction)
        {
            if (profileAction.ProfileActionInfo != null)
            {
                var profileActionInfoGameApplication = profileAction.ProfileActionInfo as ProfileActionInfoGameApplicationClass;
                if (profileActionInfoGameApplication != null)
                {
                    var profileActionInfoRow = dataSet.ProfileActionInfoApplication.NewProfileActionInfoApplicationRow();
                    profileActionInfoRow.ApplicationName = profileActionInfoGameApplication.ApplicationName;
                    profileActionInfoRow.ApplicationPath = profileActionInfoGameApplication.ApplicationPath;
                    profileActionInfoRow.ApplicationRealPath = profileActionInfoGameApplication.ApplicationRealPath;
                    profileActionInfoRow.ProfileActionRow = profileActionRow;

                    dataSet.ProfileActionInfoApplication.AddProfileActionInfoApplicationRow(profileActionInfoRow);
                }
            }
        }

        private void writeProfileActionInfoApplicationFor(ProfileDataSet dataSet, ProfileDataSet.ProfileActionRow profileActionRow, ProfileAction profileAction)
        {
            if (profileAction.ProfileActionInfo != null)
            {
                var profileActionInfoApplication = profileAction.ProfileActionInfo as ProfileActionInfoApplicationClass;
                if (profileActionInfoApplication != null)
                {
                    var profileActionInfoRow = dataSet.ProfileActionInfoApplication.NewProfileActionInfoApplicationRow();
                    profileActionInfoRow.ApplicationName = profileActionInfoApplication.ApplicationName;
                    profileActionInfoRow.ApplicationPath = profileActionInfoApplication.ApplicationPath;
                    profileActionInfoRow.ProfileActionRow = profileActionRow;

                    dataSet.ProfileActionInfoApplication.AddProfileActionInfoApplicationRow(profileActionInfoRow);
                }
            }
        }

        private void writeProfileActionInfoAdditionalApplicationFor(ProfileDataSet dataSet, ProfileDataSet.ProfileActionRow profileActionRow, ProfileAction profileAction)
        {
            if (profileAction.ProfileActionInfo != null)
            {
                var profileActionInfoAdditionalApplication = profileAction.ProfileActionInfo as ProfileActionInfoAdditionalApplicationClass;
                if (profileActionInfoAdditionalApplication != null)
                {
                    var profileActionInfoRow = dataSet.ProfileActionInfoAdditionalApplication.NewProfileActionInfoAdditionalApplicationRow();
                    profileActionInfoRow.ApplicationName = profileActionInfoAdditionalApplication.ApplicationName;
                    profileActionInfoRow.ApplicationPath = profileActionInfoAdditionalApplication.ApplicationPath;
                    profileActionInfoRow.LaunchIfNotOpen = profileActionInfoAdditionalApplication.LaunchIfNotOpen;
                    profileActionInfoRow.ProfileActionRow = profileActionRow;

                    dataSet.ProfileActionInfoAdditionalApplication.AddProfileActionInfoAdditionalApplicationRow(profileActionInfoRow);
                }
            }
        }

        private void writeProfileActionInfoAudioOutputFor(ProfileDataSet dataSet, ProfileDataSet.ProfileActionRow profileActionRow, ProfileAction profileAction)
        {
            if (profileAction.ProfileActionInfo != null)
            {
                var profileActionInfoAudioOutput = profileAction.ProfileActionInfo as ProfileActionInfoAudioOutputClass;
                if (profileActionInfoAudioOutput != null)
                {
                    var profileActionInfoRow = dataSet.ProfileActionInfoAudioOutput.NewProfileActionInfoAudioOutputRow();
                    profileActionInfoRow.AudioDeviceId = profileActionInfoAudioOutput.AudioDeviceId;
                    profileActionInfoRow.AudioDeviceName = profileActionInfoAudioOutput.AudioDeviceName;
                    profileActionInfoRow.ProfileActionRow = profileActionRow;
                    dataSet.ProfileActionInfoAudioOutput.AddProfileActionInfoAudioOutputRow(profileActionInfoRow);
                }
            }
        }

        private void writeProfileActionInfoWebLinkFor(ProfileDataSet dataSet, ProfileDataSet.ProfileActionRow profileActionRow, ProfileAction profileAction)
        {
            if (profileAction.ProfileActionInfo != null)
            {
                var profileActionInfoWebLink = profileAction.ProfileActionInfo as ProfileActionInfoWebLinksClass;
                if (profileActionInfoWebLink != null)
                {
                    foreach (var url in profileActionInfoWebLink.Urls)
                    {
                        var profileActionInfoRow = dataSet.ProfileActionInfoWebLink.NewProfileActionInfoWebLinkRow();
                        profileActionInfoRow.Url = url;
                        profileActionInfoRow.EnableTabbedBrowsing = profileActionInfoWebLink.EnableTabbedBrowsing;
                        profileActionInfoRow.ProfileActionRow = profileActionRow;
                        dataSet.ProfileActionInfoWebLink.AddProfileActionInfoWebLinkRow(profileActionInfoRow);
                    }
                }
            }
        }

        private void writeProfileActionInfoPowerPlanFor(ProfileDataSet dataSet, ProfileDataSet.ProfileActionRow profileActionRow, ProfileAction profileAction)
        {
            if (profileAction.ProfileActionInfo != null)
            {
                var profileActionInfoPowerPlan = profileAction.ProfileActionInfo as ProfileActionInfoPowerPlanClass;
                if (profileActionInfoPowerPlan != null)
                {
                    var profileActionInfoRow = dataSet.ProfileActionInfoPowerPlan.NewProfileActionInfoPowerPlanRow();
                    profileActionInfoRow.PowerPlanId = profileActionInfoPowerPlan.PowerPlanId;
                    profileActionInfoRow.PowerPlanName = profileActionInfoPowerPlan.PowerPlanName;
                    profileActionInfoRow.ProfileActionRow = profileActionRow;
                    dataSet.ProfileActionInfoPowerPlan.AddProfileActionInfoPowerPlanRow(profileActionInfoRow);
                }
            }            
        }

        private void writeProfileActionInfoThermalFor(ProfileDataSet dataSet, ProfileDataSet.ProfileActionRow profileActionRow, ProfileAction profileAction)
        {
            if (profileAction.ProfileActionInfo != null)
            {
                var profileActionInfoThermal = profileAction.ProfileActionInfo as ProfileActionInfoThermalClass;
                if (profileActionInfoThermal != null)
                {
                    var profileActionInfoRow = dataSet.ProfileActionInfoThermal.NewProfileActionInfoThermalRow();
                    profileActionInfoRow.ThermalProfileId = profileActionInfoThermal.ThermalProfileId;
                    profileActionInfoRow.ThermalProfileName = profileActionInfoThermal.ThermalProfileName;
                    profileActionInfoRow.ProfileActionRow = profileActionRow;
                    dataSet.ProfileActionInfoThermal.AddProfileActionInfoThermalRow(profileActionInfoRow);
                }
            }
        }

        private void writeProfileActionInfoAlienFXFor(ProfileDataSet dataSet, ProfileDataSet.ProfileActionRow profileActionRow, ProfileAction profileAction)
        {
            if (profileAction.ProfileActionInfo != null)
            {
                var profileActionInfoAlienFX = profileAction.ProfileActionInfo as ProfileActionInfoAlienFXClass;
                if (profileActionInfoAlienFX != null)
                {
                    var profileActionInfoRow = dataSet.ProfileActionInfoAlienFX.NewProfileActionInfoAlienFXRow();
                    profileActionInfoRow.Type = profileActionInfoAlienFX.Type.ToString();
                    profileActionInfoRow.ThemeName = profileActionInfoAlienFX.ThemeName;
                    profileActionInfoRow.ThemePath = profileActionInfoAlienFX.ThemePath;
                    profileActionInfoRow.ProfileActionRow = profileActionRow;
                    dataSet.ProfileActionInfoAlienFX.AddProfileActionInfoAlienFXRow(profileActionInfoRow);
                }
            }
        }

        private void writeProfileActionInfoEnergyBoosterFor(ProfileDataSet dataSet, ProfileDataSet.ProfileActionRow profileActionRow, ProfileAction profileAction)
        {
            if (profileAction.ProfileActionInfo != null)
            {
                var profileActionInfoEnergyBooster = profileAction.ProfileActionInfo as ProfileActionInfoEnergyBoosterClass;
                if (profileActionInfoEnergyBooster != null)
                {
                    var profileActionInfoRow = dataSet.ProfileAction.NewProfileActionRow();
                    dataSet.ProfileAction.AddProfileActionRow(profileActionInfoRow);
                }
            }
        }

        private void writeProfileActionInfoPerformanceMonitoringFor(ProfileDataSet dataSet, ProfileDataSet.ProfileActionRow profileActionRow, ProfileAction profileAction)
        {
            if (profileAction.ProfileActionInfo != null)
            {
                var profileActionInfoPerformanceMonitoring = profileAction.ProfileActionInfo as ProfileActionInfoPerformanceMonitoringClass;
                if (profileActionInfoPerformanceMonitoring != null)
                {
                    var profileActionInfoRow = dataSet.ProfileAction.NewProfileActionRow();
                    dataSet.ProfileAction.AddProfileActionRow(profileActionInfoRow);
                }
            }
        }
        #endregion
    }
}