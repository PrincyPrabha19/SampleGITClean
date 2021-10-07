using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ExternalDllLoading
{
    public class XamlToFrameworkElement : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string xaml = null;
            string route = value as string;
            FrameworkElement result = null;
            if (parameter != null)
            {
                route = parameter as string;
            }

            if (String.IsNullOrEmpty(route))
            {
                return null;
            }

            //start reading
            try
            {
                StreamResourceInfo si = Application.GetResourceStream(new Uri(route, UriKind.Relative));
                if (si != null && si.Stream != null)
                {
                    using (var sr = new System.IO.StreamReader(si.Stream))
                    {
                        xaml = sr.ReadToEnd();
                    }
                }

                result = XamlReader.Load(xaml) as FrameworkElement;
            }
            catch (Exception)
            {
                return null;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
