using System;
using System.Net;

namespace Dominator.Tools.Classes
{
    public class WebDownload : WebClient
    {
        public int Timeout { get; set; }

        public WebDownload(int timeout)
        {
            Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
                request.Timeout = this.Timeout;
            return request;
        }
    }
}
