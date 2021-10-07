using System;
using System.Collections.ObjectModel;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.GameDiscoveryHelper;
using AlienLabs.GameDiscoveryHelper.Providers;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class GameModeCreateServiceClass : GameModeCreateService
    {
        #region GameModeCreationService Members
        public ProfileService ProfileService { get; set; }
        public GameServiceProvider GameServiceProvider { get; set; }
        public ObservableCollection<GameData> Games { get; set; }

        public void Refresh()
        {
            Games = GameServiceProvider.Games.ToObservableCollection();
        }

        public string GetValidGameMode(string gameModeName, bool excludeCurrentProfile = false)
        {
            return ProfileService.GetValidGameModeName(gameModeName, excludeCurrentProfile);
        }

        public Profile NewProfile(string gameModeName, string gameProfileActionName, string gameShortcutPath, string gamePath, string gameRealPath,
            Guid gameId, string gameName, string gameIconPath, string gameInstallPath, int steamId, string steamGamePath)
        {
            var profile = ProfileService.NewProfile(gameModeName);
            if (profile != null)
            {
                profile.GameId = gameId;
                profile.GameTitle = gameName;
                profile.GamePath = gamePath;
                profile.GameRealPath = gameRealPath;
                profile.GameInstallPath = gameInstallPath;
                profile.GameIconPath = gameIconPath;
                profile.GameShortcutPath = gameShortcutPath;
                profile.SteamId = steamId;
                profile.SteamGamePath = steamGamePath;

                var profileAction = ProfileService.NewProfileAction(gameProfileActionName, GameModeActionType.GameApplication);
                if (profileAction != null)
                {
                    var profileActionInfoGameApplication = profileAction.ProfileActionInfo as ProfileActionInfoGameApplication;
                    if (profileActionInfoGameApplication != null)
                    {
                        profileActionInfoGameApplication.ApplicationName = profile.GameTitle;
                        profileActionInfoGameApplication.ApplicationPath = profile.GamePath;
                        profileActionInfoGameApplication.ApplicationRealPath = profile.GameRealPath;
                    }                    
                }                

                ProfileService.AddProfileAction(profileAction);
            }

            return profile;
        }

        public void SaveProfileAs(string gameModeName, string gameShortcutPath)
        {
            ProfileService.Profile.Name = gameModeName;
            ProfileService.Profile.GameShortcutPath = gameShortcutPath;
        }

        //public Profile SaveAsProfile(string gameModeName)
        //{
        //    var currentProfile = ProfileService.Profile;

        //    var profile = ProfileService.NewProfile(gameModeName);
        //    if (profile != null)
        //    {
        //        profile.GameId = currentProfile.GameId;
        //        profile.GameTitle = currentProfile.GameTitle;
        //        profile.GamePath = currentProfile.GamePath;
        //        profile.GameShortcutPath = currentProfile.GameShortcutPath;
        //        profile.GameModeProfileActions = currentProfile.GameModeProfileActions.Clone();
        //    }

        //    return profile;
        //}

        public void AddProfileAction(string actionName, GameModeActionType actionType)
        {
            ProfileService.AddProfileAction(
                    ProfileService.NewProfileAction(actionName, actionType)
                );
        }
        #endregion
    }
}
