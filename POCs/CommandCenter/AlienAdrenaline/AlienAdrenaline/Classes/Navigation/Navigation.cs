using System.Windows;
using System.Windows.Controls;

namespace AlienLabs.AlienAdrenaline.App.Classes.Navigation
{
    public class Navigation : Menu
    {
        static Navigation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Navigation), new FrameworkPropertyMetadata(typeof(Navigation)));
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            foreach (var item in Items)
            {
                var navItem = item as NavigationItem;
				if (navItem == null) continue;

                navItem.Checked -= Navigation_Checked;
                navItem.Checked += Navigation_Checked;
            }
        }

        void Navigation_Checked(object sender, RoutedEventArgs e)
        {
            if (((NavigationItem)sender).IsChecked == false)
                return;

            foreach (var item in Items)
            {
                var navItem = item as NavigationItem;

				if (navItem == sender || navItem == null)
                    continue;

                navItem.IsChecked = false;
            }
        }
    }
}
