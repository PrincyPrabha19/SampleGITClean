using System;
using System.Windows;
using System.Windows.Data;
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
    [ValueConversion(typeof(GameModeActionType), typeof(Visibility))]
    public class ConvertActionTypeToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool deletionAllowed = EnumHelper.GetAttributeValue<AllowDeletionAttributeClass, bool>((GameModeActionType)value);
            return (deletionAllowed) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}
