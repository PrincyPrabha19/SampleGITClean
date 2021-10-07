

using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;

namespace AlienLabs.AlienAdrenaline.Domain.Enums
{
    public enum AlienFXActionType
    {
        [ResourceKeyAttributeClass("UseCurrentAlienFXStateText")]
        None,

        [ResourceKeyAttributeClass("EnableAlienFXAPIText")]
        EnableAlienFXAPI,

        [ResourceKeyAttributeClass("PlayThemeHeaderText")]
        PlayTheme,

        [ResourceKeyAttributeClass("GoDarkText")]
        GoDark
    }
}
