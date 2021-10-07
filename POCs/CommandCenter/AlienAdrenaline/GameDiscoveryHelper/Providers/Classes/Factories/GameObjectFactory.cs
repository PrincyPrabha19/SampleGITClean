using System.Collections.Generic;
using AlienLabs.GameDiscoveryHelper.Classes;
using AlienLabs.GameDiscoveryHelper.Providers.Classes.Serialization;

namespace AlienLabs.GameDiscoveryHelper.Providers.Classes.Factories
{
    public class GameObjectFactory
    {
        private static readonly List<GameProvider> globalGameProviders = new List<GameProvider>()
        {
            NewRealTimeGamesProvider(), 
            NewAppFXGamesProvider(), 
            NewSteamGamesProvider(),
            NewUplayGamesProvider(),
            NewGOGGamesProvider()
            //NewBattleNetGamesProvider()
        };

        private static GameServiceProvider gameServiceProvider;
        public static GameServiceProvider NewGameServiceProvider()
        {
            return NewGameServiceProvider(globalGameProviders);
        }

        public static GameServiceProvider NewGameServiceProvider(List<GameProvider> gameProviders)
        {
            if (gameServiceProvider == null)
            {
                gameServiceProvider = new GameServiceProviderClass()
                {
                    GameProviders = gameProviders
                };
                
                //gameServiceProvider.Initialize();
            }

            return gameServiceProvider;
        }

        private static GameProvider predefinedGamesProvider;
        public static GameProvider NewPredefinedGamesProvider()
        {
            if (predefinedGamesProvider == null)
                predefinedGamesProvider = new PredefinedGamesProviderClass()
                {
                    PredefinedGamesSerializer = new PredefinedGamesSerializerClass()
                };

            return predefinedGamesProvider;
        }

        private static GameProvider appFXGamesProvider;
        public static GameProvider NewAppFXGamesProvider()
        {
            if (appFXGamesProvider == null)
                appFXGamesProvider = new AppFXGamesProviderClass()
                {
                };

            return appFXGamesProvider;
        }

        private static GameProvider realTimeGamesProvider;
        public static GameProvider NewRealTimeGamesProvider()
        {
            if (realTimeGamesProvider == null)
                realTimeGamesProvider = new RealTimeGamesProviderClass()
                {
                };

            return realTimeGamesProvider;
        }

        private static GameProvider steamGamesProvider;
        public static GameProvider NewSteamGamesProvider()
        {
            if (steamGamesProvider == null)
                steamGamesProvider = new SteamGamesProviderClass()
                {
                };

            return steamGamesProvider;
        }
        private static GameProvider GOGGamesProvider;

        public static GameProvider NewGOGGamesProvider()
        {
            if (GOGGamesProvider == null)
                GOGGamesProvider = new GOGGamesProviderClass()
                {

                };
            return GOGGamesProvider;
        }

        private static GameProvider UplayGamesProvider;

        public static GameProvider NewUplayGamesProvider()
        {
            if (UplayGamesProvider == null)
                UplayGamesProvider = new UplayGamesProviderClass()
                {

                };
            return UplayGamesProvider;
        }

        private static GameProvider BattleNetGamesProvider;

        public static GameProvider NewBattleNetGamesProvider()
        {
            if (BattleNetGamesProvider == null)
                BattleNetGamesProvider = new BattleNetGamesProviderClass()
                {

                };
            return BattleNetGamesProvider;

        }

        private static GameUXDiscovery gameUXDiscovery;
        public static GameUXDiscovery NewGameUXDiscovery()
        {
            if (gameUXDiscovery == null)
                gameUXDiscovery = new GameUXDiscoveryClass()
                {
                    GameUXWMI = new GameUXWMIClass(),
                    GameUXWindows = new GameUXWindowsClass()
                };

            return gameUXDiscovery;
        }

        private static GameSteamDiscovery gameSteamDiscovery;
        public static GameSteamDiscovery NewGameSteamDiscovery()
        {
            if (gameSteamDiscovery == null)
                gameSteamDiscovery = new GameSteamDiscoveryClass()
                {
                    GameSteamWindows = new GameSteamWindowsClass()
                };

            return gameSteamDiscovery;
        }

        private static GameGOGDiscovery gameGOGDiscovery;
        public static GameGOGDiscovery NewGameGOGDiscovery()
        {
            if (gameGOGDiscovery == null)
                gameGOGDiscovery = new GameGOGDiscoveryClass()
                {
                    GameGOGWindows = new GameGOGWindowsClass()
                };

            return gameGOGDiscovery;
        }

        private static GameUplayDiscovery gameUplayDiscovery;
        public static GameUplayDiscovery NewGameUplayDiscovery()
        {
            if (gameUplayDiscovery == null)
                gameUplayDiscovery = new GameUplayDiscoveryClass()
                {
                    GameUplayWindows = new GameUplayWindowsClass()
                };

            return gameUplayDiscovery;
        }

        private static GameBattleNetDiscovery gameBattleNetDiscovery;
        public static GameBattleNetDiscovery NewGameBattleNetDiscovery()
        {
            if (gameBattleNetDiscovery == null)
                gameBattleNetDiscovery = new GameBattleNetDiscoveryClass()
                {
                    GameBattleNetWindows = new GameBattleNetWindowsClass()
                };

            return gameBattleNetDiscovery;
        }
    }


}
