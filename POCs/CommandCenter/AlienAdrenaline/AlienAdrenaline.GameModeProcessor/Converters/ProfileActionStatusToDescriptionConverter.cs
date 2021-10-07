using System;
using System.Windows;
using System.Windows.Data;
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;

namespace AlienLabs.AlienAdrenaline.GameModeProcessor.Converters
{
    [ValueConversion(typeof(ProfileActionStatus), typeof(string))]
    public class ProfileActionStatusToDescriptionConverter : IValueConverter
	{
		#region Methods
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            var profileActionStatus = (ProfileActionStatus)value;
            string resourceKey = EnumHelper.GetAttributeValue<ResourceKeyAttributeClass, string>(profileActionStatus);
            if (!String.IsNullOrEmpty(resourceKey))
                return Properties.Resources.ResourceManager.GetString(resourceKey);
            return String.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return null;
		}
		#endregion
	}
}