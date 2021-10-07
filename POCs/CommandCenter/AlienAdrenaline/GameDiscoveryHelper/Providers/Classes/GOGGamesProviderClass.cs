using System;
using System.Collections.Generic;
using AlienLabs.GameDiscoveryHelper.Providers.Classes.Factories;

namespace AlienLabs.GameDiscoveryHelper.Providers.Classes
{
    class GOGGamesProviderClass : GameProvider
    {
        private GameGOGDiscovery gameGOGDiscovery { get; set; }

        public GOGGamesProviderClass()
        {
            gameGOGDiscovery = GameObjectFactory.NewGameGOGDiscovery();
        }

        public List<GameData> GetGameList()
        {
            return gameGOGDiscovery.Discover();
        }
    }
}