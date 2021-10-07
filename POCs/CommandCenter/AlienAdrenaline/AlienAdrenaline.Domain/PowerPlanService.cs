

using System;
using System.Collections.ObjectModel;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface PowerPlanService
    {
        ObservableCollection<PowerPlan> GetAllPowerPlans();
        Guid GetActivePowerPlan();
        void SetPowerPlan(Guid id);
        bool ExistsPowerPlan(Guid id);
    }
}
