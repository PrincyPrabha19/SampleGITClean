using System;
using System.Windows;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using AlienLabs.GameDiscoveryHelper.Helpers;
using AlienLabs.WindowsIconHelper;
using Microsoft.Win32;
using Size = System.Drawing.Size;

namespace AlienLabs.GameDiscoveryHelper.Classes
{
    public class GameUXWindowsClass : GameUXPathHelper, GameUXWindows
    {
        #region GameUXWindows Methods
        public void Refresh(List<GameData> games)
        {
            foreach (var game in games)
            {
                if (String.IsNullOrEmpty(game.ApplicationExePath))
                    updateApplicationExePath(game);

                if (game.Image == null)
                    updateApplicationImage(game);
            }

            games.RemoveAll(x => String.IsNullOrEmpty(x.ApplicationExePath) ||
                    (!Regex.IsMatch(x.ApplicationExePath, @"\.exe$", RegexOptions.IgnoreCase) || Regex.IsMatch(x.ApplicationExePath, @"GFWLive\.exe$", RegexOptions.IgnoreCase)));
        }
        #endregion

        #region Private Methods

        private void updateApplicationExePath(GameData game)
        {
            string applicationExePath = getApplicationExePathFromShortcut(
                String.Format(GameUXPlayShortcutFile, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), game.Id));

            if (String.IsNullOrEmpty(applicationExePath))
                applicationExePath = getApplicationExePathFromShortcut(
                    String.Format(GameUXPlayShortcutFile, Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), game.Id));

            //string firstShortcutFile = getFirstFileFromFolder(
            //    String.Format(GameUXPlayShortcutFolder, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), game.Id));
            //string applicationExePath = getApplicationExePathFromShortcut(firstShortcutFile);

            //if (String.IsNullOrEmpty(applicationExePath))
            //{
            //    firstShortcutFile = getFirstFileFromFolder(
            //        String.Format(GameUXPlayShortcutFolder, Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), game.Id));
            //    applicationExePath = getApplicationExePathFromShortcut(firstShortcutFile);
            //}

            if (String.IsNullOrEmpty(applicationExePath))
                applicationExePath = getApplicationExePathFromRegistry(String.Format(GameUXRegistryPath, game.Id));

            if (!String.IsNullOrEmpty(applicationExePath))
                game.ApplicationExePath = Path.Combine(game.InstallationPath, applicationExePath);
        }

        private void updateApplicationImage(GameData game)
        {
            byte[] image = getImageFromPath(
                String.Format(GameUXGamesBoxArtFolder, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), game.Id));

            if (image == null)
                image = getImageFromPath(
                    String.Format(GameUXGamesBoxArtFolder, Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), game.Id));

            if (image == null &&
                !String.IsNullOrEmpty(game.ConfigGDFBinaryPath))
                image = getImageFromConfigGDIBinaryPath(game.ConfigGDFBinaryPath);

            if (image == null)
                image = loadDefaultDefaultGameImage();

            game.Image = image;
        }

        private string getApplicationExePathFromShortcut(string shortcutPath)
        {
            string path = string.Empty;            

            try
            {
                if (File.Exists(shortcutPath))
                {
                    var wsShortcut = WScriptShortcutHelper.GetShortcutInfo(shortcutPath);
                    if (wsShortcut != null)
                        path = wsShortcut.TargetPath;
                }
            }
            catch (Exception)
            {
            }

            return path;
        }

        private string getApplicationExePathFromRegistry(string registryPath)
        {
            string path = string.Empty;

            try
            {
                using (RegistryKey root = Registry.LocalMachine.OpenSubKey(registryPath, false))
                {
                    object appExePath = root.GetValue("AppExePath");
                    if (appExePath != null)
                        path = appExePath.ToString();
                }
            }
            catch (Exception)
            {
            }

            return path;
        }

        private byte[] getImageFromPath(string imagePath)
        {
            return loadImageFromFile(imagePath);
        }

        private byte[] getImageFromConfigGDIBinaryPath(string configGDIBinaryPath)
        {
            byte[] bytes = null;

            try
            {
                Icon icon = IconHelper.ExtractBestFitIcon(configGDIBinaryPath, 0, new Size(256, 256));
                if (icon != null)
                    bytes = IconUtils.GetBytesFromIcon(icon);
            }
            catch (Exception)
            {
            }

            return bytes;
        }
        
        private void updateGameRegistryInformation(GameData gameData, RegistryKey key)
        {
            if (key != null)
            {
                object appExePath = key.GetValue("AppExePath");
                if (appExePath != null)
                {                        
                    string applicationExePath = appExePath.ToString();
                    if (!String.IsNullOrEmpty(applicationExePath))
                        gameData.ApplicationExePath = Path.Combine(gameData.InstallationPath, applicationExePath);
                }                
            }            
        }

        private void updateGamesFromRegistry(List<GameData> games, RegistryKey root)
        {
            foreach (string keyName in root.GetSubKeyNames())
            {
                try
                {
                    using (RegistryKey key = root.OpenSubKey(keyName))
                    {
                        Guid gameId = Guid.Empty;
                        if (!Guid.TryParse(keyName, out gameId))
                            updateGamesFromRegistry(games, key);
                        else
                        {
                            var gameData = games.Find(x => x.Id == gameId);
                            if (gameData != null)
                                updateGameRegistryInformation(gameData, key);
                        }
                    }
                }
                catch (System.Security.SecurityException)
                {
                }
            }
        }

        private byte[] loadDefaultDefaultGameImage()
        {
            byte[] bytes = null;

            try
            {
                var resourceUri = new Uri("pack://application:,,,/AlienAdrenaline.Domain;component/media/nogameboxart.png");

                var streamResourceInfo = Application.GetResourceStream(resourceUri);
                if (streamResourceInfo != null)
                {
                    using (var binaryReader = new BinaryReader(streamResourceInfo.Stream))
                    {
                        bytes = binaryReader.ReadBytes((int)streamResourceInfo.Stream.Length);
                    }
                }
            }
            catch (Exception)
            {
            }

            return bytes;
        }

        private byte[] loadImageFromFile(string file)
        {
            byte[] bytes = null;

            if (File.Exists(file))
            {
                try
                {
                    using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        using (var binaryReader = new BinaryReader(fileStream))
                        {
                            long totalBytes = new FileInfo(file).Length;
                            bytes = binaryReader.ReadBytes((Int32)totalBytes);
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }

            return bytes;
        }

        //private string getFirstFileFromFolder(string folder)
        //{
        //    string fileName = String.Empty;

        //    if (Directory.Exists(folder))
        //    {
        //        var files = Directory.GetFiles(folder);
        //        if (files.Length > 0)
        //            fileName = files[0];
        //    }

        //    return fileName;
        //}
        #endregion
    }
}
