
using System.Collections.ObjectModel;
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.Domain;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeActionPowerPlanView : GameModeActionContentView, INotifyPropertyChanged
    {
        GameModeActionPowerPlanPresenter Presenter { get; set; }       
        ObservableCollection<PowerPlan> PowerPlans { get; set; }
        PowerPlan PowerPlanSelected { get; set; }

        void ShowPowerPlanErrorMessage(bool show, string powerPlanName = null);
    }
}
