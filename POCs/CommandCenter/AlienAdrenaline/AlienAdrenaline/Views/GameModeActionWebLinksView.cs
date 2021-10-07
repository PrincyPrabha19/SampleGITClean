
using System.Collections.ObjectModel;
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.App.Presenters;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeActionWebLinksView : GameModeActionContentView, INotifyPropertyChanged
    {        
        GameModeActionWebLinksPresenter Presenter { get; set; }
        ObservableCollection<string> Urls { get; set; }
        bool EnableTabbedBrowsing { get; set; }
        void SetWebLinksItemsSource();
        void ScrollIntoView(int index);
    }
}
