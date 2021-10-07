
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;

namespace AlienLabs.AlienAdrenaline.Domain.Enums
{
    public enum GameModeActionType
    {
        [AllowMultipleAttributeClass(false)]
        [AllowCreationAttributeClass(false), InitializeOnCreationAttributeClass(false)]
        [AllowDeletionAttributeClass(false)]
        [AllowRelocationAttributeClass(false)]
        [AddOnProfileCreationAttributeClass(false)]
        [AllowRollbackAttributeClass(false)]
        [RequireThermalCapabilitiesAttributeClass(false)]
        [ResourceKeyAttributeClass("ActionSequenceSummaryText")]
        [DefaultIconAttributeClass("/Media/Actions/summary.png")]
        Summary,

        [AllowMultipleAttributeClass(false)]
        [AllowCreationAttributeClass(false), InitializeOnCreationAttributeClass(false)]
        [AllowDeletionAttributeClass(true)]
        [AllowRelocationAttributeClass(true)]
        [AddOnProfileCreationAttributeClass(false)]
        [AllowRollbackAttributeClass(false)]
        [RequireThermalCapabilitiesAttributeClass(false)]
        [ResourceKeyAttributeClass("")]
        [DefaultIconAttributeClass("")]
        Application,

        [AllowMultipleAttributeClass(false)]
        [AllowCreationAttributeClass(false), InitializeOnCreationAttributeClass(false)]
        [AllowDeletionAttributeClass(false)]
        [AllowRelocationAttributeClass(false)]
        [AddOnProfileCreationAttributeClass(false)]
        [AllowRollbackAttributeClass(false)]
        [RequireThermalCapabilitiesAttributeClass(false)]
        [ResourceKeyAttributeClass("LaunchGameText")]
        [DefaultIconAttributeClass("")]
        GameApplication,

        [AllowMultipleAttributeClass(false)]
        [AllowCreationAttributeClass(true), InitializeOnCreationAttributeClass(false)]
        [AllowDeletionAttributeClass(true)]
        [AllowRelocationAttributeClass(true)]
        [AddOnProfileCreationAttributeClass(true)]
        [AllowRollbackAttributeClass(false)]
        [RequireThermalCapabilitiesAttributeClass(false)]
        [ResourceKeyAttributeClass("MediaPlayerText")]
        [DefaultIconAttributeClass("")]
        MediaPlayerApplication,
        
        [AllowMultipleAttributeClass(false)]
        [AllowCreationAttributeClass(true), InitializeOnCreationAttributeClass(false)]
        [AllowDeletionAttributeClass(true)]
        [AllowRelocationAttributeClass(true)]
        [AddOnProfileCreationAttributeClass(true)]
        [AllowRollbackAttributeClass(false)]
        [RequireThermalCapabilitiesAttributeClass(false)]
        [ResourceKeyAttributeClass("FrapsText")]
        [DefaultIconAttributeClass("")]
        FrapsApplication,

        [AllowMultipleAttributeClass(false)]
        [AllowCreationAttributeClass(true), InitializeOnCreationAttributeClass(false)]
        [AllowDeletionAttributeClass(true)]
        [AllowRelocationAttributeClass(true)]
        [AddOnProfileCreationAttributeClass(true)]
        [AllowRollbackAttributeClass(false)]
        [RequireThermalCapabilitiesAttributeClass(false)]
        [ResourceKeyAttributeClass("VoIPText")]
        [DefaultIconAttributeClass("")]
        VoIPApplication,

        [AllowMultipleAttributeClass(true)]
        [AllowCreationAttributeClass(true), InitializeOnCreationAttributeClass(false)]
        [AllowDeletionAttributeClass(true)]
        [AllowRelocationAttributeClass(true)]
        [AddOnProfileCreationAttributeClass(true)]
        [AllowRollbackAttributeClass(false)]
        [RequireThermalCapabilitiesAttributeClass(false)]
        [ResourceKeyAttributeClass("AdditionalAppsText")]
        [DefaultIconAttributeClass("")]
        AdditionalApplication,

        [AllowMultipleAttributeClass(false)]
        [AllowCreationAttributeClass(true), InitializeOnCreationAttributeClass(true)]
        [AllowDeletionAttributeClass(true)]
        [AllowRelocationAttributeClass(true)]
        [AddOnProfileCreationAttributeClass(false)]
        [AllowRollbackAttributeClass(true)]
        [RequireThermalCapabilitiesAttributeClass(false)]
        [ResourceKeyAttributeClass("AudioOutputText")]
        [DefaultIconAttributeClass("/Media/Actions/audio_output.png")]
        AudioOutput,

        [AllowMultipleAttributeClass(false)]
        [AllowCreationAttributeClass(true), InitializeOnCreationAttributeClass(false)]
        [AllowDeletionAttributeClass(true)]
        [AllowRelocationAttributeClass(true)]
        [AddOnProfileCreationAttributeClass(true)]
        [AllowRollbackAttributeClass(false)]
        [RequireThermalCapabilitiesAttributeClass(false)]
        [ResourceKeyAttributeClass("WebLinksText")]
        [DefaultIconAttributeClass("/Media/Actions/web_links.png")]
        WebLinks,

        [AllowMultipleAttributeClass(false)]
        [AllowCreationAttributeClass(true), InitializeOnCreationAttributeClass(false)]
        [AllowDeletionAttributeClass(true)]
        [AllowRelocationAttributeClass(true)]
        [AddOnProfileCreationAttributeClass(false)]
        [AllowRollbackAttributeClass(true)]
        [RequireThermalCapabilitiesAttributeClass(false)]
        [ResourceKeyAttributeClass("PowerPlanText")]
        [DefaultIconAttributeClass("/Media/Actions/power_plan.ico")]
        PowerPlan,

        [AllowMultipleAttributeClass(false)]
        [AllowCreationAttributeClass(true), InitializeOnCreationAttributeClass(false)]
        [AllowDeletionAttributeClass(true)]
        [AllowRelocationAttributeClass(true)]
        [AddOnProfileCreationAttributeClass(false)]
        [AllowRollbackAttributeClass(true)]
        [RequireThermalCapabilitiesAttributeClass(true)]
        [ResourceKeyAttributeClass("ThermalText")]
        [DefaultIconAttributeClass("/Media/Actions/thermal.ico")]
        Thermal,

        [AllowMultipleAttributeClass(false)]
        [AllowCreationAttributeClass(true), InitializeOnCreationAttributeClass(false)]
        [AllowDeletionAttributeClass(true)]
        [AllowRelocationAttributeClass(true)]
        [AddOnProfileCreationAttributeClass(true)]
        [AllowRollbackAttributeClass(true)]
        [RequireThermalCapabilitiesAttributeClass(false)]
        [ResourceKeyAttributeClass("AlienFXText")]
        [DefaultIconAttributeClass("/Media/Actions/alienfx.ico")]
        AlienFX,

        [AllowMultipleAttributeClass(false)]
        [AllowCreationAttributeClass(true), InitializeOnCreationAttributeClass(false)]
        [AllowDeletionAttributeClass(true)]
        [AllowRelocationAttributeClass(true)]
        [AddOnProfileCreationAttributeClass(true)]
        [AllowRollbackAttributeClass(true)]
        [RequireThermalCapabilitiesAttributeClass(false)]
        [ResourceKeyAttributeClass("EnergyBoosterText")]
        [DefaultIconAttributeClass("/Media/Actions/energy_booster.png")]
        EnergyBooster,

        [AllowMultipleAttributeClass(false)]
        [AllowCreationAttributeClass(true), InitializeOnCreationAttributeClass(false)]
        [AllowDeletionAttributeClass(true)]
        [AllowRelocationAttributeClass(true)]
        [AddOnProfileCreationAttributeClass(false)]
        [AllowRollbackAttributeClass(true)]
        [RequireThermalCapabilitiesAttributeClass(false)]
        [ResourceKeyAttributeClass("PerformanceMonitoringText")]
        [DefaultIconAttributeClass("/Media/Actions/performance_monitoring.png")]
        PerformanceMonitoring
    }
}
