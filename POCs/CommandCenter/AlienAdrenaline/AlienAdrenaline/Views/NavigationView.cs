using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.CC_PlugIn;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface NavigationView : View
    {
        event Action<string> PlugInViewActivated;

        NavigationPresenter Presenter { get; set; }
        IList<ViewType> Links { get; set; }
        ViewType ViewLinkSelected { get; set; }
        Profile GameModeSelected { get; set; }        
        ObservableCollection<Profile> Profiles { get; set; }

        void Refresh();
        void UpdateNavigationPluginLinks(string plugInName);
        void ShowDynamicLightingControl();
        void HideDynamicLightingControl();
    }
}