using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Views
{
    public partial class CategoryNavigationView : IViewWithDataContextAndVisibility
    {        
        public CategoryNavigationView()
        {
            ResourceDictionaryLoader.LoadInto(Resources);            
            InitializeComponent();
        }
    }
}
