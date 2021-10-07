
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.App.Presenters;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeActionApplicationView : GameModeActionContentView, INotifyPropertyChanged
    {
        GameModeActionApplicationPresenter Presenter { get; set; }
        string ApplicationName { get; set; }
        string ApplicationPath { get; set; }
    }
}
