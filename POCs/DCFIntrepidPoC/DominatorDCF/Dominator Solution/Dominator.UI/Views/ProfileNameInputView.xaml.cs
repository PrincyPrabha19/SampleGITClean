using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Views
{
    public partial class ProfileNameInputView : IViewWithDataContextAndVisibility
    {
        public ProfileNameInputView()
        {
            ResourceDictionaryLoader.LoadInto(Resources);
            InitializeComponent();
        }
    }
}
