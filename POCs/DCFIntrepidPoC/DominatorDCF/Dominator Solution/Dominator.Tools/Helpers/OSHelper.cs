using System;
using Dominator.Tools.Enums;

namespace Dominator.Tools.Helpers
{
    public static class OSHelper
    {
        public static OSType GetOSType()
        {
            if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1) return OSType.Win7;
            if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 2) return OSType.Win8;
            if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 3) return OSType.Win81;
            if (Environment.OSVersion.Version.Major == 10) return OSType.Win10;
            return OSType.Unknown;
        }
    }
}
