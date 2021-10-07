

using System;
using System.Data;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileActionInfoPowerPlanClass : ProfileActionInfoProcessorClass, ProfileActionInfo
    {
        #region Public Properties
        public Guid PowerPlanId { get; set; }
        public string PowerPlanName { get; set; }
        #endregion

        #region ProfileActionInfo Members
        public Guid Guid { get; private set; }
        public int Id { get; set; }
        public int ProfileActionId { get; set; }

        public override void Execute()
        {
            if (!ServiceFactory.PowerPlanService.ExistsPowerPlan(PowerPlanId))
                throw new Exception(Properties.Resources.PowerPlanDoesNotExistsErrorText);

            ServiceFactory.PowerPlanService.SetPowerPlan(PowerPlanId);                            
        }

        public ProfileActionInfo Clone()
        {
            return new ProfileActionInfoPowerPlanClass()
            {
                Guid = Guid,
                PowerPlanId = PowerPlanId,
                PowerPlanName = PowerPlanName
            };
        }

        public bool Equals(ProfileActionInfo profileActionInfo)
        {
            return Guid == ((ProfileActionInfoPowerPlanClass)profileActionInfo).Guid &&
                   PowerPlanId == ((ProfileActionInfoPowerPlanClass)profileActionInfo).PowerPlanId &&
                   PowerPlanName == ((ProfileActionInfoPowerPlanClass)profileActionInfo).PowerPlanName;
        }

        public ProfileActionStatus GetStatus()
        {
            return ProfileActionStatus.None;
        }
        #endregion

        #region Constructors
        public ProfileActionInfoPowerPlanClass()
        {
            Guid = Guid.NewGuid();
            PowerPlanId = Guid.Empty;
            PowerPlanName = String.Empty;
        }

        public ProfileActionInfoPowerPlanClass(bool initialize) 
            : this()
        {
            if (initialize)
            {
                var powerPlanService = ServiceFactory.PowerPlanService;
                if (powerPlanService != null)
                    PowerPlanId = powerPlanService.GetActivePowerPlan();
            }                        
        }
        #endregion
    }
}
