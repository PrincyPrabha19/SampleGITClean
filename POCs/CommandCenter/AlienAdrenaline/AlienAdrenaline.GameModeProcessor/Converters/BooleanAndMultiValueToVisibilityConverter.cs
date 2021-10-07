using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.GameModeProcessor.Converters
{
    [ValueConversion(typeof(object[]), typeof(Visibility))]
    public class BooleanAndMultiValueToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            bool visible = values.Cast<bool>().Aggregate(true, (current, val) => current && val);
            if (visible)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

