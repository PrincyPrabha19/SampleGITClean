using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Views
{
    /// <summary>
    /// Interaction logic for MainOCView.xaml
    /// </summary>
    public partial class AdvancedOCView : IViewWithDataContextAndVisibility
    {
        public AdvancedOCView()
        {
            ResourceDictionaryLoader.LoadInto(Resources);
            ResourceDictionaryLoader.LoadInto(Resources, "/Converters/ConverterDictionary.xaml");
            InitializeComponent();
        }
    }
}
