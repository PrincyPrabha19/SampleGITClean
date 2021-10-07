using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.GameModeProcessor.Converters
{
    public class ConvertToItemNumber : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var item = value as ListBoxItem;
			if (item == null)
				return -1;

            var view = ItemsControl.ItemsControlFromItemContainer(item) as ListBox;
			if (view == null)
				return -1;

            int index = view.ItemContainerGenerator.IndexFromContainer(item);
            return String.Format("{0}.", index + 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}
