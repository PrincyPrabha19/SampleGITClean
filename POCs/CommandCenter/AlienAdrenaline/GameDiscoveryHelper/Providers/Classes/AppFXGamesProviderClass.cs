using System;
using System.Collections.Generic;
using System.IO;
using AlienLabs.GameDiscoveryHelper.Classes;
using LightFXConfigurator;
using LightFXConfigurator.Classes.Registration;
using LightFXConfigurator.Classes.Serialization;
using Microsoft.Win32;

namespace AlienLabs.GameDiscoveryHelper.Providers.Classes
{
    public class AppFXGamesProviderClass : GameProvider
    {
        #region Private Properties
        private IAppFXDiscovery appFXDiscovery { get; set; }
        #endregion

        #region Constructors
        public AppFXGamesProviderClass()
        {
            appFXDiscovery = new AppFXDiscovery();
        }
        #endregion

        #region Public Methods
        public List<GameData> GetGameList()
        {
            var games = new List<GameData>();

            var appFXGames = appFXDiscovery.Discover();
            foreach (var appFXGame in appFXGames)
            {
                if (appFXGame.AppType == LFX_ApplicationType.Game)
                {
                    GameData gameData = createGameDataFromAppFxGame(appFXGame);
                    if (gameData != null)
                        games.Add(gameData);
                }
            }

            return games;
        }
        #endregion

        #region Private Methods
        private GameData createGameDataFromAppFxGame(IAppFXData appFXData)
        {
            GameData gameData = null;
            try
            {
                if (File.Exists(appFXData.OriginalConfigFile))
                {
                    var configFileDeserializer = new ConfigFileDeserializer();
                    var appFXConfiguration = configFileDeserializer.Deserialize(appFXData.OriginalConfigFile);
                    if (appFXConfiguration != null)
                    {
                        var installDir = readValueFromRegistry(
                            getBaseRegistryKey(appFXConfiguration.Key), appFXConfiguration.SubKey, String.Empty, appFXConfiguration.Value, String.Empty) as string;
                        if (!String.IsNullOrEmpty(installDir))
                        {
                            var applicationExePath = Path.Combine(installDir, appFXConfiguration.Executable);
                            //if (File.Exists(applicationExePath))
                            //{
                                gameData = new GameDataClass
                                {
                                    Name = appFXData.Name,
                                    Image = appFXConfiguration.Image,
                                    InstallationPath = installDir,
                                    ApplicationExePath = applicationExePath
                                };
                            //}
                        }
                    }                
                }
            }
            catch (Exception e)
            {
                gameData = null;
            }

            return gameData;
        }

        private object readValueFromRegistry(RegistryKey root, string keyName, string subkeyName, string valueName, object defaultValue)
        {
            try
            {
                var fullkeyName = string.Format("{0}\\{1}", keyName, subkeyName);
                var key = root.OpenSubKey(fullkeyName, false);
                if (key != null)
                {
                    var valueTemp = key.GetValue(valueName);
                    return valueTemp ?? defaultValue;
                }
            }
            catch { }

            return defaultValue;
        }

        private RegistryKey getBaseRegistryKey(string baseKey)
        {
            RegistryHive hive;
            var ok = Enum.TryParse(baseKey, true, out hive);
            return ok ? RegistryKey.OpenBaseKey(hive, RegistryView.Default) : Registry.LocalMachine;
        }
        #endregion
    }
}
