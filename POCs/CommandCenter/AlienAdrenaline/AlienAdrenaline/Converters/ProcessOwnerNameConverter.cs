using System;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
	[ValueConversion(typeof(string), typeof(string))]
	class ProcessOwnerNameConverter : IValueConverter
	{
		#region Methods
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
		    return Regex.Replace(value.ToString(), @"^.*\\", "");
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return null;
		}
		#endregion
	}
}