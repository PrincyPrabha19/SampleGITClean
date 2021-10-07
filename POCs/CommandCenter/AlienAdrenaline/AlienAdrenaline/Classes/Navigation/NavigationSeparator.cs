using System.Windows;
using System.Windows.Controls;

namespace AlienLabs.AlienAdrenaline.App.Classes.Navigation
{
    public class NavigationSeparator : MenuItem
    {
        static NavigationSeparator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationSeparator), new FrameworkPropertyMetadata(typeof(NavigationSeparator)));
        }
    }
}
