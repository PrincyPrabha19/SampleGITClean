using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AlienLabs.GameDiscoveryHelper.Helpers;
using AlienLabs.WindowsIconHelper;
using Microsoft.Win32;
using System.Text;
namespace AlienLabs.GameDiscoveryHelper.Classes
{
    class GameUplayWindowsClass : GameUplayWindows
    {
        #region GameUplayWindows Methods
        public List<GameData> Discover()
        {
            //   var games = getSteamGamesFromRegistry();
            var games = getUplayGames();
            return games;
        }
        #endregion

        #region Private Methods

        private bool readLinex (System.IO.StreamReader file, string line, int count)
        {
            bool retVal = false;

            if ((line = file.ReadLine()) != null)
            {
                if (count !=null)
                count++;
                retVal = true;
            }

            return retVal;
        }

        private List<GameData>  parseUplayConfFile (string imgPath, string confPath)
        {
            var games = new List<GameData>();
            GameData gameData = null;
            
            int counter = 0;
            string line;

            // Open file and parse it.
            Stream stream = File.Open(confPath, FileMode.Open);

            System.IO.StreamReader file =
               new System.IO.StreamReader(stream);

            string start_token = "root:";
            string end_token = "start_game:";
            string logo = "logo_image: ";
            string thumb = "thumb_image: ";
            string background = "background_image: ";
            string splash = "splash_image: ";
            string name = "name: ";
            String result = "";
            int games_found = 0;
            bool block = false;
            bool start_token_found = false;
            bool img_found = false;
            string line1;

            string[] imgFiles = Directory.GetFiles(imgPath, "*.jpg");

            while ((line = file.ReadLine()) != null)
            {
                counter++;
                if (line.Contains(start_token) || start_token_found)
                {
                    start_token_found = false;
                    img_found = false;
                    block = true;
                    result = "";
                    gameData = new GameDataClass();
                    while (true)
                    {
                        line = file.ReadLine();
                        counter++;
                        if (line == null)
                            break;

                        if (line.Contains(logo))
                        {
                            line1 = line.Substring(line.IndexOf(logo) + logo.Length);
                            string logo_image = imgPath + line1;
                            foreach (string imgs in imgFiles)
                            {
                                if (imgs.Contains(line1) && !img_found)
                                {
                                    img_found = true;
                                    gameData.Image = System.IO.File.ReadAllBytes(imgs);
                                     //gameData.Image = Encoding.ASCII.GetBytes(logo_image);
                                    break;
                                }
                            }
                        }

                        if (line.Contains(thumb))
                        { 
                            line1 = line.Substring(line.IndexOf(thumb) + thumb.Length);
                            string thumb_image = imgPath + line1;
                            foreach (string imgs in imgFiles)
                            {
                                if (imgs.Contains(line1) && !img_found)
                                {
                                    img_found = true;
                                    gameData.Image = System.IO.File.ReadAllBytes(imgs);
                                    //gameData.Image = Encoding.ASCII.GetBytes(thumb_image);
                                    break;
                                }
                            }
                        }
                        if (line.Contains(background))
                        {
                            line1 = line.Substring(line.IndexOf(background) + background.Length);
                            string background_image = imgPath + line1;
                            foreach (string imgs in imgFiles)
                            {
                                
                                if (imgs.Contains(line1) && !img_found)
                                {
                                    img_found = true;
                                    gameData.Image = System.IO.File.ReadAllBytes(imgs);
                                    //gameData.Image = Encoding.ASCII.GetBytes(background_image);
                                    break;
                                }
                            }
                        }

                        if (line.Contains(splash))
                        {
                            img_found = true;
                            line1 = line.Substring(line.IndexOf(splash) + splash.Length);
                            string splash_image = imgPath + line1;
                            foreach (string imgs in imgFiles)
                            {
                                if (imgs.Contains(line1) && !img_found)
                                {
                                    img_found = true;
                                    gameData.Image = System.IO.File.ReadAllBytes(imgs);
                                    //gameData.Image = Encoding.ASCII.GetBytes(splash_image);
                                    break;
                                }
                            }
                        }

                        if (line.Contains(name))
                        {
                            line1 = line.Substring(line.IndexOf(name) + name.Length);
                            gameData.Name = line1;
                        }

                        //gameData.Image = Encoding.ASCII.GetBytes(imgPath + line1);


                        if (line.Contains(end_token))
                            break;
                    
                        if (line.Contains(start_token))
                        {
                            start_token_found = true;
                            result = "";
                            block = false;
                            break;
                        }

                        result += line;
                    }
                    
                    if (block)
                    {
                        games_found++;
                        // Add data to game repo...
                        games.Add(gameData);
                        block = false;
                    }
                }
            }
            //C:\Program Files(x86)\Ubisoft\Ubisoft Game Launcher\cache\assets

            return games;
        }

        private List<GameData> getUplayGames()
        {
          //  var games = new List<GameData>();

            // Initialize the install path to a known value
            string installPath = "c:\\Program Files (x86)\\Ubisoft\\Ubisoft Game Launcher";
            string confPath;
            string imgPath;

            // check registery for actual value of install path.
            bool is64bit = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"));
                       
            RegistryKey root = null;
            string value32 = string.Empty;

            if (is64bit)
            {
                var regView64 = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine,
                      RegistryView.Registry64);
                using (root = regView64.OpenSubKey(@"SOFTWARE\WOW6432Node\Ubisoft\Launcher", false))
                {
                    if (root != null)
                    {
                        object steamApps = root.GetValue("InstallDir");
                        if (steamApps != null)
                            installPath = steamApps.ToString().Replace('/', '\\') ;
                    }
                }
            }
            else {
                var regView32 = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine,
                     RegistryView.Registry32);
                using (root = regView32.OpenSubKey(@"SOFTWARE\Ubisoft\Launcher", false))
                {
                    if (root != null) {
                        object uplayApps = root.GetValue("InstallDir");
                        if (uplayApps != null)
                            installPath = uplayApps.ToString().Replace('/', '\\') ;
                    }
                }
            }

            // Open the configuration file
            confPath = installPath + "cache\\configuration\\configurations";
            imgPath = installPath + "cache\\assets\\";
            // Parse the configuration file
            return parseUplayConfFile(imgPath, confPath); ;
        }
        #endregion
    }
}
