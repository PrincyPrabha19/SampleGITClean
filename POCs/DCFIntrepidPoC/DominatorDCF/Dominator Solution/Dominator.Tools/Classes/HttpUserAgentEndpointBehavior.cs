using System.ServiceModel.Description;

namespace Dominator.Tools.Classes
{
    public class HttpUserAgentEndpointBehavior : IEndpointBehavior
    {
        private string m_userAgent;

        public HttpUserAgentEndpointBehavior(string userAgent)
        {
            m_userAgent = userAgent;
        }

        #region IEndpointBehavior Members
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            var inspector = new HttpUserAgentMessageInspector(m_userAgent);
            clientRuntime.MessageInspectors.Add(inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
        #endregion
    }
}