using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Views
{
    public partial class CPUCategoryDetailsView : IViewWithDataContextAndVisibility
    {        
        public CPUCategoryDetailsView()
        {
            ResourceDictionaryLoader.LoadInto(Resources);
            ResourceDictionaryLoader.LoadInto(Resources, "/Converters/ConverterDictionary.xaml");
            InitializeComponent();

            //addCustomEventHandlers();
        }

        //private void addCustomEventHandlers()
        //{
        //    AddHandler(CustomRoutedEvents.CommandEvent, new RoutedEventHandler(addCommand), true);
        //}

        //private void addCommand(object sender, RoutedEventArgs e)
        //{
        //    var args = e as CustomRoutedEvents.CommandEventArgs;
        //    if (args != null)
        //    {
        //        //Presenter.Receive(args.equatableCommand);
        //    }
        //}
    }
}
