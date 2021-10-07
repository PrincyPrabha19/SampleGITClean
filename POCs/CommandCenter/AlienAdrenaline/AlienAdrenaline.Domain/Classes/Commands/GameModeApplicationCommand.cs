using System;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Commands
{
    public class GameModeApplicationCommand : EquatableCommand
    {
        public CommandType CommandType { get { return CommandType.ApplicationAction; } }
        public ProfileService ProfileService { get; set; }        
        
        public string ApplicationName { get; set; }
        public string ApplicationPath { get; set; }
        public Guid Guid { get; set; }

        public void Execute()
        {
            ProfileService.SetProfileActionInfoApplication(ApplicationName, ApplicationPath, Guid);
        }

        public bool IsRedundant
        {
            get
            {
                var profileActionInfo = ProfileService.GetProfileActionInfo(Guid) as ProfileActionInfoApplicationClass;
                    return profileActionInfo != null && profileActionInfo.Guid == Guid &&
                        String.Compare(profileActionInfo.ApplicationName, ApplicationName, StringComparison.OrdinalIgnoreCase) == 0 &&
                        String.Compare(profileActionInfo.ApplicationPath, ApplicationPath, StringComparison.OrdinalIgnoreCase) == 0;
            }
        }

        public virtual bool Equals(EquatableCommand equatableCommand)
        {
            if ((equatableCommand == null) || (GetType() != equatableCommand.GetType()))
                return false;

            if (ReferenceEquals(this, equatableCommand))
                return true;

            var command = equatableCommand as GameModeApplicationCommand;
            if (command == null)
                return false;

            return CommandType == command.CommandType && Guid == command.Guid;
        }
    }
}