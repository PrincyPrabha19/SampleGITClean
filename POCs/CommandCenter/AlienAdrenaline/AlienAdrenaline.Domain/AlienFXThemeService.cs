

using System.Collections.ObjectModel;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface AlienFXThemeService
    {
        ObservableCollection<AlienFXTheme> GetAllThemes();
        string GetActiveTheme();
        bool IsGoDark();
        bool IsAlienFXAPIEnabled();
        bool ExistsAlienFXTheme(string themePath);
        void SetActiveTheme(string path);
        void GoDark();
        void GoLight();
        void EnableAlienFXAPI();
        void DisableAlienFXAPI();        
    }
}
