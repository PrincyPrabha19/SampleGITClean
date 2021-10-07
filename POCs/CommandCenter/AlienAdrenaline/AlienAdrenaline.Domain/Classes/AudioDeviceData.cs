

using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class AudioDeviceData
    {
        #region Properties
        public string Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public SoundDeviceState State { get; set; }
        public bool IsSelected { get; set; }
        public bool IsEnabled { get; set; }
        #endregion
    }
}
