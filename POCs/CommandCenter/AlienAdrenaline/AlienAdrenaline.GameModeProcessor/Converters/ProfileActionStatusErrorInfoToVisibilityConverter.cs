using System;
using System.Windows;
using System.Windows.Data;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.GameModeProcessor.Converters
{
    [ValueConversion(typeof(ProfileActionStatus), typeof(Visibility))]
    public class ProfileActionStatusErrorInfoToVisibilityConverter : IValueConverter
	{
		#region Methods
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            var profileActionStatus = (ProfileActionStatus)value;
            if (profileActionStatus == ProfileActionStatus.ExecutionFailed || profileActionStatus == ProfileActionStatus.RollbackingFailed)
                return Visibility.Visible;

		    return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return null;
		}
		#endregion
	}
}