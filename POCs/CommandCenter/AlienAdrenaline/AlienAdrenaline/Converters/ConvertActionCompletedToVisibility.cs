using System;
using System.Windows;
using System.Windows.Data;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
    [ValueConversion(typeof(ProfileAction), typeof(Visibility))]
    public class ConvertActionCompletedToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var profileAction = value as ProfileAction;
            if (profileAction != null)
            {
                if (profileAction.ProfileActionInfo.GetStatus() == ProfileActionStatus.NotReady)
                    return Visibility.Visible;
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}
