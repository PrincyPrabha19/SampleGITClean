using Dominator.Domain.Enums;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes.Commands
{
    public class AdvancedMEMOCCommand : IEquatableCommand
    {
        public CommandType CommandType => CommandType.AdvancedMEMOC;
        public IAdvancedOCModel AdvancedOCModel { get; set; }

        public int ProfileID { get; set; }

        public void Execute()
        {
            AdvancedOCModel.AddControl(HWComponentType.Memory, SettingType.XMP, ProfileID);
        }

        public bool IsRedundant
        {
            get
            {
                var currentProfileID = AdvancedOCModel.GetInitialMemoryProfileID();
                return currentProfileID == ProfileID;
            }
        }

        public virtual bool Equals(IEquatableCommand equatableCommand)
        {
            if ((equatableCommand == null) || (GetType() != equatableCommand.GetType()))
                return false;

            if (ReferenceEquals(this, equatableCommand))
                return true;

            var command = equatableCommand as AdvancedMEMOCCommand;
            return CommandType == command?.CommandType;
        }
    }
}