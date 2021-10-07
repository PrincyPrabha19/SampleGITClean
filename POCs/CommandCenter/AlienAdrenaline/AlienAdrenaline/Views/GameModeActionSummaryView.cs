
using System.Collections.ObjectModel;
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.Domain.Classes;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeActionSummaryView : GameModeActionContentView, INotifyPropertyChanged
    {
        GameModeActionSummaryPresenter Presenter { get; set; }
        ObservableCollection<GameModeActionSummaryData> GameModeActionSummaryDataList { get; set; }
        void SetActionSummaryItemsSource();
    }
}
