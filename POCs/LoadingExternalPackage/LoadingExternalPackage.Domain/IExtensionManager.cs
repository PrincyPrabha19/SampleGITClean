using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace LoadingExternalPackage.Domain
{
    public interface IExtensionManager
    {
        List<GroupMapping> LedMapping { get; set; }
        UIElement DeviceContent { get; set; }
    }
}
