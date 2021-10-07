
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
    public partial class ThermalUserControl : GameModeActionThermalView
    {
        #region GameModeActionThermalView Members
        public GameModeActionViewType ActionType { get { return GameModeActionViewType.Thermal; } }        
        public GameModeActionThermalPresenter Presenter { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event Action ProfileAction_Changed;

        public ObservableCollection<ThermalProfile> ThermalProfiles
        {
            get { return comboBoxThermalProfiles.ItemsSource as ObservableCollection<ThermalProfile>; }
            set { comboBoxThermalProfiles.ItemsSource = value; }
        }

        public ThermalProfile ThermalProfileSelected
        {
            get { return comboBoxThermalProfiles.SelectedItem as ThermalProfile; }
            set
            {
                comboBoxThermalProfiles.SelectedIndex = comboBoxThermalProfiles.Items.IndexOf(value);
                if (value != null)
                    ThermalProfileNameSelected = value.Name;
            }
        }

        private string thermalProfileNameSelected;
        public string ThermalProfileNameSelected
        {
            get
            {
                return thermalProfileNameSelected;
            }
            set
            {
                thermalProfileNameSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ThermalProfileNameSelected"));
            }
        }

        public void Refresh()
        {
            Presenter.Refresh();
        }

        public void ShowThermalProfileErrorMessage(bool show, string thermalProfileName = null)
        {
            ThermalProfileNameSelected = thermalProfileName;

            stackPanelApplicationError.Visibility = 
                (show) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;            
        }
        #endregion

        #region Constructors
        public ThermalUserControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        private void comboBoxThermalProfiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Presenter.SetApplicationInfo();
        }
        #endregion    
    }
}
