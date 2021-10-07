using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Factories;

namespace Dominator.Domain.Classes.Helpers
{
    public class PredefinedProfileValidator : IPredefinedProfileValidator
    {
        public IXTUService XTUService { private get; set; }

        public bool ValidateProfile(string profileName)
        {
            XTUService = ServiceRepository.XTUServiceInstance;
            if (XTUService == null)
                return false;

            if (!XTUService.GetProcessorBrand().Contains(profileName.Substring(0, profileName.LastIndexOf('_'))))
                return false;
            return true;
        }
    }
}
