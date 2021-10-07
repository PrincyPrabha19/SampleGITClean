using System;
using System.IO;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Tools;
using AlienLabs.GameDiscoveryHelper.Helpers;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class GameModeSaveAsPresenter : GameModeProcessorPathHelper
    {
        #region Public Properties
        public GameModeSaveAsView View { get; set; }
        public GameModeCreateService Model { get; set; }
        #endregion

        #region Public Methods
        public void Refresh()
        {
            View.GameModeName = Model.GetValidGameMode(Model.ProfileService.Profile.Name);
        }

        public string GetValidGameModeName(string gameModeName)
        {
            return Model.GetValidGameMode(gameModeName);
        }

        public void GameModeNameChanged(string gameModeName)
        {
            View.GameModeName = GetValidGameModeName(View.GameModeName);
        }

        public void SaveProfile()
        {
            string gameShortcutPath;
            if (createGameModeShotcut(Model.ProfileService.Profile, out gameShortcutPath))
                deleteGameModeShorcut(Model.ProfileService.Profile.GameShortcutPath);
            else gameShortcutPath = String.Empty;

            Model.SaveProfileAs(View.GameModeName, gameShortcutPath);
        }
        #endregion

        #region Private Methods
        [System.Runtime.InteropServices.DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

        private bool createGameModeShotcut(Profile profile, out string gameShortcutPath)
        {
            string gameShortcutName = String.Format(Properties.Resources.GameModeTitleText, View.GameModeName);
            string gameShortcutDescription = String.Format(Properties.Resources.LaunchGameModeText, View.GameModeName);
            string gameShortcutFolder = Path.GetDirectoryName(profile.GameShortcutPath);

            if (String.IsNullOrEmpty(gameShortcutFolder) || !Directory.Exists(gameShortcutFolder))
                gameShortcutFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            gameShortcutPath = Path.Combine(
                gameShortcutFolder, FilePathHelper.RemoveInvalidFileNameCharacters(gameShortcutName) + ".lnk");

            bool result = WScriptShortcutHelper.CreateShortcut(
                gameShortcutName, gameShortcutDescription, gameShortcutPath, GameModeProcessorFile, View.GameModeName,
                (profile.SteamId > 0) ? profile.GameIconPath : profile.GamePath);

            return result;
        }

        private void deleteGameModeShorcut(string gameShortcutPath)
        {
            try
            {
                if (File.Exists(gameShortcutPath)) 
                {
                    File.Delete(gameShortcutPath);
                    if (isDesktopShortcut(gameShortcutPath))
                        SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);
                }
            }
            catch (Exception)
            {
            }
        }

        private bool isDesktopShortcut(string gameShortcutPath)
        {
            string directory = Path.GetDirectoryName(gameShortcutPath);
            string desktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return String.Compare(directory, desktopDirectory, StringComparison.OrdinalIgnoreCase) == 0;
        }
        #endregion
    }
}