using System.Collections.ObjectModel;
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeProfileListView : ContentView, INotifyPropertyChanged
    {
        ObservableCollection<Profile> Profiles { get; set; }
        void SetProfileListItemsSource();
    }
}
