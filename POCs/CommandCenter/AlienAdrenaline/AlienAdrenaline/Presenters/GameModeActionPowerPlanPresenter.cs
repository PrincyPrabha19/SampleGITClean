using System;
using System.Linq;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;
using UsageTacking.Domain;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class GameModeActionPowerPlanPresenter
    {
        #region Public Properties
        public GameModeActionPowerPlanView View { get; set; }
        public GameModeActionSequenceService Model { get; set; }
        public PowerPlanService PowerPlanService { get; set; }
        public EventTrigger EventTrigger { get; set; }

        private ProfileActionInfoPowerPlanClass profileActionInfo;
        public ProfileActionInfoPowerPlanClass ProfileActionInfo
        {
            get
            {
                return profileActionInfo ??
                       (profileActionInfo = Model.CurrentProfileAction.ProfileActionInfo as ProfileActionInfoPowerPlanClass);
            }
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
        }

        public void Refresh()
        {            
            var powerPlans = PowerPlanService.GetAllPowerPlans();

            PowerPlan powerPlanSelected = null;
            Guid activePowerPlanId = PowerPlanService.GetActivePowerPlan();
            PowerPlan activePowerPlan = powerPlans.ToList().Find(pp => pp.Id == activePowerPlanId);
            if (activePowerPlan != null)
            {
                powerPlanSelected = new PowerPlanClass()
                {
                    Id = activePowerPlanId, 
                    Name = activePowerPlan.Name,
                    TempName = Properties.Resources.ActivePowerPlanText
                };

                powerPlans.Insert(0, powerPlanSelected);
            }            

            Guid profileActionInfoPowerPlanId = ProfileActionInfo.PowerPlanId;
            PowerPlan profileActionInfoPowerPlan = powerPlans.ToList().Find(pp => pp.Id == profileActionInfoPowerPlanId);
            string profileActionInfoPowerPlanName = ProfileActionInfo.PowerPlanName;

            View.PowerPlans = powerPlans;
            View.PowerPlanSelected =
                profileActionInfoPowerPlanId != Guid.Empty && profileActionInfoPowerPlanId != activePowerPlanId && profileActionInfoPowerPlan != null ? 
                    profileActionInfoPowerPlan : powerPlanSelected;

            if (profileActionInfoPowerPlanId != Guid.Empty && profileActionInfoPowerPlan == null)
                View.ShowPowerPlanErrorMessage(true, profileActionInfoPowerPlanName);
        }

        public void SetApplicationInfo()
        {
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeActionPowerPlan, DateTime.Now);

            View.ShowPowerPlanErrorMessage(false);

            ProfileActionInfo.PowerPlanId = View.PowerPlanSelected.Id;
            ProfileActionInfo.PowerPlanName = View.PowerPlanSelected.Name;

            EventTrigger.Fire(
                    CommandFactory.NewGameModePowerPlanCommand(
                        ProfileActionInfo.PowerPlanId, ProfileActionInfo.PowerPlanName, ProfileActionInfo.Guid));
        }
        #endregion
    }
}
