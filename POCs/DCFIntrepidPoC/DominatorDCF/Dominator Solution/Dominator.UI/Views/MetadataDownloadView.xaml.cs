using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Views
{
    public partial class MetadataDownloadView : IViewWithDataContextAndVisibility
    {
        public MetadataDownloadView()
        {
            ResourceDictionaryLoader.LoadInto(Resources);
            ResourceDictionaryLoader.LoadInto(Resources, "/Converters/ConverterDictionary.xaml");
            InitializeComponent();
        }
    }
}
