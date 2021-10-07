using System;
using Dominator.Tools.Classes;

namespace Dominator.Tools.Helpers
{
    public static class WebHelper
    {
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebDownload(1000))
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        stream?.Close();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
