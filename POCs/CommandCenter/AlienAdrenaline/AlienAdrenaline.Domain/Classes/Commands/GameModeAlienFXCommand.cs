using System;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Commands
{
    public class GameModeAlienFXCommand : EquatableCommand
    {
        public CommandType CommandType { get { return CommandType.AlienFX; } }
        public ProfileService ProfileService { get; set; }
        public AlienFXActionType Type { get; set; }
        public string ThemeName { get; set; }
        public string ThemePath { get; set; }
        public Guid Guid { get; set; }

        public void Execute()
        {
            ProfileService.SetProfileActionInfoAlienFX(Type, ThemeName, ThemePath, Guid);
        }

        public bool IsRedundant
        {
            get
            {
                var profileActionInfo = ProfileService.GetProfileActionInfo(Guid) as ProfileActionInfoAlienFXClass;
                return profileActionInfo != null && profileActionInfo.Guid == Guid && profileActionInfo.Type == Type &&
                    String.Compare(profileActionInfo.ThemeName, ThemeName, true) == 0 &&
                    String.Compare(profileActionInfo.ThemePath, ThemePath, true) == 0;
            }
        }

        public virtual bool Equals(EquatableCommand equatableCommand)
        {
            if ((equatableCommand == null) || (GetType() != equatableCommand.GetType()))
                return false;

            if (ReferenceEquals(this, equatableCommand))
                return true;

            var command = equatableCommand as GameModeAlienFXCommand;
            if (command == null)
                return false;

            return CommandType == command.CommandType && Guid == command.Guid;
        }
    }
}