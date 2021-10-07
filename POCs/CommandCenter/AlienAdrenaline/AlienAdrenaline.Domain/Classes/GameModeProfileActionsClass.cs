
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class GameModeProfileActionsClass : GameModeProfileActions
    {
        #region GameModeProfileActions Members
        public ObservableCollection<ProfileAction> ProfileActions { get; set; }

        public void AddProfileAction(ProfileAction profileAction)
        {
            int index = -1;
            if (ProfileActions.Count > 0)
            {
                index = ProfileActions.Count - 1;
                while (index >= 0)
                {
                    if (EnumHelper.GetAttributeValue<AllowRelocationAttributeClass, bool>(ProfileActions[index].Type))
                        break;

                    index--;
                }
            }

            ProfileActions.Insert(index + 1, profileAction);
            reOrderProfileActions();
        }

        public void DeleteProfileAction(ProfileAction profileAction)
        {
            if (ProfileActions != null)
            {
                ProfileActions.Remove(profileAction);
                reOrderProfileActions();
            }                
        }

        public void RelocateProfileAction(ProfileAction profileAction, bool forward)
        {
            int index = ProfileActions.IndexOf(profileAction);
            if (index != -1)
            {
                ProfileActions.Move(index, (forward) ? index + 1 : index - 1);
                reOrderProfileActions();
            }
        }

        public GameModeProfileActions Clone()
        {
            var gameModeProfileActions = new GameModeProfileActionsClass();

            foreach (var profileAction in ProfileActions)
            {
                var newProfileAction = new ProfileActionClass()
                {
                    Guid = profileAction.Guid,
                    Name = profileAction.Name,
                    Type = profileAction.Type,
                    OrderNo = profileAction.OrderNo,
                    ProfileActionInfo = profileAction.ProfileActionInfo.Clone()
                };

                gameModeProfileActions.ProfileActions.Add(newProfileAction);
            }

            return gameModeProfileActions;
        }

        public void ReplaceProfileActions(GameModeProfileActions gameModeProfileActions)
        {
            ProfileActions.Clear();

            foreach (var profileAction in gameModeProfileActions.ProfileActions)
            {
                var newProfileAction = new ProfileActionClass()
                {
                    Guid = profileAction.Guid,
                    Name = profileAction.Name,
                    Type = profileAction.Type,
                    OrderNo = profileAction.OrderNo,
                    ProfileActionInfo = profileAction.ProfileActionInfo.Clone()
                };

                ProfileActions.Add(newProfileAction);
            }
        }
        #endregion

        #region IEnumerable Members
        IEnumerator<ProfileAction> IEnumerable<ProfileAction>.GetEnumerator()
        {
            return ProfileActions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ProfileActions.GetEnumerator();
        }
        #endregion

        #region Public Properties
        public ProfileAction this[int index]
        {
            get
            {
                if (index >= 0 && index < ProfileActions.Count)
                    return ProfileActions[index];
                return null;
            }
        }
        #endregion

        #region Constructors
        public GameModeProfileActionsClass()
        {
            ProfileActions = new ObservableCollection<ProfileAction>();
        }
        #endregion

        #region Private Methods
        private void reOrderProfileActions()
        {
            int i = 0;
            foreach (var profileAction in ProfileActions)
                profileAction.OrderNo = i++;
        }
        #endregion
    }
}
