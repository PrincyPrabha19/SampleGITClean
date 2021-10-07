using System.ServiceModel;

namespace Dominator.Tools.Classes.Security
{
    public class AppAuthenticationException : FaultException
    {
        public AppAuthenticationException(FaultReason reason = null, FaultCode code = null)
            : base(reason, code)
        {
        }
    }
}