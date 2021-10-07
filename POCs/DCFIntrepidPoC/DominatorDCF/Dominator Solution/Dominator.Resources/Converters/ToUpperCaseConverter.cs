using System;
using System.Windows.Data;

namespace Dominator.Resources.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    class ToUpperCaseConverter : IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string text = "";
            if (value is string)
                text = (string)value;

            return text.ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}