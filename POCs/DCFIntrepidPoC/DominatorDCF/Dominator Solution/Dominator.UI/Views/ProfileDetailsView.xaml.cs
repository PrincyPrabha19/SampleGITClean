using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Views
{
    public partial class ProfileDetailsView : IViewWithDataContextAndVisibility
    {
        public ProfileDetailsView()
        {
            ResourceDictionaryLoader.LoadInto(Resources);
            InitializeComponent();
        }
    }
}
