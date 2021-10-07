using System;
using System.Collections.ObjectModel;
using System.Linq;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Commands
{
    public class GameModeProfileActionsCommand : EquatableCommand
    {
        public CommandType CommandType { get { return CommandType.ProfileActions; } }
        public GameModeProfileActions GameModeProfileActions { get; set; }
        public ProfileService ProfileService { get; set; }

        public void Execute()
        {
            ProfileService.ReplaceGameModeProfileActions(GameModeProfileActions);
        }

        public bool IsRedundant
        {
            get
            {
                
                bool areEqual = areEqualCollection(ProfileService.GameModeProfileActions.ProfileActions);
                return areEqual;
            }
        }

        private bool areEqualCollection(ObservableCollection<ProfileAction> profileActions)
        {
            bool areEqual = true;

            if (profileActions.Count != GameModeProfileActions.ProfileActions.Count)
                return false;

            foreach (var profileAction in GameModeProfileActions.ProfileActions)
            {
                areEqual = profileActions.ToList().Exists(
                    pa => pa.Guid == profileAction.Guid && pa.Type == profileAction.Type && 
                        pa.OrderNo == profileAction.OrderNo && String.Compare(pa.Name, profileAction.Name, true) == 0);

                if (!areEqual)
                    break;
            }

            return areEqual;
        }

        public virtual bool Equals(EquatableCommand equatableCommand)
        {
            if ((equatableCommand == null) || (GetType() != equatableCommand.GetType()))
                return false;

            if (ReferenceEquals(this, equatableCommand))
                return true;

            var gameModeProfileActionsCommand = equatableCommand as GameModeProfileActionsCommand;
            if (gameModeProfileActionsCommand == null)
                return false;

            return CommandType == gameModeProfileActionsCommand.CommandType;
        }
    }
}