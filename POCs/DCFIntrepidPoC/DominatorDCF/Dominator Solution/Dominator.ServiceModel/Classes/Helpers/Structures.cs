using System.Collections.Generic;

namespace Dominator.ServiceModel.Classes.Helpers
{
    public class ControlValue
    {
        public uint Id { get; set; }
        public decimal Value { get; set; }
    }

    public class SettingsInfo
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal StepValue { get; set; }
        public int NoOfDecimals { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal SafeModeUpperLimit { get; set; }
        public decimal DangerModeLowerLimit { get; set; }
    }

    public class ConfiguredSettings
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Default { get; set; }
    }

    public class ControlInfo
    {
        public decimal MaxValue { get; set; }
        public decimal MinValue { get; set; }
        public decimal StepValue { get; set; }
        public int NoOfDecimals { get; set; }
        public decimal ActiveValue { get; set; }
        public decimal BootValue { get; set; }
        public bool RequiresReboot { get; set; }
        public List<decimal> SupportedValues { get; set; } 
    }
}
