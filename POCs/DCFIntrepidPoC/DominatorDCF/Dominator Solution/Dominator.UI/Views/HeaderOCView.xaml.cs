using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Views
{
    /// <summary>
    /// Interaction logic for MainOCView.xaml
    /// </summary>
    public partial class HeaderOCView : IViewWithDataContextAndVisibility
    {
        public HeaderOCView()
        {
            ResourceDictionaryLoader.LoadInto(Resources);
            InitializeComponent();
        }
    }
}
