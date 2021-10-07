using System.Windows;
using System.Windows.Controls;

namespace AlienLabs.AlienAdrenaline.App.Classes.Navigation
{
    public class NavigationItem : MenuItem
    {
        static NavigationItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationItem), new FrameworkPropertyMetadata(typeof(NavigationItem)));
        }
    }
}
