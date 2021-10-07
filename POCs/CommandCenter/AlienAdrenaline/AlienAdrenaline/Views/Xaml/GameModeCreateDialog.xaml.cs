
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using AlienLabs.AlienAdrenaline.App.Factories;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.Tools;
using AlienLabs.GameDiscoveryHelper;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml
{
	public partial class GameModeCreateDialog : GameModeCreateView
    {
        #region Private Properties
	    private readonly FolderOperations folderOperations = ObjectFactory.NewFolderOperations();
	    private readonly FileOperations fileOperations = ObjectFactory.NewFileOperations();
        #endregion

        #region Public Properties
        public GameModeCreatePresenter Presenter { get; set; }        
        #endregion

        #region GameModeCreateView Members
        public ViewType Type { get { return ViewType.GameModeCreate; } }
        public ObservableCollection<GameData> Games { get; set; }

        public string GameModeName
        {
            get
            {
                return textBoxGameModeName.Text.Trim();
            }
            set
            {
                textBoxGameModeName.Text = value;
            }
        }

        public GameData GameSelected
        {
            get
            {
                if (listBoxGames.SelectedIndex != -1)
                    return listBoxGames.Items[listBoxGames.SelectedIndex] as GameData;
                return null;
            }
            set
            {
                GameModeName = (value != null) ? Presenter.GetValidGameModeName(value.Name) : string.Empty;
				stackPanelGamePath.Visibility = Presenter.ShowGameModePath() ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public string GameModeShortcutFolder
        {
            get
            {
                return textBoxShortcutFolder.Text.Trim();
            }
            set
            {
                textBoxShortcutFolder.Text = value;
            }
        }

        public string GamePath
        {
            get
            {
                return textBoxGamePath.Text.Trim();
            }
            set
            {
                textBoxGamePath.Text = value;
            }
        }

        public bool IsNextEnabled
        {
            get
            {
                return buttonNext.IsEnabled;
            }
            set
            {
                buttonNext.IsEnabled = value;
            }
        }

        public void SetGamesItemsSource()
        {
            listBoxGames.ItemsSource = Games;
        }
        #endregion

        #region Constructors
        public GameModeCreateDialog()
		{
			InitializeComponent();
		}
		#endregion

        public void Refresh()
        {
            Presenter.Refresh();
        }

		#region Event Handlers
		private void buttonCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}

		private void buttonNext_Click(object sender, RoutedEventArgs e)
		{
            Presenter.GameModeNameChanged(GameModeName);
		    Presenter.AddProfile();
            DialogResult = true;
            Close();
		}

        private void listBoxGames_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                Presenter.ChangeGameSelected(e.AddedItems[0] as GameData);
            else
                Presenter.ChangeGameSelected(null);

            Presenter.UpdateNextButton();
        }

        //private void textBoxGameModeName_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    if (!String.IsNullOrEmpty(GameModeName) && !String.IsNullOrWhiteSpace(GameModeName))
        //        Presenter.GameModeNameChanged(GameModeName);
        //    Presenter.UpdateNextButton();
        //}

        private void textBoxGameModeName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //if (!String.IsNullOrEmpty(GameModeName) && !String.IsNullOrWhiteSpace(GameModeName))
            //    Presenter.GameModeNameChanged(GameModeName);
            Presenter.UpdateNextButton();
        }

        //private void textBoxShortcutFolder_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    if (!folderOperations.IsValidFolderPath(GameModeShortcutFolder))
        //        GameModeShortcutFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //    Presenter.UpdateNextButton();
        //}

        private void textBoxShortcutFolder_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!folderOperations.IsValidFolderPath(GameModeShortcutFolder))
                GameModeShortcutFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Presenter.UpdateNextButton();
        }

	    private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            var folder = folderOperations.GetSelectedPath();
            if (folderOperations.IsValidFolderPath(folder))
                GameModeShortcutFolder = folder;
            else
            if (!folderOperations.IsValidFolderPath(GameModeShortcutFolder))
                GameModeShortcutFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            Presenter.UpdateNextButton();
        }

        //private void textBoxGamePath_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    if (!fileOperations.IsValidFilePath(GamePath))
        //        GamePath = String.Empty;

        //    Presenter.UpdateNextButton();
        //}

        private void textBoxGamePath_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!FilePathHelper.IsValidPath(GamePath))
                GamePath = String.Empty;

            Presenter.UpdateNextButton();
        }

	    private void buttonBrowseGamePath_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            var gamePath = fileOperations.GetFilePath();
            if (FilePathHelper.IsValidPath(gamePath))
            {
                GamePath = gamePath;
                if (String.IsNullOrEmpty(GameModeName))
                    Presenter.GameModeNameChanged(FilePathHelper.GetFileDescription(gamePath));                                    
            }                

            Presenter.UpdateNextButton();
		}
		#endregion

        #region Overriding Methods
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			DragMove();
		}
		#endregion
    }
}