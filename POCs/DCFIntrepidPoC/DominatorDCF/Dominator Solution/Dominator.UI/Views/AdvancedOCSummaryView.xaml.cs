using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Views
{
    public partial class AdvancedOCSummaryView : IViewWithDataContextAndVisibility
    {
        public AdvancedOCSummaryView()
        {
            ResourceDictionaryLoader.LoadInto(Resources);
            ResourceDictionaryLoader.LoadInto(Resources, "/Converters/ConverterDictionary.xaml");
            InitializeComponent();
        }
    }
}
