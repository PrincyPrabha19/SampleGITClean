using System;
using Windows.UI.Xaml.Data;
using static System.Int32;

namespace ResponsiveUISampleApp.Converters
{
    public class WidthStrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is Tuple<double,double> columnTuple)) return "";

            var param = (string) parameter;
            if (string.IsNullOrEmpty(param) || !TryParse(param, out int columns)) return "";

            var width = columnTuple.Item1 * columns + columnTuple.Item2 * (columns - 1);
            return width.ToString("####");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
