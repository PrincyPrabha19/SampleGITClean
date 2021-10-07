using LoadingExternalPackage.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace LoadingExternalPackage.View
{
    public sealed partial class ControlCanvas : UserControl
    {
        #region Properties

        public CanvasViewModel CanvasVeiwModel { get; set; }
        
        #endregion

        #region Cosntructor

        public ControlCanvas()
        {
            this.InitializeComponent();
            
            CanvasVeiwModel = new CanvasViewModel();
            //LedMappingCount.Text = CanvasVeiwModel.DeviceContentText;
            //var controlContent = XamlReader.Load(CanvasVeiwModel.DeviceContentText) as UIElement;
            //var content = ((Windows.UI.Xaml.Controls.UserControl)controlContent).Content;
            Content = CanvasVeiwModel.DeviceContent;
            //Content = CanvasVeiwModel.DeviceContent;
        }

        #endregion
    }
}
