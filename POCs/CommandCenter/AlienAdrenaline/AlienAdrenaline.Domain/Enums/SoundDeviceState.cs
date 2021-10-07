
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;

namespace AlienLabs.AlienAdrenaline.Domain.Enums
{
    public enum SoundDeviceState : uint
    {
        [ResourceKeyAttributeClass("AudioDeviceReadyText")]
        Ready = 1,

        [ResourceKeyAttributeClass("AudioDeviceDisabledText")]
        Disabled = 2,

        [ResourceKeyAttributeClass("AudioDeviceNotPresentText")]
        NotPresent = 4,

        [ResourceKeyAttributeClass("AudioDeviceUnpluggedText")]
        Unplugged = 8,
    }
}