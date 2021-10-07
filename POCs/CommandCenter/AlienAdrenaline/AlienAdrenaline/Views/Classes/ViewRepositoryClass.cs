using System.Collections.Generic;
using System.Linq;

namespace AlienLabs.AlienAdrenaline.App.Views.Classes
{
    public class ViewRepositoryClass : ViewRepository
    {
        private ViewFactory viewFactory;
        public ViewFactory ViewFactory { set { viewFactory = value; } }

        private readonly IDictionary<ViewType, View> cache = new Dictionary<ViewType, View>();

        public View GetByType(ViewType type)
        {
            return cache.ContainsKey(type) ? cache[type] : newView(type);
        }

        public IList<ContentView> GetAllContentViews()
        {
	        return cache.Values.OfType<ContentView>().Select(view => view).ToList();
        }

	    private View newView(ViewType type)
        {
            var view = viewFactory.NewView(type);
            cache.Add(type, view);
            return view;
        }
    }
}