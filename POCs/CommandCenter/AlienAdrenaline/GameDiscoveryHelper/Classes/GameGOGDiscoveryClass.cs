using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienLabs.GameDiscoveryHelper.Classes
{
    class GameGOGDiscoveryClass : GameGOGDiscovery
    {
        public GameGOGWindows GameGOGWindows { get; set; }

        public List<GameData> Discover()
        {
            var games = new List<GameData>();

            if (GameGOGWindows != null)
                games = GameGOGWindows.Discover();

            return games;
        }
    }
}
