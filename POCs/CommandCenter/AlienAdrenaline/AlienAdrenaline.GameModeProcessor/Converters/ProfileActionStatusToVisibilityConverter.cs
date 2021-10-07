using System;
using System.Windows;
using System.Windows.Data;
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.GameModeProcessor.Converters
{
    [ValueConversion(typeof(ProfileActionStatus), typeof(Visibility))]
    public class ProfileActionStatusToVisibilityConverter : IValueConverter
	{
		#region Methods
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            var profileActionStatus = (ProfileActionStatus)value;
			return profileActionStatus != ProfileActionStatus.None ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return null;
		}
		#endregion
	}
}