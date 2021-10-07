using System;
using System.IO;
using System.Windows;
using System.Windows.Data;
using AlienLabs.AlienAdrenaline.Tools;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
    public class FileNotFoundToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var path = value as string;
            if (!String.IsNullOrEmpty(path) && !FilePathHelper.IsValidPath(path))
                return Visibility.Visible;
           
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}