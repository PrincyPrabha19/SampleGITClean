
using System.Collections.ObjectModel;
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.Domain;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeActionThermalView : GameModeActionContentView, INotifyPropertyChanged
    {
        GameModeActionThermalPresenter Presenter { get; set; }       
        ObservableCollection<ThermalProfile> ThermalProfiles { get; set; }
        ThermalProfile ThermalProfileSelected { get; set; }

        void ShowThermalProfileErrorMessage(bool show, string thermalProfileName = null);
    }
}
