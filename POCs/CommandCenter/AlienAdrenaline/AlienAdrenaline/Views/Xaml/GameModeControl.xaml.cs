using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using AlienLabs.AlienAdrenaline.App.Factories;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.Tools;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml
{
    public partial class GameModeControl : GameModeView
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

        #region GameModeView Properties
        public GameModePresenter Presenter { get; set; }
        public ViewType Type { get { return ViewType.GameMode; } }

        private Profile profile;
        public Profile Profile
        {
            get { return profile; }
            set 
            {
                profile = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Profile"));
            }
        }

        public GameModeActionSequenceView ActionSequenceView
        {
            get { return borderActionSequence.Child as GameModeActionSequenceView; }
            set { borderActionSequence.Child = value as UIElement; }
        }

        private byte[] gameImage;
        public byte[] GameImage
        {
            get { return gameImage; }
            set
            {
                gameImage = value;
                PropertyChanged(this, new PropertyChangedEventArgs("GameImage"));
            }
        }

        private string gamePath;
        public string GamePath
        {
            get { return gamePath; }
            set
            {
                gamePath = value;
                PropertyChanged(this, new PropertyChangedEventArgs("GamePath"));
            }
        }        
        #endregion

        #region Constructors
        public GameModeControl()
		{
			InitializeComponent();
		}
        #endregion

        #region Public Methods
        public void Refresh()
        {
            Presenter.Refresh();

            if (!Presenter.IsFoundGameExecutablePath())
                MsgBox.Show(Properties.Resources.ChangeGameModeTitleText,
                    Properties.Resources.GameExecPathDoesNotExistText, MsgBoxIcon.Exclamation, MsgBoxButtons.Ok);
        }
        #endregion

        #region Event Handlers
        private void btnCreateShorcut_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string defaultShortcutFolder = String.Empty;
            string defaultShortcutFile = String.Empty;
            try
            {
                defaultShortcutFolder = Path.GetDirectoryName(Profile.GameShortcutPath);
                defaultShortcutFile = Path.GetFileName(Profile.GameShortcutPath);
            }
            catch (Exception)
            {
            }            

            FolderOperations folderOperations = ObjectFactory.NewFolderOperations();
            var shortcutFolder = folderOperations.GetSelectedPath(defaultShortcutFolder);
            if (!String.IsNullOrEmpty(shortcutFolder))
            {
                if (!folderOperations.IsValidFolderPath(shortcutFolder))
                    shortcutFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                createGameModeShortcut(shortcutFolder, defaultShortcutFolder, Profile.GameShortcutPath);
            }
        }

        private void btnCloseGameModeProfile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Presenter.CloseGameModeProfile();
        }
        #endregion

        #region Private Methods
        private void createGameModeShortcut(string shortcutFolder, string defaultShortcutFolder, string defaultShortcutPath)
        {
            string defaultShortcutFile = Path.GetFileName(defaultShortcutPath);
            bool isSameShortcutFolder = String.Compare(shortcutFolder, defaultShortcutFolder, true) == 0;

            string shortcutPath = Path.Combine(shortcutFolder, defaultShortcutFile);
            if (isSameShortcutFolder && File.Exists(shortcutPath))
            {
                MsgBox.Show(Properties.Resources.GameModeShortcutCreateTitleText,
                    Properties.Resources.GameModeShortcutAlreadyExistsText, MsgBoxIcon.Information, MsgBoxButtons.Ok);
                return;
            }            

            if (!isSameShortcutFolder && Presenter.IsDirty)
            {
                if (MsgBox.Show(
                    Properties.Resources.GameModeShortcutCreateTitleText, Properties.Resources.WantToSavePendingChangesText, MsgBoxIcon.Question, MsgBoxButtons.YesNo) == MsgBoxResult.No)
                    return;
            }            
            
            if (!Presenter.CreateGameModeShotcut(shortcutFolder, out shortcutPath))
            {
                MsgBox.Show(Properties.Resources.GameModeShortcutCreateTitleText,
                    String.Format(Properties.Resources.GameModeShortcutCreateErrorText, shortcutPath), MsgBoxIcon.Exclamation, MsgBoxButtons.Ok);
                return;
            }

            if (!isSameShortcutFolder)
            {
                Presenter.DeleteGameModeShorcut(defaultShortcutPath);
                Presenter.SaveProfile();
            }

            MsgBox.Show(Properties.Resources.GameModeShortcutCreateTitleText,
                String.Format(Properties.Resources.GameModeShortcutCreateDescriptionText, shortcutPath), MsgBoxIcon.Information, MsgBoxButtons.Ok);
        }
        #endregion
    }
}