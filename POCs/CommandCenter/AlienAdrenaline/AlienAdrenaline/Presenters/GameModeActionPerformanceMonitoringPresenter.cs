using System;
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;
using UsageTacking.Domain;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class GameModeActionPerformanceMonitoringPresenter
    {
        #region Public Properties
        public GameModeActionPerformanceMonitoringView View { get; set; }
        public GameModeActionSequenceService Model { get; set; }
        //public PerformanceMonitoringService PerformanceMonitoringService { get; set; }
        public EventTrigger EventTrigger { get; set; }

        private ProfileActionInfoPerformanceMonitoringClass profileActionInfo;
        public ProfileActionInfoPerformanceMonitoringClass ProfileActionInfo
        {
            get
            {
                return profileActionInfo ??
                       (profileActionInfo = Model.CurrentProfileAction.ProfileActionInfo as ProfileActionInfoPerformanceMonitoringClass);
            }
        }        
        #endregion

        #region Public Methods
        public void Initialize()
        {
        }

        public void Refresh()
        {     
        }

        public void SetApplicationInfo()
        {
			//AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeActionEnergyBooster, DateTime.Now);

            //EventTrigger.Fire(
            //        CommandFactory.NewGameModePowerPlanCommand(
            //            ProfileActionInfo.PowerPlanId, ProfileActionInfo.PowerPlanName, ProfileActionInfo.Guid));
        }
        #endregion
    }
}
