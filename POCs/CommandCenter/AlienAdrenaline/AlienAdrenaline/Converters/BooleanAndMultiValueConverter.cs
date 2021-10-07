using System;
using System.Linq;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
    public class BooleanAndMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return values.Cast<bool>().Aggregate(true, (current, val) => current && val);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

