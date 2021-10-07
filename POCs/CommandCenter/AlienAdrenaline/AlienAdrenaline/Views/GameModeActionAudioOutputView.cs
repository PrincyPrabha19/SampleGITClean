
using System.Collections.ObjectModel;
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.Domain.Classes;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeActionAudioOutputView : GameModeActionContentView, INotifyPropertyChanged
    {        
        GameModeActionAudioOutputPresenter Presenter { get; set; }
        ObservableCollection<AudioDeviceData> AudioDevices { get; set; }
        AudioDeviceData AudioDeviceSelected { get; set; }
    }
}
