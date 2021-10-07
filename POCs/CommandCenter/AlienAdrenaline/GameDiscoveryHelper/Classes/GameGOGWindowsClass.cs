using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AlienLabs.GameDiscoveryHelper.Helpers;
using AlienLabs.WindowsIconHelper;
using Microsoft.Win32;


namespace AlienLabs.GameDiscoveryHelper.Classes
{
    class GameGOGWindowsClass : GameGOGWindows 
    {
        #region GameGOGWindows Methods
        public List<GameData> Discover()
        {
            var games = getGOGGamesFromRegistry();

            return games;
        }
        #endregion

        #region Private Methods
        private List<GameData> getGOGGamesFromRegistry()
        {
            var games = new List<GameData>();

            try
            {
                string applicationExePath = String.Empty;
                string installationPath = String.Empty;
                using (RegistryKey root = Registry.CurrentUser.OpenSubKey(GameGOGRegistryPath, false))
                {
                    if (root != null)
                    {
                        object steamExe = root.GetValue("SteamExe");
                        if (steamExe != null)
                            applicationExePath = steamExe.ToString().Replace('/', '\\');

                        object steamPath = root.GetValue("SteamPath");
                        if (steamPath != null)
                            installationPath = steamPath.ToString().Replace('/', '\\');
                    }
                }

                using (RegistryKey root = Registry.LocalMachine.OpenSubKey(GameSteamRegistryUninstallPath, false))
                {
                    if (root != null)
                    {
                        foreach (string keyName in root.GetSubKeyNames())
                        {
                            if (!keyName.StartsWith("Steam App", StringComparison.InvariantCultureIgnoreCase))
                                continue;

                            int steamId;
                            if (!Int32.TryParse(keyName.Substring(9, keyName.Length - 9), out steamId))
                                continue;

                            if (isSteamGameDownloading(installationPath, steamId))
                                continue;

                            var gameData = getSteamGameDataFromRegistry(steamId);
                            if (gameData != null)
                            {
                                gameData.ApplicationExePath = String.Format(GameSteamApplicationPath, applicationExePath, steamId);
                                gameData.InstallationPath = installationPath;
                                if (!String.IsNullOrEmpty(gameData.ApplicationIconPath))
                                    gameData.Image = getImageFromIconPath(gameData.ApplicationIconPath);
                                games.Add(gameData);
                            }
                        }
                    }
                }
            }
            catch (System.Security.SecurityException)
            {
            }

            return games;
        }

        private GameData getGOGGameDataFromRegistry(int steamId)
        {
            GameData gameData = null;

            try
            {
                using (RegistryKey root = Registry.LocalMachine.OpenSubKey(
                    String.Format(GameSteamRegistryUninstallAppsPath, steamId), false))
                {
                    if (root != null)
                    {
                        gameData = new GameDataClass() { SteamId = steamId };

                        object displayName = root.GetValue("DisplayName");
                        if (displayName != null)
                            gameData.Name = displayName.ToString();

                        object displayIcon = root.GetValue("DisplayIcon");
                        if (displayIcon != null)
                            gameData.ApplicationIconPath = displayIcon.ToString();

                        //object installLocation = root.GetValue("InstallLocation");
                        //if (installLocation != null)
                        //{
                        //    gameData.SteamGameInstallationPath = installLocation.ToString();
                        //    gameData.SteamGameExePath = findSteamGameExecutable(gameData.SteamGameInstallationPath);
                        //}
                    }
                }
            }
            catch (System.Security.SecurityException)
            {
            }

            return gameData;
        }

        private byte[] getImageFromIconPath(string iconPath)
        {
            byte[] bytes = null;

            if (File.Exists(iconPath))
            {
                Icon icon = IconHelper.ExtractBestFitIcon(iconPath, 0, new Size(256, 256));
                if (icon != null)
                    bytes = IconUtils.GetBytesFromIcon(icon);
            }

            return bytes;
        }

        private bool isGOGGameDownloading(string installationPath, int steamId)
        {
            string downloadingPath = String.Format(GameSteamDownloadingPath, installationPath, steamId);
            return Directory.Exists(downloadingPath);
        }

        //private string findSteamGameExecutable(string gameInstallPath)
        //{
        //    string gamePath = String.Empty;

        //    try
        //    {
        //        using (RegistryKey root = Registry.LocalMachine.OpenSubKey(GameSteamRegistryFirewallRulesPath, false))
        //        {
        //            if (root != null)
        //            {
        //                foreach (string valueName in root.GetValueNames())
        //                {
        //                    string valueData = root.GetValue(valueName).ToString();
        //                    int index = valueData.IndexOf(gameInstallPath, 0, StringComparison.InvariantCultureIgnoreCase);
        //                    if (index != -1)
        //                    {
        //                        int index2 = valueData.IndexOf('|', index);
        //                        if (index2 != -1)
        //                        {
        //                            gamePath = valueData.Substring(index, index2 - index);
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Security.SecurityException)
        //    {
        //    }

        //    return gamePath;
        //}
        #endregion
    }
}
