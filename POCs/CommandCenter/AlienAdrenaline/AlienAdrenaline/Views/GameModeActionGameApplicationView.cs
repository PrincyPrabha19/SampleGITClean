

using System.ComponentModel;
using AlienLabs.AlienAdrenaline.App.Presenters;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeActionGameApplicationView : GameModeActionContentView, INotifyPropertyChanged
    {
        GameModeActionGameApplicationPresenter Presenter { get; set; }
        string GameTitle { get; set; }        
        byte[] GameImage { get; set; }
        string GamePath { get; set; }
        string GameRealPath { get; set; }
    }
}
