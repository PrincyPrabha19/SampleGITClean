using System;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Commands
{
    public class GameModeAdditionalApplicationCommand : EquatableCommand
    {
        public CommandType CommandType { get { return CommandType.AdditionalApplicationAction; } }
        public ProfileService ProfileService { get; set; }
        
        public string ApplicationName { get; set; }
        public string ApplicationPath { get; set; }
        public bool LaunchIfNotOpen { get; set; }
        public Guid Guid { get; set; }

        public void Execute()
        {
            ProfileService.SetProfileActionInfoAdditionalApplication(ApplicationName, ApplicationPath, LaunchIfNotOpen, Guid);
        }

        public bool IsRedundant
        {
            get
            {
                var profileActionInfo = ProfileService.GetProfileActionInfo(Guid) as ProfileActionInfoAdditionalApplicationClass;
                return profileActionInfo != null && profileActionInfo.Guid == Guid && 
                    String.Compare(profileActionInfo.ApplicationName, ApplicationName, true) == 0 && 
                    String.Compare(profileActionInfo.ApplicationPath, ApplicationPath, true) == 0 &&
                    profileActionInfo.LaunchIfNotOpen == LaunchIfNotOpen;
            }
        }

        public virtual bool Equals(EquatableCommand equatableCommand)
        {
            if ((equatableCommand == null) || (GetType() != equatableCommand.GetType()))
                return false;

            if (ReferenceEquals(this, equatableCommand))
                return true;

            var command = equatableCommand as GameModeAdditionalApplicationCommand;
            if (command == null)
                return false;

            return CommandType == command.CommandType && Guid == command.Guid;
        }
    }
}