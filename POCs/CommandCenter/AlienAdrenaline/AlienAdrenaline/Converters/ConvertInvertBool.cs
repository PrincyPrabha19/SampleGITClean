﻿using System;
using System.Windows.Data;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
    public class ConvertInvertBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}