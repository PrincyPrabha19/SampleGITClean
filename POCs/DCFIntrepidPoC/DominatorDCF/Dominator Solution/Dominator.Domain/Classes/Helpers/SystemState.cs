using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Factories;

namespace Dominator.Domain.Classes.Helpers
{
    class SystemState
    {
        public IXTUService XTUService { private get; set; }

        public bool IsSystemOverclocked()
        {
            XTUService = ServiceRepository.XTUServiceInstance;
            if (XTUService == null)
                return false;
            var res = XTUService.IsSystemOverclocked();
            return res;
        }
    }
}
