using System.Security.Cryptography.X509Certificates;
using System.Windows;

namespace Dominator.UI
{
    public interface IViewWithDataContextAndVisibility
    {
        object DataContext { get; set; }
        Visibility Visibility { get; set; }

        event RoutedEventHandler Loaded;
    }
}
