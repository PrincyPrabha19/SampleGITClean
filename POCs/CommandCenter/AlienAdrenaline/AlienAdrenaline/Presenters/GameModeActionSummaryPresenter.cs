using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class GameModeActionSummaryPresenter
    {
        #region Public Properties
        public GameModeActionSummaryView View { get; set; }
        public GameModeActionSummaryService Model { get; set; }
        #endregion

        #region Public Methods
        public void Initialize()
        {
        }

        public void Refresh()
        {
            Model.Refresh();            
            View.GameModeActionSummaryDataList = Model.GameModeActionSummaryDataList;
            View.SetActionSummaryItemsSource();
        }
        #endregion
    }
}
