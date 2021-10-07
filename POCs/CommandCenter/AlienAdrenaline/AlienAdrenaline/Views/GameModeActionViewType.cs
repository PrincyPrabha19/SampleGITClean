
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public enum GameModeActionViewType
    {
        [AllowCachingAttributeClass(false)]
        [AllowMultipleAttributeClass(false)]
        [DefaultIconAttributeClass("")]
        Summary,

        [AllowCachingAttributeClass(true)]
        [AllowMultipleAttributeClass(false)]
        [DefaultIconAttributeClass("")]
        GameApplication,

        [AllowCachingAttributeClass(true)]
        [AllowMultipleAttributeClass(false)]
        [DefaultIconAttributeClass("")]
        MediaPlayerApplication,

        [AllowCachingAttributeClass(false)]
        [AllowMultipleAttributeClass(false)]
        [DefaultIconAttributeClass("")]
        FrapsApplication,

        [AllowCachingAttributeClass(true)]
        [AllowMultipleAttributeClass(false)]
        [DefaultIconAttributeClass("")]
        VoIPApplication,

        [AllowCachingAttributeClass(true)]
        [AllowMultipleAttributeClass(true)]
        [DefaultIconAttributeClass("")]
        AdditionalApplication,

        [AllowCachingAttributeClass(true)]
        [AllowMultipleAttributeClass(false)]
        [DefaultIconAttributeClass("")]
        AudioOutput,

        [AllowCachingAttributeClass(true)]
        [AllowMultipleAttributeClass(false)]
        [DefaultIconAttributeClass("")]
        WebLinks,

        [AllowCachingAttributeClass(true)]
        [AllowMultipleAttributeClass(false)]
        [DefaultIconAttributeClass("")]
        PowerPlan,

        [AllowCachingAttributeClass(true)]
        [AllowMultipleAttributeClass(false)]
        [DefaultIconAttributeClass("")]
        Thermal,

        [AllowCachingAttributeClass(true)]
        [AllowMultipleAttributeClass(false)]
        [DefaultIconAttributeClass("")]
        AlienFX,

        [AllowCachingAttributeClass(true)]
        [AllowMultipleAttributeClass(false)]
        [DefaultIconAttributeClass("")]
        EnergyBooster,

        [AllowCachingAttributeClass(true)]
        [AllowMultipleAttributeClass(false)]
        [DefaultIconAttributeClass("")]
        PerformanceMonitoring
    }
}