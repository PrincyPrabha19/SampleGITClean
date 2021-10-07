using System;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
    [ValueConversion(typeof(int), typeof(int))]
    public class ConvertArrayIndexToViewIndex : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int index = (int) value;
            return index + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}
