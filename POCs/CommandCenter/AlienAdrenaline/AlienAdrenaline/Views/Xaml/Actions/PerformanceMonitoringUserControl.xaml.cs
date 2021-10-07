
using System;
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.Tools;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml.Actions
{
    public partial class PerformanceMonitoringUserControl : GameModeActionPerformanceMonitoringView
    {
        #region GameModeActionEnergyBoosterView Members
        public GameModeActionViewType ActionType { get { return GameModeActionViewType.PerformanceMonitoring; } }        
        public GameModeActionPerformanceMonitoringPresenter Presenter { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event Action ProfileAction_Changed;

        public void Refresh()
        {
            Presenter.Refresh();          
        }
        #endregion

        #region Constructors
        public PerformanceMonitoringUserControl()
        {
            InitializeComponent();
        }
        #endregion   
    }
}
