using Dominator.Domain.Enums;

namespace Dominator.Domain.Classes.Commands
{
    public class ProfileNameCommand: IEquatableCommand
    {
        public CommandType CommandType => CommandType.ProfileName;
        public IAdvancedOCModel AdvancedOCModel { get; set; }

        public string ProfileName { get; set; }

        public void Execute()
        {
            AdvancedOCModel.UpdateProfileName(ProfileName);
        }

        public bool IsRedundant
        {
            get
            {
                string currentProfileName = AdvancedOCModel.IsNewProfile ? "":AdvancedOCModel.GetExistingProfileName();
                return string.Equals(currentProfileName, ProfileName);
            }
        }

        public virtual bool Equals(IEquatableCommand equatableCommand)
        {
            if ((equatableCommand == null) || (GetType() != equatableCommand.GetType()))
                return false;

            if (ReferenceEquals(this, equatableCommand))
                return true;

            var command = equatableCommand as ProfileNameCommand;
            return CommandType == command?.CommandType;
        }
    }
}
