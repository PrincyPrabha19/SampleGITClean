using System;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;
using AlienLabs.AlienAdrenaline.Tools;
using UsageTacking.Domain;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class GameModeActionAdditionalApplicationPresenter
    {
        #region Public Properties
        public GameModeActionAdditionalApplicationView View { get; set; }
        public GameModeActionSequenceService Model { get; set; }
        public EventTrigger EventTrigger { get; set; }

        private ProfileActionInfoAdditionalApplicationClass profileActionInfo;
        public ProfileActionInfoAdditionalApplicationClass ProfileActionInfo
        {
            get
            {
                return profileActionInfo ??
                       (profileActionInfo = Model.CurrentProfileAction.ProfileActionInfo as ProfileActionInfoAdditionalApplicationClass);
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
            View.LaunchIfNotOpen = ProfileActionInfo.LaunchIfNotOpen;
            View.ExitIfOpen = !ProfileActionInfo.LaunchIfNotOpen;
        }

        public bool IsValidApplicationPath()
        {
            return FilePathHelper.IsValidPath(View.ApplicationPath);
        }

        public void SetApplicationInfo()
        {
            if (IsValidApplicationPath())
            {
				AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeActionAdditionalApplication, DateTime.Now);

                View.ApplicationName = FilePathHelper.GetFileDescription(View.ApplicationPath);

                ProfileActionInfo.ApplicationName = View.ApplicationName;
                ProfileActionInfo.LaunchIfNotOpen = View.LaunchIfNotOpen;

                if (String.Compare(ProfileActionInfo.ApplicationPath, View.ApplicationPath, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    ProfileActionInfo.ApplicationPath = View.ApplicationPath;
                    Model.UpdateCurrentProfileActionImage(ProfileActionInfo.ApplicationPath);
                }     

                EventTrigger.Fire(
                        CommandFactory.NewGameModeAdditionalApplicationCommand(
                            ProfileActionInfo.ApplicationName, ProfileActionInfo.ApplicationPath, ProfileActionInfo.LaunchIfNotOpen, ProfileActionInfo.Guid));
            }
        }
        #endregion
    }
}
