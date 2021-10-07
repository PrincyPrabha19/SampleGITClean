using System;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.GameModeProcessor.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    public class ProfileActionStatusProgressConverter : IValueConverter
	{
		#region Methods
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            var progress = (double?)value;
            if (progress.HasValue)
                return String.Format(Properties.Resources.ActionProgressText, progress);
		    return String.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return null;
		}
		#endregion
	}
}