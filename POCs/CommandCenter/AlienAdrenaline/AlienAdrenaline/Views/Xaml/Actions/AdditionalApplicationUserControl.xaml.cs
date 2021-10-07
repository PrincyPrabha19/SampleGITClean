using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AlienLabs.AlienAdrenaline.App.Factories;
using AlienLabs.AlienAdrenaline.App.Presenters;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml.Actions
{
    public partial class AdditionalApplicationUserControl : GameModeActionAdditionalApplicationView
    {
        #region Private Properties
        private readonly FileOperations fileOperations = ObjectFactory.NewFileOperations();
        #endregion

        #region GameModeActionWebLinksView Members
        public GameModeActionViewType ActionType { get { return GameModeActionViewType.AdditionalApplication; } }        
        public GameModeActionAdditionalApplicationPresenter Presenter { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event Action ProfileAction_Changed;

        public string ApplicationName { get; set; }

        public string ApplicationPath
        {
            get { return textBoxApplicationPath.Text; }
            set { textBoxApplicationPath.Text = value; }
        }

        public bool LaunchIfNotOpen
        {
            get { return radioButtonLaunchIfNotOpen.IsChecked.Value; }
            set { radioButtonLaunchIfNotOpen.IsChecked = value; }
        }

        public bool ExitIfOpen
        {
            get { return radioButtonExitIfOpen.IsChecked.Value; }
            set { radioButtonExitIfOpen.IsChecked = value; }
        }

        public void Refresh()
        {
            Presenter.Refresh();
        }
        #endregion

        #region Constructors
        public AdditionalApplicationUserControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            var applicationPath = fileOperations.GetFilePath();
            if (!String.IsNullOrEmpty(applicationPath))
                ApplicationPath = applicationPath;

            if (isApplicationPathValid())
                Presenter.SetApplicationInfo();

            if (ProfileAction_Changed != null)
                ProfileAction_Changed();
        }

        private void textBoxApplicationPath_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!isApplicationPathValid())
                Presenter.SetApplicationInfo();

            if (ProfileAction_Changed != null)
                ProfileAction_Changed();
        }
		
        private void radioButtonApplicationBehavior_Click(object sender, RoutedEventArgs e)
        {
            Presenter.SetApplicationInfo();
        }
        #endregion    
    
        #region Private Methods
        private bool isApplicationPathValid()
        {
            stackPanelApplicationError.Visibility = Visibility.Collapsed;
            if (!Presenter.IsValidApplicationPath())
            {
                stackPanelApplicationError.Visibility = Visibility.Visible;
                return false;
            }

            return true;
        }
        #endregion
    }
}
