using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using LoadingExternalPackage.Controls;
using LoadingExternalPackage.Domain;
using LoadingExternalPackage.Domain.Helper;
using Windows.UI.Xaml.Input;

namespace LoadingExternalPackage.Controls.ViewModel
{
    public class ControlViewModel
    {
        #region Properties

        public UIElement ControlContent { get; set; }
        public IDeviceControlSettings ControlSettings { get; set; }

        public List<GroupMapping> GroupMappings { get; set; }

        #endregion
        
        #region Constructor

        public ControlViewModel()
        {

        }

        #endregion
    }
}
