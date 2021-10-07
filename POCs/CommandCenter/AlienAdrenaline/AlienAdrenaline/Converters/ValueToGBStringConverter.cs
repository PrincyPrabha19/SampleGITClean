using System;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
    public class ValueToGBStringConverter : IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return String.Format(Properties.Resources.GBStr, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}

