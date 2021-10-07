using System.Collections.Generic;
using AlienLabs.GameDiscoveryHelper.Classes;
using AlienLabs.GameDiscoveryHelper.Providers.Classes.DataSet;

namespace AlienLabs.GameDiscoveryHelper.Providers.Classes
{
    public class PredefinedGamesProviderClass : GameProvider
    {
        public PredefinedGamesSerializer PredefinedGamesSerializer { get; set; }
        public PredefinedGames PredefinedGames { get; private set; }

        public List<GameData> GetGameList()
        {
            if (PredefinedGames == null)
                PredefinedGames = PredefinedGamesSerializer.Deserialize();

            var games = new List<GameData>();
            if (PredefinedGames != null)
                foreach (var game in PredefinedGames.Items)
                    games.Add(new GameDataClass()
                                  {
                                      Name = game.Name,
                                      Image = game.Image
                                  });

            return games;
        }
    }
}
