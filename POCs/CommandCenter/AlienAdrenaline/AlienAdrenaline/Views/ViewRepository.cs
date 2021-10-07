using System.Collections.Generic;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface ViewRepository
    {
        View GetByType(ViewType type);
        IList<ContentView> GetAllContentViews();
    }
}