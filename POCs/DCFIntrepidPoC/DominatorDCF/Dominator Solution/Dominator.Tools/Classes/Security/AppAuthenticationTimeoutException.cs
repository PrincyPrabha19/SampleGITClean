using System.ServiceModel;

namespace Dominator.Tools.Classes.Security
{
    public class AppAuthenticationTimeoutException : FaultException
    {
        public AppAuthenticationTimeoutException(FaultReason reason = null, FaultCode code = null)
            : base(reason, code)
        {
        }
    }
}
