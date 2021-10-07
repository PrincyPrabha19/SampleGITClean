
using System.Collections.ObjectModel;
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.GameModeProcessor.Presenters;

namespace AlienLabs.AlienAdrenaline.GameModeProcessor
{
    public interface MainView : INotifyPropertyChanged
    {
        MainPresenter Presenter { get; set; }
        ObservableCollection<GameModeActionSummaryData> GameModeActionSummaryDataList { get; set; }
        string GameModeName { get; set; }
        string GameModeActionExecutionSummary { get; set; }
        bool GameRealPathNotFound { get; set; }
        bool GameApplicationNotEngaged { get; set; }
        bool RollBackActionEnded { get; set; }

        void SetActionSummaryItemsSource();
        void ScrollIntoView(int index);
        void MinimizeWindow();
        void RestoreWindow();
        void CloseWindow();
    }
}
