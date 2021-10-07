
using System.Collections.ObjectModel;
using AlienLabs.GameDiscoveryHelper;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeCreateView : ContentView
    {
        string GameModeName { get; set; }
        string GameModeShortcutFolder { get; set; }
        string GamePath { get; set; }
        GameData GameSelected { get; set; }
        ObservableCollection<GameData> Games { get; set; }
        bool IsNextEnabled { get; set; }
        void SetGamesItemsSource();            
    }
}
