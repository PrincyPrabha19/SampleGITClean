using System;
using System.Windows.Data;
using AlienLabs.AlienAdrenaline.App.Helpers;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
    public class ToHourMinuteSeconds : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return TimeSpanHelper.ConvertTotalSecondsToHourMinutesSeconds(System.Convert.ToInt32(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}