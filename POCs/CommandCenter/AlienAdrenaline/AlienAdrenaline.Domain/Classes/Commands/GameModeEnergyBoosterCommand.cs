using System;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Commands
{
    public class GameModeEnergyBoosterCommand : EquatableCommand
    {
        public CommandType CommandType { get { return CommandType.EnergyBooster; } }
        public ProfileService ProfileService { get; set; }
        public Guid Guid { get; set; }

        public void Execute()
        {
            ProfileService.SetProfileActionInfoEnergyBooster(Guid);
        }

        public bool IsRedundant
        {
            get
            {
                var profileActionInfo = ProfileService.GetProfileActionInfo(Guid) as ProfileActionInfoEnergyBoosterClass;
                return profileActionInfo != null && profileActionInfo.Guid == Guid;
            }
        }

        public virtual bool Equals(EquatableCommand equatableCommand)
        {
            if ((equatableCommand == null) || (GetType() != equatableCommand.GetType()))
                return false;

            if (ReferenceEquals(this, equatableCommand))
                return true;

            var command = equatableCommand as GameModeEnergyBoosterCommand;
            if (command == null)
                return false;

            return CommandType == command.CommandType && Guid == command.Guid;
        }
    }
}