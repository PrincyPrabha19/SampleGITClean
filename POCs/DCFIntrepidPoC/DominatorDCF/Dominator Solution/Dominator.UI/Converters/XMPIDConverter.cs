using System;
using System.Globalization;
using System.Windows.Data;

namespace Dominator.UI.Converters
{
    public class XMPIDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var profileID = System.Convert.ToDecimal(value);
            return profileID > 0 ? (profileID-1).ToString(CultureInfo.InvariantCulture) : Properties.Resources.Disabled;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}