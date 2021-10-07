using System;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Commands
{
    public class GameModeThermalCommand : EquatableCommand
    {
        public CommandType CommandType { get { return CommandType.Thermal; } }
        public ProfileService ProfileService { get; set; }
        public Guid ThermalProfileId { get; set; }
        public string ThermalProfileName { get; set; }
        public Guid Guid { get; set; }

        public void Execute()
        {
            ProfileService.SetProfileActionInfoThermal(ThermalProfileId, ThermalProfileName, Guid);
        }

        public bool IsRedundant
        {
            get
            {
                var profileActionInfo = ProfileService.GetProfileActionInfo(Guid) as ProfileActionInfoThermalClass;
                return profileActionInfo != null && profileActionInfo.Guid == Guid &&
                    profileActionInfo.ThermalProfileId == ThermalProfileId &&
                    String.Compare(profileActionInfo.ThermalProfileName, ThermalProfileName, true) == 0;
            }
        }

        public virtual bool Equals(EquatableCommand equatableCommand)
        {
            if ((equatableCommand == null) || (GetType() != equatableCommand.GetType()))
                return false;

            if (ReferenceEquals(this, equatableCommand))
                return true;

            var command = equatableCommand as GameModeThermalCommand;
            if (command == null)
                return false;

            return CommandType == command.CommandType && Guid == command.Guid;
        }
    }
}