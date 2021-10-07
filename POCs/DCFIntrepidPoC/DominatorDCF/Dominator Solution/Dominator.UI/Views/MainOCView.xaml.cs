using System.Threading;
using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.Views
{
    /// <summary>
    /// Interaction logic for MainOCView.xaml
    /// </summary>
    public partial class MainOCView : IViewWithDataContextAndVisibility
    {
        public MainOCView()
        {
            //while (!System.Diagnostics.Debugger.IsAttached) Thread.Sleep(100);
            ResourceDictionaryLoader.LoadInto(Resources);
            InitializeComponent();
        }
    }
}
