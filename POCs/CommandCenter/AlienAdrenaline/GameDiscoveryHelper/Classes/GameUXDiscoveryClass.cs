using System.Collections.Generic;

namespace AlienLabs.GameDiscoveryHelper.Classes
{
    public class GameUXDiscoveryClass : GameUXDiscovery
    {
        public GameUXWMI GameUXWMI { get; set; }
        public GameUXWindows GameUXWindows { get; set; }

        public List<GameData> Discover()
        {
            var games = new List<GameData>();

            if (GameUXWMI != null)
            {
                games = GameUXWMI.Discover();
                
                if (GameUXWindows != null)
                    GameUXWindows.Refresh(games);
            }            

            return games;
        }
    }
}
