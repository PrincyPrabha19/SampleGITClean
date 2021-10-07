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
    public class GameModeActionThermalPresenter
    {
        #region Public Properties
        public GameModeActionThermalView View { get; set; }
        public GameModeActionSequenceService Model { get; set; }
        public ThermalProfileService ThermalProfileService { get; set; }
        public EventTrigger EventTrigger { get; set; }

        private ProfileActionInfoThermalClass profileActionInfo;
        public ProfileActionInfoThermalClass ProfileActionInfo
        {
            get
            {
                return profileActionInfo ??
                       (profileActionInfo = Model.CurrentProfileAction.ProfileActionInfo as ProfileActionInfoThermalClass);
            }
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
        }

        public void Refresh()
        {
            var thermalProfiles = ThermalProfileService.GetAllThermalProfiles();

            ThermalProfile thermalProfileSelected = null;
            Guid activeThermalProfileId = ThermalProfileService.GetActiveThermalProfile();
            ThermalProfile activeThermalProfile = thermalProfiles.ToList().Find(pp => pp.Id == activeThermalProfileId);
            if (activeThermalProfile != null)
            {
                thermalProfileSelected = new ThermalProfileClass()
                {
                    Id = activeThermalProfileId,
                    Name = activeThermalProfile.Name, 
                    TempName = Properties.Resources.ActiveThermalProfileText
                };
                thermalProfiles.Insert(0, thermalProfileSelected);
            }

            Guid profileActionInfoThermalId = ProfileActionInfo.ThermalProfileId;
            ThermalProfile profileActionInfoThermal = thermalProfiles.ToList().Find(pp => pp.Id == profileActionInfoThermalId);
            string profileActionInfoPowerPlanName = ProfileActionInfo.ThermalProfileName;

            View.ThermalProfiles = thermalProfiles;
            View.ThermalProfileSelected =
                profileActionInfoThermalId != Guid.Empty && profileActionInfoThermalId != activeThermalProfileId && profileActionInfoThermal != null ?
                    profileActionInfoThermal : thermalProfileSelected;

            if (profileActionInfoThermalId != Guid.Empty && profileActionInfoThermal == null)
                View.ShowThermalProfileErrorMessage(true, profileActionInfoPowerPlanName);
        }

        public void SetApplicationInfo()
        {
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeActionThermal, DateTime.Now);

            View.ShowThermalProfileErrorMessage(false);

            ProfileActionInfo.ThermalProfileId = View.ThermalProfileSelected.Id;
            ProfileActionInfo.ThermalProfileName = View.ThermalProfileSelected.Name;

            EventTrigger.Fire(
                    CommandFactory.NewGameModeThermalCommand(
                        ProfileActionInfo.ThermalProfileId, ProfileActionInfo.ThermalProfileName, ProfileActionInfo.Guid));
        }
        #endregion
    }
}
