

using System;
using System.Collections.ObjectModel;
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles
{
    public interface ProfileService
    {
        Profile Profile { get; }
        GameModeProfileActions GameModeProfileActions { get; }
        ProfileAction CurrentProfileAction { get; }

        void Initialize();
        void SetCurrentProfile(Profile profile);
        void SetCurrentProfileAction(ProfileAction profileAction);
        void SetProfileActionInfoApplication(string applicationName, string applicationPath, Guid guid);
        void SetProfileActionInfoGameApplication(string applicationName, string applicationPath, string applicationRealPath, Guid guid);
        void SetProfileActionInfoAdditionalApplication(string applicationName, string applicationPath, bool launchIfNotOpen, Guid guid);        
        void SetProfileActionInfoAudioOutput(string audioDeviceId, string audioDeviceName, Guid guid);
        void SetProfileActionInfoWebLinks(ObservableCollection<string> urls, bool enableTabbedBrowsing, Guid guid);
        void SetProfileActionInfoPowerPlan(Guid powerPlanId, string powerPlanName, Guid guid);
        void SetProfileActionInfoThermal(Guid thermalProfileId, string thermalProfileName, Guid guid);
        void SetProfileActionInfoAlienFX(AlienFXActionType type, string themeName, string themePath, Guid guid);
        void SetProfileActionInfoEnergyBooster(Guid guid);
        void SetProfileActionInfoPerformanceMonitoring(Guid guid);
        ProfileActionInfo GetProfileActionInfo(Guid guid);

        Profile NewProfile(string name);
        ProfileAction NewProfileAction(string name, GameModeActionType type);
        void AddProfileAction(ProfileAction profileAction);
        GameModeProfileActions CloneGameModeProfileActions();
        void ReplaceGameModeProfileActions(GameModeProfileActions gameModeProfileActions);

        string GetValidGameModeName(string gameModeName, bool excludeCurrentProfile = false);
    }
}
