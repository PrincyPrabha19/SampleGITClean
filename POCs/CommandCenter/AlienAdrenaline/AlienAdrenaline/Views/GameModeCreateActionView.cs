
using System.Collections.ObjectModel;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeCreateActionView : ContentView
    {
        GameModeActionType GameModeActionTypeSelected { get; }
        ProfileAction ProfileAction { get; set; }
        ObservableCollection<GameModeActionType> AvailableActions { get; set; }
    }
}
