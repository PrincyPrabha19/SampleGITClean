using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Dominator.Tools.Classes
{
    public class HttpUserAgentMessageInspector : IClientMessageInspector
    {
        private const string USER_AGENT_HTTP_HEADER = "user-agent";

        private string m_userAgent;

        public HttpUserAgentMessageInspector(string userAgent)
        {
            m_userAgent = userAgent;
        }

        #region IClientMessageInspector Members
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref Message request, System.ServiceModel.IClientChannel channel)
        {
            HttpRequestMessageProperty httpRequestMessage;

            object httpRequestMessageObject;

            if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out httpRequestMessageObject))
            {
                httpRequestMessage = httpRequestMessageObject as HttpRequestMessageProperty;
                if (httpRequestMessage != null && 
                    String.IsNullOrEmpty(httpRequestMessage.Headers[USER_AGENT_HTTP_HEADER]))
                    httpRequestMessage.Headers[USER_AGENT_HTTP_HEADER] = m_userAgent;
            }
            else
            {
                httpRequestMessage = new HttpRequestMessageProperty();
                httpRequestMessage.Headers.Add(USER_AGENT_HTTP_HEADER, m_userAgent);
                request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessage);
            }

            return null;
        }
        #endregion
    }
}