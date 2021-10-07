using System.Configuration;
using System.Globalization;

namespace Dominator.Tools.Helpers
{
    public static class UICulture
    {
        public static CultureInfo Current => !string.IsNullOrEmpty(ConfigurationManager.AppSettings["Culture"])
            ? new CultureInfo(ConfigurationManager.AppSettings["Culture"]) : CultureInfo.CurrentUICulture;
    }
}