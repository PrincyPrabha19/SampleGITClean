using System;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Format("({0}%)", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}