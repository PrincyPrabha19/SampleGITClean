using System;
using System.Collections.Generic;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using UsageTacking.Domain;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class GameModeActionSequencePresenter
    {
        #region Public Properties
        public GameModeActionSequenceView View { get; set; }
        public GameModeActionSequenceService Model { get; set; }
        public GameModeActionViewRepository ViewRepository { get; set; }
        public EventTrigger EventTrigger { get; set; }        
        #endregion

        #region Public Methods
        public void Initialize()
        {
        }

        public void Refresh()
        {
            ViewRepository.ClearAllContentViews();

            Model.Refresh();
            updateProfileActionNameTranslations(Model.GameModeProfileActions);
            View.GameModeProfileActions = Model.GameModeProfileActions;
            View.SetActionSequenceItemsSource();            

            SelectProfileAction(null);
        }

        public void Activate(GameModeActionViewType type, Guid id)
        {
            var view = ViewRepository.GetByType(type, id) as GameModeActionContentView;
            if (view != null)
            {
                View.ActionDetailsView = view;
                View.ShowSummaryLink(type != GameModeActionViewType.Summary);
            }                
        }

        public void AddProfileAction(ProfileAction profileAction)
        {
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeAddProfileAction, DateTime.Now);
            View.GameModeProfileActions.AddProfileAction(profileAction);

            EventTrigger.Fire(
                Model.NewGameModeProfileActionsCommand(View.GameModeProfileActions));

            EventTrigger.Fire(
                CommandFactory.NewGameModeProfileActionInfoCommand(profileAction.ProfileActionInfo, profileAction.Type));
        }

        public void DeleteProfileAction(ProfileAction profileAction)
        {
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeDeleteProfileAction, DateTime.Now);
            View.GameModeProfileActions.DeleteProfileAction(profileAction);

            SelectProfileAction(null);

            EventTrigger.Fire(
                CommandFactory.NewGameModeProfileActionInfoCommand(profileAction.ProfileActionInfo, profileAction.Type));

            EventTrigger.Fire(
                Model.NewGameModeProfileActionsCommand(View.GameModeProfileActions));
        }

        public void RelocateProfileAction(ProfileAction profileAction, bool forward)
        {
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeRelocateProfileAction, DateTime.Now);
            View.GameModeProfileActions.RelocateProfileAction(profileAction, forward);

            EventTrigger.Fire(
                Model.NewGameModeProfileActionsCommand(View.GameModeProfileActions));
        }

        public void SelectProfileAction(ProfileAction profileAction)
        {
            Model.SetCurrentProfileAction(profileAction);
            View.CurrentProfileActionImage = Model.CurrentProfileActionImage;
            View.CurrentProfileActionTitle = getProfileActionTitle (Model.CurrentProfileActionType);

            var gameModeActionViewType = GameModeActionViewType.Summary;
            if (profileAction == null || Enum.TryParse(profileAction.Type.ToString(), true, out gameModeActionViewType))
                Activate(gameModeActionViewType, profileAction != null ? profileAction.Guid : Guid.Empty);                       
        }

        public void RefreshProfileActionNameAndImage()
        {
            View.CurrentProfileActionImage = Model.CurrentProfileActionImage;
        }

        public void RefreshGameApplicationAction()
        {
            //throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        private void updateProfileActionNameTranslations(IEnumerable<ProfileAction> gameModeProfileActions)
        {
            foreach (var profileAction in gameModeProfileActions)
            {
                string resourceKey = EnumHelper.GetAttributeValue<ResourceKeyAttributeClass, string>(profileAction.Type);
                if (!String.IsNullOrEmpty(resourceKey))
                    profileAction.Name = Properties.Resources.ResourceManager.GetString(resourceKey);
            }
        }

        private string getProfileActionTitle(GameModeActionType type)
        {
            string resourceValue = string.Empty;
            string resourceKey = EnumHelper.GetAttributeValue<ResourceKeyAttributeClass, string>(type);
            if (!String.IsNullOrEmpty(resourceKey))
                resourceValue = Properties.Resources.ResourceManager.GetString(resourceKey);

            if (type != GameModeActionType.Summary)
                resourceValue = String.Format(Properties.Resources.ActionDetailsText, resourceValue);

            return resourceValue;
        }
        #endregion
    }
}
