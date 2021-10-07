

using System.Collections.ObjectModel;
using AlienLabs.AlienAdrenaline.Domain.Classes;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface AudioDeviceService
    {
        ObservableCollection<AudioDeviceData> AudioDevices { get; set; }
        AudioDeviceData AudioDeviceSelected { get; }

        AudioDeviceData GetAudioDevice(string audioDeviceId);
        void UpdateSelectedAudioDevice(string audioDeviceId);
        void SetDefaultAudioDevice(string audioDeviceId);
        AudioDeviceData GetDefaultAudioDevice();
        void Refresh();
    }
}
