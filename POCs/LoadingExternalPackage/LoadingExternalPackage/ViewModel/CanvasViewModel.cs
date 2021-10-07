using LoadingExternalPackage.Domain;
using System.Collections.Generic;
using LoadingExternalPackage.Controls;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace LoadingExternalPackage.ViewModel
{
    public class CanvasViewModel
    {
        #region Properties

        public UIElement DeviceContent { get; set; }
        public string DeviceContentText { get; set; }
        public List<GroupMapping> LedMapping { get; set; }

        #endregion

        #region Constructor
        
        public CanvasViewModel()
        {
            DeviceContent = new Alienware15R3();
            
            //load the content and mapping from app extension 
            //var extensionManager = new ExtensionManager("extension01.alienfx");
            //LedMappings = extensionManager.LedMapping;
            //DeviceContentText = extensionManager.DeviceContentText;
            //DeviceContent = extensionManager.DeviceContent;
        }

        #endregion
    }
}
