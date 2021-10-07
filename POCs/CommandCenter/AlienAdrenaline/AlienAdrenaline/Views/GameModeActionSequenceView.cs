
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.Domain;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeActionSequenceView : ContentView, INotifyPropertyChanged
    {        
        GameModeActionSequencePresenter Presenter { get; set; }
        GameModeActionContentView ActionDetailsView { get; set; }
        GameModeProfileActions GameModeProfileActions { get; set; }        
        string CurrentProfileActionTitle { get; set; }
        byte[] CurrentProfileActionImage { get; set; }
        void SetActionSequenceItemsSource();
        void ShowSummaryLink(bool show);
    }
}
