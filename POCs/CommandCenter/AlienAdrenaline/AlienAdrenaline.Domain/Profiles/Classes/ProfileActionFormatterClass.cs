using System;
using System.Globalization;
using System.IO;
using System.Linq;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileActionFormatterClass : ProfileActionFormatter
    {
        #region ProfileActionFormatter Members
        public void Format(ProfileAction profileAction, GameModeActionSummaryData gameModeActionSummaryData)
        {
            string description = String.Empty;
            string details = String.Empty;

            switch (profileAction.Type)
            {
                case GameModeActionType.GameApplication:
                    formatGameApplicationInfo(profileAction.ProfileActionInfo, out description, out details);
                    break;
                case GameModeActionType.Application:                
                case GameModeActionType.MediaPlayerApplication:
                case GameModeActionType.FrapsApplication:
                case GameModeActionType.VoIPApplication:
                    formatApplicationInfo(profileAction.ProfileActionInfo, out description, out details);
                    break;
                case GameModeActionType.AdditionalApplication:
                    formatAdditionalApplicationInfo(profileAction.ProfileActionInfo, out description, out details);
                    break;
                case GameModeActionType.AudioOutput:
                    formatAudioOutputInfo(profileAction.ProfileActionInfo, out description, out details);
                    break;
                case GameModeActionType.WebLinks:
                    formatWebLinksInfo(profileAction.ProfileActionInfo, out description, out details);
                    break;
                case GameModeActionType.PowerPlan:
                    formatPowerPlanInfo(profileAction.ProfileActionInfo, out description, out details);
                    break;
                case GameModeActionType.Thermal:
                    formatThermalInfo(profileAction.ProfileActionInfo, out description, out details);
                    break;
                case GameModeActionType.AlienFX:
                    formatAlienFXInfo(profileAction.ProfileActionInfo, out description, out details);
                    break;
                case GameModeActionType.EnergyBooster:
                    formatEnergyBoosterInfo(profileAction.ProfileActionInfo, out description, out details);
                    break;
                case GameModeActionType.PerformanceMonitoring:
                    formatPerformanceMonitoringInfo(profileAction.ProfileActionInfo, out description, out details);
                    break;
            }            

            if (String.IsNullOrEmpty(description) && String.IsNullOrEmpty(details))
            {
                gameModeActionSummaryData.ProfileActionStatus = ProfileActionStatus.NotReady;
                description = Properties.Resources.NoneText;
            }            

            gameModeActionSummaryData.ProfileActionName = String.Format("{0}:", profileAction.Name);
            gameModeActionSummaryData.ProfileActionInfoDescription = description;
            gameModeActionSummaryData.ProfileActionInfoDetails = details;
        }
        #endregion

        #region Private Methods
        private void formatGameApplicationInfo(ProfileActionInfo profileActionInfo, out string description, out string details)
        {
            description = String.Empty;
            details = String.Empty;

            var profileActionInfoGameApplication = profileActionInfo as ProfileActionInfoGameApplicationClass;
            if (profileActionInfoGameApplication != null)
            {
                description = profileActionInfoGameApplication.ApplicationName;
                details = String.Format(Properties.Resources.ApplicationPathText, profileActionInfoGameApplication.ApplicationPath);
                if (!String.IsNullOrEmpty(profileActionInfoGameApplication.ApplicationRealPath) &&
                    String.Compare(profileActionInfoGameApplication.ApplicationPath, profileActionInfoGameApplication.ApplicationRealPath, StringComparison.OrdinalIgnoreCase) != 0)
                    details += "\n" + String.Format(Properties.Resources.ApplicationRealPathText, profileActionInfoGameApplication.ApplicationRealPath);

                if (String.IsNullOrEmpty(description))
                {
                    try
                    {
                        description = Path.GetFileName(profileActionInfoGameApplication.ApplicationPath);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
        }

        private void formatApplicationInfo(ProfileActionInfo profileActionInfo, out string description, out string details)
        {
            description = String.Empty;
            details = String.Empty;

            var profileActionInfoApplication = profileActionInfo as ProfileActionInfoApplicationClass;
            if (profileActionInfoApplication != null)
            {
                description = profileActionInfoApplication.ApplicationName;                
                details = profileActionInfoApplication.ApplicationPath;

                if (String.IsNullOrEmpty(description))
                {
                    try
                    {
                        description = Path.GetFileName(profileActionInfoApplication.ApplicationPath);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
        }

        private void formatAdditionalApplicationInfo(ProfileActionInfo profileActionInfo, out string description, out string details)
        {
            description = String.Empty;
            details = String.Empty;

            var profileActionInfoAdditionalApplication = profileActionInfo as ProfileActionInfoAdditionalApplicationClass;
            if (profileActionInfoAdditionalApplication != null)
            {
                description = profileActionInfoAdditionalApplication.ApplicationName;
                details = profileActionInfoAdditionalApplication.ApplicationPath;

                if (String.IsNullOrEmpty(description))
                {
                    try
                    {
                        description = Path.GetFileName(profileActionInfoAdditionalApplication.ApplicationPath);
                    }
                    catch (ArgumentException)
                    {
                    }
                }

                if (!String.IsNullOrEmpty(details))
                    details += String.Format("\n({0})", 
                        (profileActionInfoAdditionalApplication.LaunchIfNotOpen) ? 
                            Properties.Resources.ApplicationLaunchIfNotOpenText.ToLower() : Properties.Resources.ApplicationExitIfOpenText.ToLower());
            }
        }

        private void formatAudioOutputInfo(ProfileActionInfo profileActionInfo, out string description, out string details)
        {
            description = String.Empty;
            details = String.Empty;

            var profileActionInfoAudioOutput = profileActionInfo as ProfileActionInfoAudioOutputClass;
            if (profileActionInfoAudioOutput != null)
                description = profileActionInfoAudioOutput.AudioDeviceName;
        }

        private void formatWebLinksInfo(ProfileActionInfo profileActionInfo, out string description, out string details)
        {
            description = String.Empty;
            details = String.Empty;

            var profileActionInfoWebLinks = profileActionInfo as ProfileActionInfoWebLinksClass;
            if (profileActionInfoWebLinks != null)
                details = profileActionInfoWebLinks.Urls.Aggregate(String.Empty, (current, url) => current + (url + "\n")).TrimEnd();             
        }

        private void formatPowerPlanInfo(ProfileActionInfo profileActionInfo, out string description, out string details)
        {
            description = String.Empty;
            details = String.Empty;

            var profileActionInfoPowerPlan = profileActionInfo as ProfileActionInfoPowerPlanClass;
            if (profileActionInfoPowerPlan != null)
                description = profileActionInfoPowerPlan.PowerPlanName;
        }

        private void formatThermalInfo(ProfileActionInfo profileActionInfo, out string description, out string details)
        {
            description = String.Empty;
            details = String.Empty;

            var profileActionInfoThermal = profileActionInfo as ProfileActionInfoThermalClass;
            if (profileActionInfoThermal != null)
                description = profileActionInfoThermal.ThermalProfileName;
        }

        private void formatAlienFXInfo(ProfileActionInfo profileActionInfo, out string description, out string details)
        {
            description = String.Empty;
            details = String.Empty;

            var profileActionInfoAlienFX = profileActionInfo as ProfileActionInfoAlienFXClass;
            if (profileActionInfoAlienFX != null)
            {
                string resourceKey =
                    EnumHelper.GetAttributeValue<ResourceKeyAttributeClass, string>(profileActionInfoAlienFX.Type);
                description = Properties.Resources.ResourceManager.GetString(resourceKey);

                if (profileActionInfoAlienFX.Type == AlienFXActionType.PlayTheme)
                {
                    description += " " + profileActionInfoAlienFX.ThemeName;
                    details = profileActionInfoAlienFX.ThemePath;
                }
            }               
        }

        private void formatEnergyBoosterInfo(ProfileActionInfo profileActionInfo, out string description, out string details)
        {
            description = Properties.Resources.ReadyText;
            details = String.Empty;
        }

        private void formatPerformanceMonitoringInfo(ProfileActionInfo profileActionInfo, out string description, out string details)
        {
            description = Properties.Resources.ReadyText;
            details = String.Empty;
        }
        #endregion
    }
}
