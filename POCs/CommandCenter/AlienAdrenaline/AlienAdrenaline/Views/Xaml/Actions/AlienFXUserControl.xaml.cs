
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.Domain;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml.Actions
{
    /// <summary>
    /// Interaction logic for WebLinks.xaml
    /// </summary>
    public partial class AlienFXUserControl : GameModeActionAlienFXView
    {
        #region GameModeActionWebLinksView Members
        public GameModeActionViewType ActionType { get { return GameModeActionViewType.AlienFX; } }        
        public GameModeActionAlienFXPresenter Presenter { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event Action ProfileAction_Changed;

        public ObservableCollection<AlienFXTheme> Themes
        {
            get { return comboBoxThemes.ItemsSource as ObservableCollection<AlienFXTheme>; }
            set { comboBoxThemes.ItemsSource = value; }
        }

        public AlienFXTheme ThemeSelected
        {
            get { return comboBoxThemes.SelectedItem as AlienFXTheme; }
            set
            {
                comboBoxThemes.SelectedIndex = comboBoxThemes.Items.IndexOf(value);
                if (value != null)
                    ThemeNameSelected = value.Name;
            }
        }

        private string themeNameSelected;
        public string ThemeNameSelected
        {
            get { return themeNameSelected; }
            set
            {
                themeNameSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ThemeNameSelected"));
            }
        }

        public bool IsUseCurrentAlienFXStateSelected
        {
            get { return checkBoxUseCurrentAlienFXState.IsChecked.Value; }
            set { checkBoxUseCurrentAlienFXState.IsChecked = value; }            
        }

        public bool IsEnableAlienFXAPISelected
        {
            get { return radioButtonEnableAlienFXAPI.IsChecked.Value; }
            set { radioButtonEnableAlienFXAPI.IsChecked = value; }
        }

        public bool IsPlayThemeSelected
        {
            get { return radioButtonPlayTheme.IsChecked.Value; }
            set { radioButtonPlayTheme.IsChecked = value; }
        }

        public bool IsGoDarkSelected
        {
            get { return radioButtonGoDark.IsChecked.Value; }
            set { radioButtonGoDark.IsChecked = value; }
        }

        public void Refresh()
        {
            Presenter.Refresh();
        }

        public void ShowThemeErrorMessage(bool show, string themeName = null)
        {
            ThemeNameSelected = themeName;

            stackPanelApplicationError.Visibility = 
                (show) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;            
        }
        #endregion

        #region Constructors
        public AlienFXUserControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        private void comboBoxThemes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Presenter.SetApplicationInfo();
        }

        private void checkBoxAlienFXOptions_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Presenter.SetApplicationInfo();
        }
        #endregion    
    }
}
