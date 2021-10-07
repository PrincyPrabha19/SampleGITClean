using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Dominator.Domain;
using Dominator.Domain.Classes.Factories;

namespace Dominator.UI.Converters
{
    public class FanConverter:IValueConverter
    {
        private readonly IOverclockingModel ocModel = OverclockingFactory.NewOverclockingModel();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //0=PUMP 1=FAN
            var fanSpeed = System.Convert.ToDecimal(value);
            if (ocModel.GetFanType() > 0)
            {
                return fanSpeed > 0 ? string.Format(Properties.Resources.FanSpeedFormat, fanSpeed.ToString("0", CultureInfo.InvariantCulture)) :(fanSpeed == 0 ? Properties.Resources.FanMin : Properties.Resources.PumpOff);
            }
            return fanSpeed > 0 ? Properties.Resources.PumpOn : Properties.Resources.PumpOff;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}
