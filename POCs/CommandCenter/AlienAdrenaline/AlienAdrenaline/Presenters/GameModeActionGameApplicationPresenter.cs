using System;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;
using AlienLabs.AlienAdrenaline.Tools;
using AlienLabs.CommandCenter.Tools;
using UsageTacking.Domain;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class GameModeActionGameApplicationPresenter
    {
        #region Public Properties
        public GameModeActionGameApplicationView View { get; set; }        
        public GameModeActionSequenceService Model { get; set; }
        //public GameModeService GameModeService { get; set; }
        public EventTrigger EventTrigger { get; set; }

        private ProfileActionInfoGameApplicationClass profileActionInfo;
        public ProfileActionInfoGameApplicationClass ProfileActionInfo
        {
            get
            {
                return profileActionInfo ??
                       (profileActionInfo = Model.CurrentProfileAction.ProfileActionInfo as ProfileActionInfoGameApplicationClass);
            }
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
        }

        public void Refresh()
        {
            View.GameTitle = Model.ProfileService.Profile.GameTitle;
            View.GameImage = Model.ProfileService.Profile.GameImage;
            View.GamePath = ProfileActionInfo.ApplicationPath;
            View.GameRealPath = ProfileActionInfo.ApplicationRealPath;
        }

        public bool IsValidGameRealPath()
        {
            return FilePathHelper.IsValidPath(View.GameRealPath);
        }

        public void WriteGameDefinitionToRegistry()
        {
            if (!String.IsNullOrEmpty(View.GameRealPath))
            {
                var gameDefinitionRegistryWriter = new GameDefinitionRegistryHelper();
                gameDefinitionRegistryWriter.SetRealApplicationPath(View.GameTitle, View.GameRealPath);                
            }
        }

        public void SetApplicationInfo()
        {
            if (IsValidGameRealPath())
            {
                //AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features., DateTime.Now);

                ProfileActionInfo.ApplicationRealPath = View.GameRealPath;
                Model.ProfileService.Profile.GameRealPath = View.GameRealPath;

                //if (String.Compare(ProfileActionInfo.ApplicationPath, View.ApplicationPath, true) != 0)
                //{
                //    ProfileActionInfo.ApplicationPath = View.ApplicationPath;
                //    Model.UpdateCurrentProfileActionImage(ProfileActionInfo.ApplicationPath);
                //}

                EventTrigger.Fire(
                        CommandFactory.NewGameModeGameApplicationCommand(
                            ProfileActionInfo.ApplicationName, ProfileActionInfo.ApplicationPath, ProfileActionInfo.ApplicationRealPath, ProfileActionInfo.Guid));
            }
        }
        #endregion
    }
}
