

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.Domain.Classes;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml.Actions
{
    /// <summary>
    /// Interaction logic for ActionSequenceSummaryUserControl.xaml
    /// </summary>
    public partial class SummaryUserControl : GameModeActionSummaryView
    {
        #region GameModeActionSummaryView Properties
        public GameModeActionViewType ActionType { get { return GameModeActionViewType.Summary; } }
        public GameModeActionSummaryPresenter Presenter { get; set; }
        public ObservableCollection<GameModeActionSummaryData> GameModeActionSummaryDataList { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event Action ProfileAction_Changed;

        public void Refresh()
        {
            Presenter.Refresh();
        }

        public void SetActionSummaryItemsSource()
        {
            listBoxActionSummary.Items.Clear();
            listBoxActionSummary.ItemsSource = GameModeActionSummaryDataList;
        }
        #endregion

        #region Constructors
        public SummaryUserControl()
        {
            InitializeComponent();
        }
        #endregion
    }
}
