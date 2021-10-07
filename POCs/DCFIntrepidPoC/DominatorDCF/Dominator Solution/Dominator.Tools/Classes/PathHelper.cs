using System;
using System.IO;

namespace Dominator.Tools.Classes
{
    public static class PathHelper
    {
        public static bool IsValid(string path)
        {
            try
            {
                var tmpPath = Path.GetFullPath(path);
                return Path.IsPathRooted(tmpPath);
            }
            catch (Exception)
            {
                // ignored
            }

            return false;
        }
    }
}
