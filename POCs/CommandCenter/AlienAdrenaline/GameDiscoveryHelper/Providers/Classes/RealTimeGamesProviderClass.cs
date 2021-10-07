using System.Collections.Generic;
using AlienLabs.GameDiscoveryHelper.Providers.Classes.Factories;

namespace AlienLabs.GameDiscoveryHelper.Providers.Classes
{
    public class RealTimeGamesProviderClass : GameProvider
    {
        private GameUXDiscovery gameUXDiscovery { get; set; }

        public RealTimeGamesProviderClass()
        {
            gameUXDiscovery = GameObjectFactory.NewGameUXDiscovery();
        }

        public List<GameData> GetGameList()
        {
            return gameUXDiscovery.Discover();
        }
    }
}
