
using System;
using System.ComponentModel;
using System.Configuration;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.Tools;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml.Actions
{
    public partial class EnergyBoosterUserControl : GameModeActionEnergyBoosterView
    {
        #region GameModeActionEnergyBoosterView Members
        public GameModeActionViewType ActionType { get { return GameModeActionViewType.EnergyBooster; } }        
        public GameModeActionEnergyBoosterPresenter Presenter { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event Action ProfileAction_Changed;

        private bool isUpdateDefinitionsEnabled = true;
        public bool IsUpdateDefinitionsEnabled
        {
            get { return isUpdateDefinitionsEnabled; }
            set
            {
                isUpdateDefinitionsEnabled = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsUpdateDefinitionsEnabled"));
            }
        }

        private string updateDefinitionsStatus;
        public string UpdateDefinitionsStatus
        {
            get { return updateDefinitionsStatus; }
            set
            {
                updateDefinitionsStatus = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UpdateDefinitionsStatus"));
            }
        }

        public void Refresh()
        {
            Presenter.Refresh();

            if (Presenter.AutoUpdateDefinitions &&
                MsgBox.Show(Properties.Resources.UpdateDefinitionsTitleText,
                    Properties.Resources.UpdateDefinitionsQuestionText, MsgBoxIcon.Question, MsgBoxButtons.YesNo) == MsgBoxResult.Yes)
                Presenter.UpdateDefinitions();                
        }
        #endregion

        #region Constructors
        public EnergyBoosterUserControl()
        {
            InitializeComponent();
        }
        #endregion   

        #region Event Handlers
        private void buttonUpdate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Presenter.UpdateDefinitions();
        }

        private void linkClickhere_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Presenter.OpenEnergyBoosterSiteUrl();
        }
        #endregion
    }
}
