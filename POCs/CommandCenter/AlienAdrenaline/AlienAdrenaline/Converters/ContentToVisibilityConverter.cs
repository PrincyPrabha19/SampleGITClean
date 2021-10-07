using System;
using System.Windows;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
	[ValueConversion(typeof(object), typeof(Visibility))]
	public class ContentToVisibilityConverter : IValueConverter
	{
		#region Methods
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var content = value as string;
			return string.IsNullOrEmpty(content) ? Visibility.Visible : Visibility.Hidden;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return null;
		}
		#endregion
	}
}