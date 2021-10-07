using LoadingExternalPackage.Controls.ViewModel;
using LoadingExternalPackage.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace LoadingExternalPackage.Controls
{
    public sealed partial class Alienware15R3 : UserControl
    {
        #region Properties

        public ControlViewModel ViewModel { get; set; }

        public List<GroupMapping> LedMapping { get; set; }
        public UIElement DeviceContent { get; set; }

        #endregion

        #region Constructor

        public Alienware15R3()
        {
            this.InitializeComponent();

            Task.FromResult(LoadExtension()).Wait();
            ViewModel = new ControlViewModel();
        }

        #endregion

        #region Methods

        private async Task LoadExtension()
        {
            var extensionManager = new ExtensionManager("extension01.alienfx");
            await extensionManager.LoadExtension();

            LedMapping = extensionManager.LedMapping;

            var controlContent = XamlReader.Load(extensionManager.DeviceContentText) as UIElement;
            Content = (Windows.UI.Xaml.Controls.UserControl)controlContent;
        }

        #endregion
    }
}
