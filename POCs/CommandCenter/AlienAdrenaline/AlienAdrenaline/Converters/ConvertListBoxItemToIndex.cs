using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
    [ValueConversion(typeof(ListBoxItem), typeof(int))]
    public class ConvertListBoxItemToIndex : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var item = value as ListBoxItem;
			if (item == null)
				return -1;

            var view = ItemsControl.ItemsControlFromItemContainer(item) as ListBox;
        	if (view != null)
        		 return view.ItemContainerGenerator.IndexFromContainer(item);

			return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}
