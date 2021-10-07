using System;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Commands
{
    public class GameModeAudioOutputCommand : EquatableCommand
    {
        public CommandType CommandType { get { return CommandType.AudioOutputAction; } }
        public ProfileService ProfileService { get; set; }
        public string AudioDeviceId { get; set; }
        public string AudioDeviceName { get; set; }
        public Guid Guid { get; set; }

        public void Execute()
        {
            ProfileService.SetProfileActionInfoAudioOutput(AudioDeviceId, AudioDeviceName, Guid);
        }

        public bool IsRedundant
        {
            get
            {
                var profileActionInfo = ProfileService.GetProfileActionInfo(Guid) as ProfileActionInfoAudioOutputClass;
                return profileActionInfo != null && profileActionInfo.Guid == Guid && 
                    String.Compare(profileActionInfo.AudioDeviceId, AudioDeviceId, true) == 0 && 
                    String.Compare(profileActionInfo.AudioDeviceName, AudioDeviceName, true) == 0;
            }
        }

        public virtual bool Equals(EquatableCommand equatableCommand)
        {
            if ((equatableCommand == null) || (GetType() != equatableCommand.GetType()))
                return false;

            if (ReferenceEquals(this, equatableCommand))
                return true;

            var command = equatableCommand as GameModeAudioOutputCommand;
            if (command == null)
                return false;

            return CommandType == command.CommandType && Guid == command.Guid;
        }
    }
}