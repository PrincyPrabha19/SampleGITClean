using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienLabs.GameDiscoveryHelper.Classes
{
    class GameUplayDiscoveryClass : GameUplayDiscovery
    {
        public GameUplayWindows GameUplayWindows { get; set; }

        public List<GameData> Discover()
        {
            var games = new List<GameData>();

            if (GameUplayWindows != null)
                games = GameUplayWindows.Discover();

            return games;
        }
    }
}