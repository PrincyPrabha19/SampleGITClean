using System;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Commands
{
    public class GameModePowerPlanCommand : EquatableCommand
    {
        public CommandType CommandType { get { return CommandType.PowerPlan; } }
        public ProfileService ProfileService { get; set; }
        public Guid PowerPlanId { get; set; }
        public string PowerPlanName { get; set; }
        public Guid Guid { get; set; }

        public void Execute()
        {
            ProfileService.SetProfileActionInfoPowerPlan(PowerPlanId, PowerPlanName, Guid);
        }

        public bool IsRedundant
        {
            get
            {
                var profileActionInfo = ProfileService.GetProfileActionInfo(Guid) as ProfileActionInfoPowerPlanClass;
                return profileActionInfo != null && profileActionInfo.Guid == Guid && 
                    profileActionInfo.PowerPlanId == PowerPlanId && 
                    String.Compare(profileActionInfo.PowerPlanName, PowerPlanName, true) == 0;
            }
        }

        public virtual bool Equals(EquatableCommand equatableCommand)
        {
            if ((equatableCommand == null) || (GetType() != equatableCommand.GetType()))
                return false;

            if (ReferenceEquals(this, equatableCommand))
                return true;

            var command = equatableCommand as GameModePowerPlanCommand;
            if (command == null)
                return false;

            return CommandType == command.CommandType && Guid == command.Guid;
        }
    }
}