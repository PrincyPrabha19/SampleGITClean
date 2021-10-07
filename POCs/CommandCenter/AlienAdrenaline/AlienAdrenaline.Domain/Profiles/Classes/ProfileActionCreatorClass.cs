

using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileActionCreatorClass : ProfileActionCreator
    {
        public ProfileAction New(string name, GameModeActionType type)
        {
            return New(name, type, false);
        }

        public ProfileAction New(string name, GameModeActionType type, bool initialize)
        {
            var profileAction = new ProfileActionClass()
            {
                Name = name,
                Type = type
            };

            setProfileActionInfo(profileAction, null, initialize);

            return profileAction;
        }

        public ProfileAction New(ProfileAction profileActionTemplate, bool initialize)
        {
            var newProfileAction = new ProfileActionClass()
            {
                Name = profileActionTemplate.Name,
                Type = profileActionTemplate.Type
            };

            setProfileActionInfo(newProfileAction, profileActionTemplate, initialize);

            return newProfileAction;
        }

        private void setProfileActionInfo(ProfileAction profileAction, ProfileAction profileActionTemplate, bool initialize)
        {
            switch (profileAction.Type)
            {
                case GameModeActionType.GameApplication:
                    profileAction.ProfileActionInfo = new ProfileActionInfoGameApplicationClass();
                    break;

                case GameModeActionType.Application:
                case GameModeActionType.MediaPlayerApplication:
                case GameModeActionType.FrapsApplication:
                case GameModeActionType.VoIPApplication:
                    profileAction.ProfileActionInfo = new ProfileActionInfoApplicationClass();
                    break;
                case GameModeActionType.AdditionalApplication:
                    profileAction.ProfileActionInfo = new ProfileActionInfoAdditionalApplicationClass();
                    break;
                case GameModeActionType.AudioOutput:
                    profileAction.ProfileActionInfo = new ProfileActionInfoAudioOutputClass(initialize);
                    break;
                case GameModeActionType.WebLinks:
                    profileAction.ProfileActionInfo = new ProfileActionInfoWebLinksClass();
                    break;
                case GameModeActionType.PowerPlan:
                    profileAction.ProfileActionInfo = new ProfileActionInfoPowerPlanClass(initialize);
                    break;
                case GameModeActionType.Thermal:
                    profileAction.ProfileActionInfo = new ProfileActionInfoThermalClass(initialize);
                    break;
                case GameModeActionType.AlienFX:
                    profileAction.ProfileActionInfo = new ProfileActionInfoAlienFXClass(initialize);
                    break;
                case GameModeActionType.EnergyBooster:
                    profileAction.ProfileActionInfo = new ProfileActionInfoEnergyBoosterClass(initialize);
                    break;
                case GameModeActionType.PerformanceMonitoring:
                    profileAction.ProfileActionInfo = new ProfileActionInfoPerformanceMonitoringClass(initialize);
                    break;
            }
        }
    }
}