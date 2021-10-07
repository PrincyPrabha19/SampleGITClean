using System;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;
using AlienLabs.AlienAdrenaline.Tools;
using UsageTacking.Domain;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class GameModeActionApplicationPresenter
    {
        #region Public Properties
        public GameModeActionApplicationView View { get; set; }
        public GameModeActionSequenceService Model { get; set; }                
        public EventTrigger EventTrigger { get; set; }

        private ProfileActionInfoApplicationClass profileActionInfo;
        public ProfileActionInfoApplicationClass ProfileActionInfo
        {
            get 
            {
                return profileActionInfo ??
                       (profileActionInfo = Model.CurrentProfileAction.ProfileActionInfo as ProfileActionInfoApplicationClass);
            }
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
        }

        public void Refresh()
        {           
            View.ApplicationName = ProfileActionInfo.ApplicationName;
            View.ApplicationPath = ProfileActionInfo.ApplicationPath;
        }

        public bool IsValidApplicationPath()
        {
            return FilePathHelper.IsValidPath(View.ApplicationPath);
        }

        public void SetApplicationInfo()
        {
            if (IsValidApplicationPath())
            {
				AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeActionApplication, DateTime.Now);

                View.ApplicationName = FilePathHelper.GetFileDescription(View.ApplicationPath);
                ProfileActionInfo.ApplicationName = View.ApplicationName;

                if (String.Compare(ProfileActionInfo.ApplicationPath, View.ApplicationPath, StringComparison.OrdinalIgnoreCase) != 0)
                {                    
                    ProfileActionInfo.ApplicationPath = View.ApplicationPath;
                    Model.UpdateCurrentProfileActionImage(ProfileActionInfo.ApplicationPath);
                }                                

                EventTrigger.Fire(
                        CommandFactory.NewGameModeApplicationCommand(
                            ProfileActionInfo.ApplicationName, ProfileActionInfo.ApplicationPath, ProfileActionInfo.Guid));
            }
        }
        #endregion
    }
}
