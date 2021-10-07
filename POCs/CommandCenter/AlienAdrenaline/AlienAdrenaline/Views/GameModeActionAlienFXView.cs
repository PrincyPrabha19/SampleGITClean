
using System.Collections.ObjectModel;
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.Domain;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeActionAlienFXView : GameModeActionContentView, INotifyPropertyChanged
    {
        GameModeActionAlienFXPresenter Presenter { get; set; }       
        ObservableCollection<AlienFXTheme> Themes { get; set; }
        AlienFXTheme ThemeSelected { get; set; }

        bool IsUseCurrentAlienFXStateSelected { get; set; }
        bool IsEnableAlienFXAPISelected { get; set; }
        bool IsPlayThemeSelected { get; set; }
        bool IsGoDarkSelected { get; set; }

        void ShowThemeErrorMessage(bool show, string themeName = null);
    }
}
