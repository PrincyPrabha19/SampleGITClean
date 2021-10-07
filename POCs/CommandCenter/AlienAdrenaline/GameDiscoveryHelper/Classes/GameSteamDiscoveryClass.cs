using System.Collections.Generic;

namespace AlienLabs.GameDiscoveryHelper.Classes
{
    public class GameSteamDiscoveryClass : GameSteamDiscovery
    {
        public GameSteamWindows GameSteamWindows { get; set; }

        public List<GameData> Discover()
        {
            var games = new List<GameData>();

            if (GameSteamWindows != null)
                games = GameSteamWindows.Discover();

            return games;
        }
    }
}
