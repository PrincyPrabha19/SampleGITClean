using System.Collections.Generic;

namespace Dominator.Domain
{
    public interface IDataConfigurationReader
    {
        ITuningManager TuningManager { get; set; }
        void Initialize();
        string Model { get; set; }
        bool ThermalThrottling { get;  set; }
        bool PowerLimitThrottling { get;  set; }
        bool CurrentLimitThrottling { get;  set; }
        bool VoltageOffsetAlwaysOn { get; set; }
        int NumberOfFreqLevels { get; set; }
        int NumberOfCoreVoltageLevels { get; set; }
        int NumberOfVoltageOffsetLevels { get; set; }
        decimal FanMaxValue { get; set; }
        decimal FanMinValue { get; set; }
        decimal FanType { get; set; }
        List<IDataConfiguration> FrequencyConfiguration { get; }
        List<IConfiguration> CoreVoltageConfiguration { get; }
        List<IConfiguration> VoltageOffsetConfiguration { get; }
    }
}
