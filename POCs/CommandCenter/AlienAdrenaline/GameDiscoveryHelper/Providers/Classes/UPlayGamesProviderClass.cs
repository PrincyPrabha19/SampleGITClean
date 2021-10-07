using System.Collections.Generic;
using AlienLabs.GameDiscoveryHelper.Providers.Classes.Factories;

namespace AlienLabs.GameDiscoveryHelper.Providers.Classes
{
    public class UplayGamesProviderClass : GameProvider
    {
        private GameUplayDiscovery gameUplayDiscovery { get; set; }

        public UplayGamesProviderClass()
        {
            gameUplayDiscovery = GameObjectFactory.NewGameUplayDiscovery();
        }

        public List<GameData> GetGameList()
        {
            return gameUplayDiscovery.Discover();
        }
    }
}
