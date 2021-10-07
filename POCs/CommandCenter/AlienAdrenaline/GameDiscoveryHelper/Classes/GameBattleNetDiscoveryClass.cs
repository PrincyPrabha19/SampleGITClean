using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienLabs.GameDiscoveryHelper.Classes
{
    class GameBattleNetDiscoveryClass : GameBattleNetDiscovery
    {
        public GameBattleNetWindows GameBattleNetWindows { get; set; }

        public List<GameData> Discover()
        {
            var games = new List<GameData>();

            if (GameBattleNetWindows != null)
                games = GameBattleNetWindows.Discover();

            return games;
        }
    }
}
