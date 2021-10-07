using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlienLabs.GameDiscoveryHelper.Providers.Classes.Factories;

namespace AlienLabs.GameDiscoveryHelper.Providers.Classes
{
    class BattleNetGamesProviderClass : GameProvider
    {
        private GameBattleNetDiscovery gameBattleNetDiscovery { get; set; }

        public BattleNetGamesProviderClass()
        {
            gameBattleNetDiscovery = GameObjectFactory.NewGameBattleNetDiscovery();
        }

        public List<GameData> GetGameList()
        {
            return gameBattleNetDiscovery.Discover();
        }
    }
}
