using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Views
{
    public partial class ProfileNavigationView : IViewWithDataContextAndVisibility
    {        
        public ProfileNavigationView()
        {
            ResourceDictionaryLoader.LoadInto(Resources);
            ResourceDictionaryLoader.LoadInto(Resources, "/Converters/ConverterDictionary.xaml");
            InitializeComponent();
        }
    }
}
