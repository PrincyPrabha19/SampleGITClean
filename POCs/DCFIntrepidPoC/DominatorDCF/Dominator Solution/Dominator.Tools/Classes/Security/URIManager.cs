using System;

namespace Dominator.Tools.Classes.Security
{
    public static class URIManager
    {
        #region Properties
        private const string URI_TEMPLATE = "net.pipe://localhost/{0}";
        #endregion

        #region Methods
        public static string GetURI(Type classType)
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, URI_TEMPLATE, classType.Name);
        }

        public static string GetUniqueURI()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, URI_TEMPLATE, Guid.NewGuid());
        }
        #endregion
    }
}
