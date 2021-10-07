
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.App.Presenters;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeActionPerformanceMonitoringView : GameModeActionContentView, INotifyPropertyChanged
    {
        GameModeActionPerformanceMonitoringPresenter Presenter { get; set; }
    }
}
