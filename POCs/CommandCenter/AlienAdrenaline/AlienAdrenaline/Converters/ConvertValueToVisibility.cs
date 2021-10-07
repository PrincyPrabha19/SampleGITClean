using System;
using System.Windows;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
    public class ConvertValueToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int val = (int)value;

            if (val != 0)
                return Visibility.Visible;

            if ((string)parameter == "Hidden")
                return Visibility.Hidden;
            
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}