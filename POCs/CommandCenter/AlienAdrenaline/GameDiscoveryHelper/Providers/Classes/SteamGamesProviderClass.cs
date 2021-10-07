using System.Collections.Generic;
using AlienLabs.GameDiscoveryHelper.Providers.Classes.Factories;

namespace AlienLabs.GameDiscoveryHelper.Providers.Classes
{
    public class SteamGamesProviderClass : GameProvider
    {
        private GameSteamDiscovery gameSteamDiscovery { get; set; }

        public SteamGamesProviderClass()
        {
            gameSteamDiscovery = GameObjectFactory.NewGameSteamDiscovery();
        }

        public List<GameData> GetGameList()
        {
            return gameSteamDiscovery.Discover();
        }
    }
}
