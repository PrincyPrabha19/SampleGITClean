using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Dominator.UI.Converters
{
    public class FanPumpConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fanType = System.Convert.ToDecimal(value);
            return fanType > 0 ? Properties.Resources.FanSpeed : Properties.Resources.Pump;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}
