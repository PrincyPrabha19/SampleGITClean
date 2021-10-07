
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using AlienLabs.AlienAdrenaline.AudioAPI.Classes;
using AlienLabs.AlienAdrenaline.AudioAPI.Enums;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.WindowsIconHelper;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class AudioDeviceServiceClass : AudioDeviceService
    {
        #region Private Properties
        private MMDeviceEnumerator deviceEnumerator;
        private PolicyConfigVista policyConfigVista;
        #endregion

        #region AudioDeviceService Members
        public ObservableCollection<AudioDeviceData> AudioDevices { get; set; }

        public AudioDeviceData AudioDeviceSelected
        {
            get
            {
                if (AudioDevices != null)
                    return (from device in AudioDevices
                            where device.IsSelected
                            select device).FirstOrDefault();

                return null;
            }
        }

        public AudioDeviceData GetAudioDevice(string audioDeviceId)
        {
            return (from ad in AudioDevices
                    where ad.Id == audioDeviceId
                    select ad).FirstOrDefault();
        }

        public void UpdateSelectedAudioDevice(string audioDeviceId)
        {
            foreach (var device in AudioDevices)
                device.IsSelected = device.Id == audioDeviceId;
        }

        public void SetDefaultAudioDevice(string audioDeviceId)
        {
            try
            {
                policyConfigVista.SetDefaultEndPoint(audioDeviceId, ERole.eMultimedia);
            }
            catch (ArgumentException e)
            {
                throw new Exception(Properties.Resources.SoundDeviceErrorText);
            }            
        }

        public AudioDeviceData GetDefaultAudioDevice()
        {
            try
            {
                var defaultDevice = deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
                if (defaultDevice != null)
                {
                    return new AudioDeviceData()
                    {
                        Id = defaultDevice.ID,
                        Name = defaultDevice.FriendlyName,
                        State = getDeviceState(defaultDevice.State)
                    };
                }
            }
            catch (ArgumentException e)
            {
                throw new Exception(Properties.Resources.SoundDeviceErrorText);
            }

            return null;
        }

        public void Refresh()
        {
            if (AudioDevices == null)
                initializeAudioDeviceList();
        }
        #endregion

        #region Constructors
        public AudioDeviceServiceClass()
        {
            deviceEnumerator = new MMDeviceEnumerator();
            policyConfigVista = new PolicyConfigVista();
        }
        #endregion

        #region Private Members
        private void initializeAudioDeviceList()
        {
            AudioDevices = new ObservableCollection<AudioDeviceData>();

            var defaultIconPath = String.Empty;
            var defaultDevice = deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
            //if (defaultDevice != null)
            //{
            //    string[] parts = defaultDevice.IconPath.Split(',');
            //    defaultIconPath = parts[0];
            //}

            var deviceCollection = deviceEnumerator.EnumerateAudioEndPoints(
                EDataFlow.eRender, EDeviceState.DEVICE_STATE_ACTIVE | EDeviceState.DEVICE_STATE_UNPLUGGED);
            if (deviceCollection != null && deviceCollection.Count > 0)
            {
                string[] _parts = deviceCollection[0].IconPath.Split(',');
                defaultIconPath = _parts[0];

                if (String.IsNullOrEmpty(defaultIconPath)) return;
                using (var iconExtractor = new IconExtractor(defaultIconPath))
                {
                    //var deviceCollection = deviceEnumerator.EnumerateAudioEndPoints(
                    //    EDataFlow.eRender, EDeviceState.DEVICE_STATE_ACTIVE | EDeviceState.DEVICE_STATE_UNPLUGGED);
                    for (int i = 0; i < deviceCollection.Count; i++)
                    {
                        var mmDevice = deviceCollection[i];
                        string[] parts = mmDevice.IconPath.Split(',');
                        var iconId = Math.Abs(Convert.ToInt32(parts[1]));

                        Icon icon = iconExtractor.GetIconById(iconId);
                        icon = IconHelper.GetBestFitIcon(icon, new Size(48, 48));

                        var device = new AudioDeviceData()
                        {
                            Id = mmDevice.ID,
                            Name = mmDevice.FriendlyName,
                            Image = IconUtils.GetBytesFromIcon(icon),
                            State = getDeviceState(mmDevice.State),
                            IsSelected = defaultDevice != null && defaultDevice.ID == mmDevice.ID,
                            IsEnabled = mmDevice.State == EDeviceState.DEVICE_STATE_ACTIVE
                        };

                        AudioDevices.Add(device);
                    }
                }
            }          
        }

        private SoundDeviceState getDeviceState(EDeviceState eDeviceState)
        {
            return (SoundDeviceState) eDeviceState;
        }
        #endregion
    }
}
