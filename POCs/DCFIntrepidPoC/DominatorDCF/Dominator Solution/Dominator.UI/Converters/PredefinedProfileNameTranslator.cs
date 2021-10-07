using System;
using System.Windows.Data;

namespace Dominator.UI.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class PredefinedProfileNameTranslator : IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var name = value?.ToString().ToUpper();
            if (string.IsNullOrEmpty(name)) return string.Empty;
            var resource = Properties.Resources.ResourceManager.GetObject(name);
            if (resource != null) return resource.ToString();
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}