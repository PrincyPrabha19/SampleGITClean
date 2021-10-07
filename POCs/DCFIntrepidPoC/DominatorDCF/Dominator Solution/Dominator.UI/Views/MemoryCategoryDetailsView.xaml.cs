using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Views
{
    public partial class MemoryCategoryDetailsView : IViewWithDataContextAndVisibility
    {        
        public MemoryCategoryDetailsView()
        {
            ResourceDictionaryLoader.LoadInto(Resources);
            ResourceDictionaryLoader.LoadInto(Resources, "/Converters/ConverterDictionary.xaml");
            InitializeComponent();
        }
    }
}
