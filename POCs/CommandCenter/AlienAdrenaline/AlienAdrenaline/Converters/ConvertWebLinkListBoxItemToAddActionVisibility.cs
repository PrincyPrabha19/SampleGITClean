using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
    [ValueConversion(typeof(ListBoxItem), typeof(Visibility))]
    public class ConvertWebLinkListBoxItemToAddActionVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {            
            var isAddButton = System.Convert.ToBoolean(parameter);

            var item = value as ListBoxItem;
			if (item == null)
				return Visibility.Hidden;

            var view = ItemsControl.ItemsControlFromItemContainer(item) as ListBox;
			if (view == null)
				return Visibility.Hidden;

            int index = view.ItemContainerGenerator.IndexFromContainer(item);
            if (isAddButton)
                return (index >= view.Items.Count - 1) ? Visibility.Visible : Visibility.Hidden;
            
            return (index >= 0) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}
