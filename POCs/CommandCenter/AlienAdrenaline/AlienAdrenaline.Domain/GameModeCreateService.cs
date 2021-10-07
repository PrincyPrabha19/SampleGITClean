
using System;
using System.Collections.ObjectModel;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.GameDiscoveryHelper;
using AlienLabs.GameDiscoveryHelper.Providers;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface GameModeCreateService
    {
        ProfileService ProfileService { get; set; }
        GameServiceProvider GameServiceProvider { get; set; }        
        ObservableCollection<GameData> Games { get; set; }

        void Refresh();
        string GetValidGameMode(string gameModeName, bool excludeCurrentProfile = false);
        Profile NewProfile(string gameModeName, string gameProfileActionName, string gameShortcutPath, string gamePath, string gameRealPath,
            Guid gameId, string gameName, string gameIconPath, string gameInstallPath, int steamId, string steamGamePath);
        void SaveProfileAs(string gameModeName, string gameShortcutPath);
        void AddProfileAction(string actionName, GameModeActionType actionType);        
    }
}