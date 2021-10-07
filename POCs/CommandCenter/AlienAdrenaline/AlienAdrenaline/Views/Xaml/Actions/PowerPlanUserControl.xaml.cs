
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
    public partial class PowerPlanUserControl : GameModeActionPowerPlanView
    {
        #region GameModeActionWebLinksView Members
        public GameModeActionViewType ActionType { get { return GameModeActionViewType.PowerPlan; } }        
        public GameModeActionPowerPlanPresenter Presenter { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event Action ProfileAction_Changed;

        public ObservableCollection<PowerPlan> PowerPlans
        {
            get { return comboBoxPowerPlans.ItemsSource as ObservableCollection<PowerPlan>; }
            set { comboBoxPowerPlans.ItemsSource = value; }
        }

        public PowerPlan PowerPlanSelected
        {
            get { return comboBoxPowerPlans.SelectedItem as PowerPlan; }
            set
            {
                comboBoxPowerPlans.SelectedIndex = comboBoxPowerPlans.Items.IndexOf(value);
                if (value != null)
                    PowerPlanNameSelected = value.Name;
            }
        }

        private string powerPlanNameSelected;
        public string PowerPlanNameSelected
        {
            get
            {
                return powerPlanNameSelected;
            }
            set
            {
                powerPlanNameSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("PowerPlanNameSelected"));
            }
        }

        public void Refresh()
        {
            Presenter.Refresh();
        }

        public void ShowPowerPlanErrorMessage(bool show, string powerPlanName = null)
        {
            PowerPlanNameSelected = powerPlanName;

            stackPanelApplicationError.Visibility = 
                (show) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;            
        }
        #endregion

        #region Constructors
        public PowerPlanUserControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        private void comboBoxPowerPlans_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Presenter.SetApplicationInfo();
        }
        #endregion    
    }
}
