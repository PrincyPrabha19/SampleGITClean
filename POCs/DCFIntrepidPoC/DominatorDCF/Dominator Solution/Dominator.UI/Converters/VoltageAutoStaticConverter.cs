using System;
using System.Globalization;
using System.Windows.Data;

namespace Dominator.UI.Converters
{
    public class VoltageAutoStaticConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var voltage = System.Convert.ToDecimal(value);
            return voltage > 0 ? string.Format(parameter.ToString(), Properties.Resources.VoltageModeStatic) : string.Format(parameter.ToString(),Properties.Resources.VoltageModeAuto);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}
