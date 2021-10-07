using System;
using System.IO;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;
using AlienLabs.AlienAdrenaline.Tools;
using AlienLabs.GameDiscoveryHelper.Helpers;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class GameModePresenter : GameModeProcessorPathHelper
    {
        #region Public Properties
        public GameModeView View { get; set; }
        public GameModeService Model { get; set; }
        public ViewRepository ViewRepository { get; set; }      
        public CommandContainer CommandContainer { get; set; }
        public EventTrigger EventTrigger { get; set; }

        public virtual bool IsDirty
        {
            get { return !CommandContainer.IsEmpty; }
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
        }

        public void Refresh()
        {
            View.Profile = Model.Profile;
            View.GameImage = Model.GameImage;
            View.GamePath = Model.GamePath;

            View.ActionSequenceView = ViewRepository.GetByType(ViewType.GameModeActionSequence) as GameModeActionSequenceView;
            if (View.ActionSequenceView != null)
                View.ActionSequenceView.Refresh();
        }

        public bool CreateGameModeShotcut(string gameShortcutFolder, out string gameShortcutPath)
        {
            string gameShortcutName = String.Format(Properties.Resources.GameModeTitleText, Model.Profile.Name);
            string gameShortcutDescription = String.Format(Properties.Resources.LaunchGameModeText, Model.Profile.Name);

            if (String.IsNullOrEmpty(gameShortcutFolder) || !Directory.Exists(gameShortcutFolder))
                gameShortcutFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);            
            gameShortcutPath = Path.Combine(
                gameShortcutFolder, FilePathHelper.RemoveInvalidFileNameCharacters(gameShortcutName) + ".lnk");

            bool result = WScriptShortcutHelper.CreateShortcut(
                gameShortcutName, gameShortcutDescription, gameShortcutPath, GameModeProcessorFile, Model.Profile.Name, 
                (Model.Profile.SteamId > 0) ? Model.Profile.GameIconPath : Model.Profile.GamePath);

            if (result)
                Model.Profile.GameShortcutPath = gameShortcutPath;
            return result;
        }

        public void DeleteGameModeShorcut(string gameShortcutPath)
        {
            try
            {
                if (File.Exists(gameShortcutPath))
                    File.Delete(gameShortcutPath);
            }
            catch (Exception)
            {
            }
        }

        public void SaveProfile()
        {
            EventTrigger.Fire(ProfileActionType.Save);
        }

        public bool IsFoundGameExecutablePath()
        {
            return FilePathHelper.IsValidPath(Model.GamePath);
        }

        public void CloseGameModeProfile()
        {
            EventTrigger.Fire(ProfileActionType.Close);
        }
        #endregion
    }
}