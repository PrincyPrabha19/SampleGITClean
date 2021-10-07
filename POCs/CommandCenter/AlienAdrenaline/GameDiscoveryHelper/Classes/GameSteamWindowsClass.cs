using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AlienLabs.GameDiscoveryHelper.Helpers;
using AlienLabs.WindowsIconHelper;
using Microsoft.Win32;

namespace AlienLabs.GameDiscoveryHelper.Classes
{
    public class GameSteamWindowsClass : GameSteamPathHelper, GameSteamWindows
    {
        #region GameSteamWindows Methods
        public List<GameData> Discover()
        {
            //   var games = getSteamGamesFromRegistry();
            var games = getSteamGames();
            return games;
        }
        #endregion

        #region Private Methods


        // This method gets the string between last two " "
        private string getvalue(string line)
        {
            string retVal;
            // find last occurance of "
            int last = line.LastIndexOf('\"');

            string line1 = line.Substring(0, last);
            last = line1.LastIndexOf('\"');
            retVal = line1.Substring(last + 1);
            return retVal;
        }

        private GameData parseSteamACF(string pathAcf)
        {
            GameData gameData = null;
            gameData = new GameDataClass();
            int counter = 0;
            string line;
            bool foundappid = false;
            bool foundname = false;
            // Open file and parse it.
            System.IO.StreamReader file =
               new System.IO.StreamReader(pathAcf);
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("appid") && !foundappid)
                {
                    foundappid = true;
                    gameData.SteamId = Int32.Parse(getvalue(line));
                }

                if (line.Contains("name")&& !foundname)
                {
                    foundname = true;
                    gameData.Name = getvalue(line);
                }

                if (foundname && foundappid)
                    break;
                counter++;
            }

            return gameData;
        }


        private List<GameData> getSteamGames()
        {
            var games = new List<GameData>();

            // Check registry to get Steam Install path.
            string appsDir = String.Empty;
            string installationPath = String.Empty;
            using (RegistryKey root = Registry.CurrentUser.OpenSubKey(GameSteamRegistryPath, false))
            {
                if (root != null)
                {
                    object steamApps = root.GetValue("SteamPath");
                    if (steamApps != null)
                        appsDir = steamApps.ToString().Replace('/', '\\')+"\\steamapps";
                }

                try
                {
                    // Look for all the ACF files
                    string[] gamesAcf = Directory.GetFiles(@appsDir, "*.acf");
                    int i = 0;
                    foreach (string nameAcf in gamesAcf)
                    {
                        // Parse ACF Files to get the name of the Games, ID etc...
                        GameData gameData = parseSteamACF(gamesAcf[i++]);
                        // Add data to game repo...
                        games.Add(gameData); 
                    }

                }
                catch (Exception e)
                {
                   
                }
            }
            return games;
        }

        private List<GameData> getSteamGamesFromRegistry()
        {
            var games = new List<GameData>();

            try
            {
                string applicationExePath = String.Empty;
                string installationPath = String.Empty;
                using (RegistryKey root = Registry.CurrentUser.OpenSubKey(GameSteamRegistryPath, false))
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

        private GameData getSteamGameDataFromRegistry(int steamId)
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
    
        private bool isSteamGameDownloading(string installationPath, int steamId)
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
