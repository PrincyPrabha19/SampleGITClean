using System;
using System.Collections.Generic;
using System.Linq;

namespace AlienLabs.GameDiscoveryHelper.Providers.Classes
{
    public class GameServiceProviderClass : GameServiceProvider
    {
        #region GameServiceProvider Properties
        public List<GameProvider> GameProviders { get; set; }
        public List<GameData> Games { get; set; }
        public bool Initialized { get; private set; }
        #endregion

        #region Constructors
        public GameServiceProviderClass()
        {
            GameProviders = new List<GameProvider>();
            Games = new List<GameData>();
        }
        #endregion

        #region GameServiceProvider Methods
        public void Initialize()
        {
            if (!Initialized)
            {
                Initialized = true;

                var games = new List<GameData>();
                foreach (var gameProvider in GameProviders)
                    games.AddRange(gameProvider.GetGameList());

                games = games.GroupBy(g => g.Name).Select(g => g.First()).ToList();

                Games = (from g in games
                         orderby g.Name
                         select g).ToList();
            }
        }

        public GameData GetGameById(Guid id)
        {
            return (from g in Games
                    where g.Id == id
                    select g).FirstOrDefault();
        }

        public GameData GetGameByPath(string gamePath)
        {
            return (from g in Games
                    where String.Compare(g.ApplicationExePath, gamePath, true) == 0
                    select g).FirstOrDefault();
        }
        #endregion
    }
}