using System;
using System.ComponentModel;
using System.Configuration;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;
using UsageTacking.Domain;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class GameModeActionEnergyBoosterPresenter //: ioloEnergyBooster.IAsyncResult
    {
        #region Public Properties
        public GameModeActionEnergyBoosterView View { get; set; }
        public GameModeActionSequenceService Model { get; set; }
        public EnergyBoosterService EnergyBoosterService { get; set; }
        public EventTrigger EventTrigger { get; set; }

        public bool AutoUpdateDefinitions
        {
            get { return ProfileActionInfo.AutoUpdateDefinitions; }
            set { ProfileActionInfo.AutoUpdateDefinitions = value; }
        }

        private ProfileActionInfoEnergyBoosterClass profileActionInfo;
        public ProfileActionInfoEnergyBoosterClass ProfileActionInfo
        {
            get
            {
                return profileActionInfo ??
                       (profileActionInfo = Model.CurrentProfileAction.ProfileActionInfo as ProfileActionInfoEnergyBoosterClass);
            }
        }        
        #endregion

        #region Public Methods
        public void Initialize()
        {
            EnergyBoosterService.UpdateTUDEnded += energyBoosterServiceUpdateTUDEnded;
        }

        public void Refresh()
        {     
        }

        public void UpdateDefinitions()
        {
            View.IsUpdateDefinitionsEnabled = false;
            View.UpdateDefinitionsStatus = Properties.Resources.UpdateDefStart;

            var worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerAsync();            
        }

        public void SetApplicationInfo()
        {
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeActionEnergyBooster, DateTime.Now);

            //EventTrigger.Fire(
            //        CommandFactory.NewGameModePowerPlanCommand(
            //            ProfileActionInfo.PowerPlanId, ProfileActionInfo.PowerPlanName, ProfileActionInfo.Guid));
        }

        public void OpenEnergyBoosterSiteUrl()
        {
            try
            {
                var siteUrl = Properties.Resources.EnergyBoosterSiteUrl;
                if (String.IsNullOrEmpty(siteUrl))
                    return;

                siteUrl = Uri.EscapeUriString(siteUrl);
                if (!String.IsNullOrEmpty(siteUrl))
                    System.Diagnostics.Process.Start(siteUrl);
            }
            catch { }
        }

        //public void Complete(EnergyBoosterError errorCode)
        //{
        //    View.UpdateDefinitionsStatus = errorCodeToString(errorCode);
        //    View.IsUpdateDefinitionsEnabled = true;
        //    AutoUpdateDefinitions = false;
        //}
        #endregion

        #region Private Methods
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            EnergyBoosterService.UpdateTUD();
        }

        private void energyBoosterServiceUpdateTUDEnded(EnergyBoosterError error)
        {
            View.UpdateDefinitionsStatus = errorCodeToString(error);
            View.IsUpdateDefinitionsEnabled = true;
            AutoUpdateDefinitions = false;
        }

        private string errorCodeToString(EnergyBoosterError error)
        {
            switch (error)
            {
                case EnergyBoosterError.Er_OK: return Properties.Resources.UpdateDefOK;
                case EnergyBoosterError.Er_Fail: return Properties.Resources.UpdateDefFail;
                case EnergyBoosterError.Er_NoConnect: return Properties.Resources.UpdateDefNoConnect;
                case EnergyBoosterError.Er_UpToDate: return Properties.Resources.UpdateDefUpToDate;
                case EnergyBoosterError.Er_TimeOut: return Properties.Resources.UpdateDefTimeout;
                default: return Properties.Resources.UpdateDefFail;
            }
        }
        #endregion
    }
}
