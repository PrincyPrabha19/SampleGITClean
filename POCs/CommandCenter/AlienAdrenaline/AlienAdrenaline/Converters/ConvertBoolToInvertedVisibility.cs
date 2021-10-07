using System;
using System.Windows;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
    public class ConvertBoolToInvertedVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool boolValue;

            if (value is string)
                boolValue = Boolean.Parse((string)value);
            else if (value is bool)
                boolValue = (bool)value;
            else
                return Visibility.Collapsed;

            if (!boolValue)
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