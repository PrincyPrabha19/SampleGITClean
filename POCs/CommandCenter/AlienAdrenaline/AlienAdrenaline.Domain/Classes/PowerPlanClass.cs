using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class PowerPlanClass : PowerPlan
    {
        #region PowerPlan Members
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TempName { get; set; }
        public string Description { get; set; }
        #endregion
    }
}
