using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Views
{
    public partial class ShellView : IViewWithDataContextAndVisibility
    {
        #region Constructor
        public ShellView()
        {
            ResourceDictionaryLoader.LoadInto(Resources);
            InitializeComponent();            
        }
        #endregion
    }
}
