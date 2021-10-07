
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.App.Presenters;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeActionEnergyBoosterView : GameModeActionContentView, INotifyPropertyChanged
    {
        GameModeActionEnergyBoosterPresenter Presenter { get; set; }

        bool IsUpdateDefinitionsEnabled { get; set; }
        string UpdateDefinitionsStatus { get; set; }
    }
}
