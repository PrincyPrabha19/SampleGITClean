using System.ComponentModel;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeView : ContentView, INotifyPropertyChanged
    {
        GameModePresenter Presenter { get; set; }
        Profile Profile { get; set; }
        GameModeActionSequenceView ActionSequenceView { get; set; }
        byte[] GameImage { get; set; }
        string GamePath { get; set; }
    }
}
