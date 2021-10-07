using System;
using System.Windows;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
	[ValueConversion(typeof(object), typeof(Visibility))]
	public class ContentToCollapsedVisibilityConverter : IValueConverter
	{
		#region Methods
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var content = value as string;
			return string.IsNullOrEmpty(content) ?  Visibility.Collapsed : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return null;
		}
		#endregion
	}
}