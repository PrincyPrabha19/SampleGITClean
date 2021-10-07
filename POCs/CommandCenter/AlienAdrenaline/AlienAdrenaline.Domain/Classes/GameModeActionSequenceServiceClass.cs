using System;
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class GameModeActionSequenceServiceClass : GameModeActionSequenceService
    {
        #region GameModeActionSequenceService Members
        public ProfileService ProfileService { get; set; }
        public GameModeProfileActionImageRepository GameModeProfileActionImageRepository { get; set; }
        public GameModeProfileActions GameModeProfileActions { get; set; }
        public ProfileAction CurrentProfileAction { get; private set; }

        private byte[] currentProfileActionImage;
        public byte[] CurrentProfileActionImage
        {
            get
            {
                return currentProfileActionImage;            
            }
            set
            {
                currentProfileActionImage = value;
                if (CurrentProfileAction != null)
                    CurrentProfileAction.Image = currentProfileActionImage;
            }
        }

        public GameModeActionType CurrentProfileActionType
        {
            get
            {
                if (CurrentProfileAction != null)
                    return CurrentProfileAction.Type;
                return GameModeActionType.Summary;
            }
        }

        public void Refresh()
        {
            GameModeProfileActions = ProfileService.CloneGameModeProfileActions();
        }

        public void SetCurrentProfileAction(ProfileAction profileAction)
        {            
            CurrentProfileAction = profileAction;
            ProfileService.SetCurrentProfileAction(profileAction);
            
            CurrentProfileActionImage =
                CurrentProfileAction != null
                    ? GetProfileActionImage(CurrentProfileAction) : GetProfileActionImage(GameModeActionType.Summary);
        }

        public void UpdateCurrentProfileActionImage(string applicationPath = null)
        {
            CurrentProfileActionImage = GetProfileActionImage(CurrentProfileAction, applicationPath);
        }

        public byte[] GetProfileActionImage(ProfileAction profileAction, string applicationPath = null)
        {
            byte[] image = null;

            if (profileAction != null)
            {
                if (profileAction.ProfileActionInfo is ProfileActionInfoApplication)
                {
                    string _applicationPath =
                        applicationPath ?? ((ProfileActionInfoApplication)profileAction.ProfileActionInfo).ApplicationPath;
                    if (!String.IsNullOrEmpty(_applicationPath))
                        image = GameModeProfileActionImageRepository.GetImageFromApplicationPath(_applicationPath);
                }
                else
                {
                    image = GetProfileActionImage(profileAction.Type);
                }
            }

            return image;            
        }

        public byte[] GetProfileActionImage(GameModeActionType type)
        {
            var iconPath = EnumHelper.GetAttributeValue<DefaultIconAttributeClass, string>(type);
            if (!String.IsNullOrEmpty(iconPath))
                return GameModeProfileActionImageRepository.GetImageFromResourcePath(iconPath);

            return null;
        }

        public EquatableCommand NewGameModeProfileActionsCommand(GameModeProfileActions gameModeProfileActions)
        {
            return CommandFactory.NewGameModeProfileActionsCommand(gameModeProfileActions);
        }
        #endregion

        #region Constructors
        public GameModeActionSequenceServiceClass()
        {
        }
        #endregion
    }
}
