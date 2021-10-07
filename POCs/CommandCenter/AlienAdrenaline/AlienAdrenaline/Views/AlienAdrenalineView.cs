using System.ComponentModel;
using System.Windows.Controls;
using AlienLabs.AlienAdrenaline.App.Presenters;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface AlienAdrenalineView : INotifyPropertyChanged
    {
        AlienAdrenalinePresenter Presenter { get; set; }
		NavigationView NavigationView { get; set; }
		ContentView ActiveView { get; set; }
        UserControl ActivePluginView { get; set; }
		ApplicationButtonsView ApplicationButtonsView { get; }
		CategorySelectorView CategorySelectorView { get; set; }

    	bool IsDirty { get; set; }
        bool IsDeleteEnabled { get; set; }
    }
}
