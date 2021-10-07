using System;
using System.Windows;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.GameModeProcessor.Converters
{
    public class ConvertVisibilityToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((Visibility)value == Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}