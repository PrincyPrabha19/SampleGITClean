using System;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class BooleanNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
