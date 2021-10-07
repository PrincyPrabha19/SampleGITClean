using System;
using System.Collections.Generic;

namespace AlienAdrenaline.SupportService
{
    public interface PowerPlanSupportReaderService
    {
        List<PowerPlan> GetAllPowerPlans();
        Guid GetActivePowerPlan();
    }
}
