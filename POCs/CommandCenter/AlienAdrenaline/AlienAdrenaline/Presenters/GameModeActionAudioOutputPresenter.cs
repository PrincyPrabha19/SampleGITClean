using System;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;
using UsageTacking.Domain;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class GameModeActionAudioOutputPresenter
    {
        #region Public Properties
        public GameModeActionAudioOutputView View { get; set; }
        public GameModeActionSequenceService Model { get; set; }
        public AudioDeviceService AudioDeviceService { get; set; }        
        public EventTrigger EventTrigger { get; set; }

        private ProfileActionInfoAudioOutputClass profileActionInfo;
        public ProfileActionInfoAudioOutputClass ProfileActionInfo
        {
            get
            {
                return profileActionInfo ??
                       (profileActionInfo = Model.CurrentProfileAction.ProfileActionInfo as ProfileActionInfoAudioOutputClass);
            }
        }
        #endregion

        #region Public Methods
        public void Refresh()
        {
            if (AudioDeviceService != null)
            {
                AudioDeviceService.Refresh();

                AudioDeviceData defaultAudioDevice = AudioDeviceService.AudioDeviceSelected;
                if (!String.IsNullOrEmpty(ProfileActionInfo.AudioDeviceId))
                {
                    var audioDevice = AudioDeviceService.GetAudioDevice(ProfileActionInfo.AudioDeviceId);
                    if (audioDevice != null)
                        defaultAudioDevice = audioDevice;
                }

                View.AudioDevices = AudioDeviceService.AudioDevices;
                View.AudioDeviceSelected = defaultAudioDevice;
            }            
        }

        public void SetAudioOutputInfo()
        {
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeActionAudioOutput, DateTime.Now);

            ProfileActionInfo.AudioDeviceId = View.AudioDeviceSelected.Id;
            ProfileActionInfo.AudioDeviceName = View.AudioDeviceSelected.Name;

            EventTrigger.Fire(
                CommandFactory.NewGameModeAudioOutputCommand(
                    ProfileActionInfo.AudioDeviceId, ProfileActionInfo.AudioDeviceName, ProfileActionInfo.Guid));
        }
        #endregion
    }
}
