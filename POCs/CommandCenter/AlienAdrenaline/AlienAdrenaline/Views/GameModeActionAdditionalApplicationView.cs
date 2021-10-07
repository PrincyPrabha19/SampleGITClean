
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.App.Presenters;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeActionAdditionalApplicationView : GameModeActionContentView, INotifyPropertyChanged
    {
        GameModeActionAdditionalApplicationPresenter Presenter { get; set; }
        string ApplicationName { get; set; }
        string ApplicationPath { get; set; }
        bool LaunchIfNotOpen { get; set; }
        bool ExitIfOpen { get; set; }
    }
}
