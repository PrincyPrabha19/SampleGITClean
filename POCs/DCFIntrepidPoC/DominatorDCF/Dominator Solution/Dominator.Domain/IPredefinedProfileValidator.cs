using Dominator.ServiceModel;

namespace Dominator.Domain
{
    public interface IPredefinedProfileValidator
    {
        IXTUService XTUService { set; }

        bool ValidateProfile(string profileName);
    }
}
