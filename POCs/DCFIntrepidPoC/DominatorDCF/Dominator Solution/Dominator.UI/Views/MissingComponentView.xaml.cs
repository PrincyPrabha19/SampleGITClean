using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Views
{
    public partial class MissingComponentView : IViewWithDataContextAndVisibility
    {
        public MissingComponentView()
        {
            ResourceDictionaryLoader.LoadInto(Resources);
            InitializeComponent();            
        }
    }
}
