using System;
using System.Globalization;
using System.Windows.Data;
using Dominator.Domain;
using Dominator.Domain.Classes.Factories;

namespace Dominator.UI.Converters
{
    public class CelciusFahrenheitConverter: IValueConverter
    {
        private IOverclockingModel model = OverclockingFactory.NewOverclockingModel();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var temp = System.Convert.ToDecimal(value);
            var celcius = Properties.Resources.TemperatureCelciusFormat;
            var fahrenheit = Properties.Resources.TemperatureFahrenheitFormat;
            return model.IsTempUnitCelsius ? string.Format(celcius.ToString(), temp) : string.Format(fahrenheit.ToString(), temp);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}
