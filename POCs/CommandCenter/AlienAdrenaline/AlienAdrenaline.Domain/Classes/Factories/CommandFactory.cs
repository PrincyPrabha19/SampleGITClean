using System;
using System.Collections.ObjectModel;
using AlienLabs.AlienAdrenaline.Domain.Classes.Commands;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Factories
{
    public class CommandFactory
    {
        public static EquatableCommand NewGameModeProfileActionsCommand(GameModeProfileActions gameModeProfileActions)
        {
            return new GameModeProfileActionsCommand
            {
                GameModeProfileActions = gameModeProfileActions,
                ProfileService = ServiceFactory.ProfileService
            };
        }

        public static EquatableCommand NewGameModeProfileActionInfoCommand(ProfileActionInfo profileActionInfo, GameModeActionType type)
        {
            switch (type)
            {
                case GameModeActionType.GameApplication:
                    var profileActionInfoGameApplication = profileActionInfo as ProfileActionInfoGameApplicationClass;
                    if (profileActionInfoGameApplication == null)
						return null;

                    return NewGameModeGameApplicationCommand(
                                profileActionInfoGameApplication.ApplicationName,
                                profileActionInfoGameApplication.ApplicationPath,
                                profileActionInfoGameApplication.ApplicationRealPath,
                                profileActionInfoGameApplication.Guid);
                case GameModeActionType.Application:                
                case GameModeActionType.MediaPlayerApplication:
                case GameModeActionType.FrapsApplication:
                case GameModeActionType.VoIPApplication:
                    var profileActionInfoApplication = profileActionInfo as ProfileActionInfoApplicationClass;
					if (profileActionInfoApplication == null)
						return null;

                    return NewGameModeApplicationCommand(
                                profileActionInfoApplication.ApplicationName, 
                                profileActionInfoApplication.ApplicationPath, 
                                profileActionInfoApplication.Guid);

                case GameModeActionType.AdditionalApplication:
                    var profileActionInfoAdditionalApplication = profileActionInfo as ProfileActionInfoAdditionalApplicationClass;
					if (profileActionInfoAdditionalApplication == null)
						return null;

                    return NewGameModeAdditionalApplicationCommand(
                                profileActionInfoAdditionalApplication.ApplicationName, 
                                profileActionInfoAdditionalApplication.ApplicationPath,
                                profileActionInfoAdditionalApplication.LaunchIfNotOpen, 
                                profileActionInfoAdditionalApplication.Guid);

                case GameModeActionType.AudioOutput:
                    var profileActionInfoAudioOutput = profileActionInfo as ProfileActionInfoAudioOutputClass;
					if (profileActionInfoAudioOutput == null)
						return null;

                    return NewGameModeAudioOutputCommand(
                                profileActionInfoAudioOutput.AudioDeviceId, 
                                profileActionInfoAudioOutput.AudioDeviceName, 
                                profileActionInfoAudioOutput.Guid);

                case GameModeActionType.WebLinks:
                    var profileActionInfoWebLinks = profileActionInfo as ProfileActionInfoWebLinksClass;
					if (profileActionInfoWebLinks == null)
						return null;


                    return NewGameModeWebLinksCommand(
                                profileActionInfoWebLinks.Urls,
                                profileActionInfoWebLinks.EnableTabbedBrowsing,
                                profileActionInfoWebLinks.Guid);
            }

            return null;
        }

        public static EquatableCommand NewGameModeGameApplicationCommand(string applicationName, string applicationPath, string applicationRealPath, Guid guid)
        {
            return new GameModeGameApplicationCommand
            {
                ApplicationName = applicationName,
                ApplicationPath = applicationPath,
                ApplicationRealPath = applicationRealPath,
                Guid = guid,
                ProfileService = ServiceFactory.ProfileService
            };
        }

        public static EquatableCommand NewGameModeApplicationCommand(string applicationName, string applicationPath, Guid guid)
        {
            return new GameModeApplicationCommand
            {
                ApplicationName = applicationName,
                ApplicationPath = applicationPath,
                Guid = guid,
                ProfileService = ServiceFactory.ProfileService
            };
        }

        public static EquatableCommand NewGameModeAdditionalApplicationCommand(string applicationName, string applicationPath, bool launchIfNotOpen, Guid guid)
        {
            return new GameModeAdditionalApplicationCommand
            {                
                ApplicationName = applicationName,
                ApplicationPath = applicationPath,
                LaunchIfNotOpen = launchIfNotOpen,
                Guid = guid,
                ProfileService = ServiceFactory.ProfileService
            };
        }

        public static EquatableCommand NewGameModeAudioOutputCommand(string audioDeviceId, string audioDeviceName, Guid guid)
        {
            return new GameModeAudioOutputCommand
            {
                AudioDeviceId = audioDeviceId,
                AudioDeviceName = audioDeviceName,
                Guid = guid,
                ProfileService = ServiceFactory.ProfileService
            };
        }

        public static EquatableCommand NewGameModeWebLinksCommand(ObservableCollection<string> urls, bool enableTabbedBrowsing, Guid guid)
        {
            return new GameModeWebLinksCommand
            {
                Urls = urls,
                EnableTabbedBrowsing = enableTabbedBrowsing,
                Guid = guid,
                ProfileService = ServiceFactory.ProfileService
            };
        }

        public static EquatableCommand NewGameModePowerPlanCommand(Guid powerPlanId, string powerPlanName, Guid guid)
        {
            return new GameModePowerPlanCommand
            {
                PowerPlanId = powerPlanId,
                PowerPlanName = powerPlanName,
                Guid = guid,
                ProfileService = ServiceFactory.ProfileService
            };
        }

        public static EquatableCommand NewGameModeThermalCommand(Guid thermalProfileId, string thermalProfileName, Guid guid)
        {
            return new GameModeThermalCommand
            {
                ThermalProfileId = thermalProfileId,
                ThermalProfileName = thermalProfileName,
                Guid = guid,
                ProfileService = ServiceFactory.ProfileService
            };
        }

        public static EquatableCommand NewGameModeAlienFXCommand(AlienFXActionType type, string themeName, string themePath, Guid guid)
        {
            return new GameModeAlienFXCommand
            {
                Type = type,
                ThemeName = themeName,
                ThemePath = themePath,
                Guid = guid,
                ProfileService = ServiceFactory.ProfileService
            };
        }

        public static EquatableCommand NewGameModeEnergyBoosterCommand(Guid guid)
        {
            return new GameModeEnergyBoosterCommand
            {
                Guid = guid,
                ProfileService = ServiceFactory.ProfileService
            };
        }

        public static EquatableCommand NewGameModePerformanceMonitoringCommand(Guid guid)
        {
            return new GameModePerformanceMonitoringCommand
            {
                Guid = guid,
                ProfileService = ServiceFactory.ProfileService
            };
        }
    }
}