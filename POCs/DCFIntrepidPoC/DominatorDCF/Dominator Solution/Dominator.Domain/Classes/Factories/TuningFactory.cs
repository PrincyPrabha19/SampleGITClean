using Dominator.Domain.Classes.Helpers;
using Dominator.ServiceModel.Classes.Factories;

namespace Dominator.Domain.Classes.Factories
{
    public static class TuningFactory
    {
        private static ITuningManager tuningManager;
        public static ITuningManager NewTuningManager()
        {
            var xtuService = ServiceRepository.XTUServiceInstance;
            return tuningManager ?? (tuningManager = new TuningManager
            {
                CPUTuning = new CPUTuning { XTUService = xtuService },
                MemoryTuning = new MemoryTuning { XTUService = xtuService}
            });
        }
    }
}
