using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class ThermalProfileClass : ThermalProfile
    {
        #region PowerPlan Members
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TempName { get; set; }
        #endregion
    }
}
