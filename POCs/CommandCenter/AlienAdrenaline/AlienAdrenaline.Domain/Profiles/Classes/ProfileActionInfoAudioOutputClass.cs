

using System;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileActionInfoAudioOutputClass : ProfileActionInfoProcessorClass, ProfileActionInfo
    {
        #region Public Properties
        public string AudioDeviceId { get; set; }
        public string AudioDeviceName { get; set; }
        #endregion

        #region ProfileActionInfo Members
        public Guid Guid { get; private set; }
        public int Id { get; set; }
        public int ProfileActionId { get; set; }

        public override void Execute()
        {
            var audioDeviceService = new AudioDeviceServiceClass();
            audioDeviceService.SetDefaultAudioDevice(AudioDeviceId);
        }

        public ProfileActionInfo Clone()
        {
            return new ProfileActionInfoAudioOutputClass()
            {
                Guid = Guid,
                AudioDeviceId = AudioDeviceId,
                AudioDeviceName = AudioDeviceName
            };
        }

        public bool Equals(ProfileActionInfo profileActionInfo)
        {
            return Guid == ((ProfileActionInfoAudioOutputClass)profileActionInfo).Guid &&
                   AudioDeviceId == ((ProfileActionInfoAudioOutputClass)profileActionInfo).AudioDeviceId &&
                   AudioDeviceName == ((ProfileActionInfoAudioOutputClass)profileActionInfo).AudioDeviceName;
        }

        public ProfileActionStatus GetStatus()
        {
            return ProfileActionStatus.None;
        }
        #endregion

        #region Constructors
        public ProfileActionInfoAudioOutputClass()
        {
            Guid = Guid.NewGuid();
            AudioDeviceId = String.Empty;
            AudioDeviceName = String.Empty;
        }

        public ProfileActionInfoAudioOutputClass(bool initialize) 
            : this()
        {
            if (initialize)
            {
                var audioDeviceService = new AudioDeviceServiceClass();
                audioDeviceService.Refresh();

                var defaultAudioDevice = audioDeviceService.GetDefaultAudioDevice();
                if (defaultAudioDevice == null)
                {
                    if (audioDeviceService.AudioDevices.Count > 0)
                        defaultAudioDevice = audioDeviceService.AudioDevices[0];
                }

                if (defaultAudioDevice != null)
                {
                    AudioDeviceId = defaultAudioDevice.Id;
                    AudioDeviceName = defaultAudioDevice.Name;
                }
            }                        
        }
        #endregion
    }
}
