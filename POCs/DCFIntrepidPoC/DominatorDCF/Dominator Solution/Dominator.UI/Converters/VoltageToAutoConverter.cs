using System;
using System.Globalization;
using System.Windows.Data;

namespace Dominator.UI.Converters
{
    public class VoltageAutoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var voltage = System.Convert.ToDecimal(value, CultureInfo.InvariantCulture);
            return voltage > 0 ? string.Format(System.Convert.ToString(parameter, CultureInfo.InvariantCulture), voltage) : Properties.Resources.VoltageModeAuto;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}