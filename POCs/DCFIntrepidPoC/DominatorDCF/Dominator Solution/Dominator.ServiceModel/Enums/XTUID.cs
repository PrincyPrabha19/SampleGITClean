namespace Dominator.ServiceModel.Enums
{
    public enum XTUID : uint
    {
        BaseClockFrequency      = 0x00000001,
        CPUActualFrequency      = 0xd0000005,
        CPUEffectiveFrequency =   0x00000005,
        CPUActualVoltage =        0x00000002,
        CPUEffectiveVoltage =     0xa5000004,
        CPUTemperature =          0xa1000001,
        CPUFanSpeed =             0xa4000001,
        CPUVoltageOffset =        0x00000022,
        CPUVoltageMode =          0x00000058,
        CoreActualFrequencyBase = 0x0000001d,
        CoreTempuratureBase =     0x00000007,
        ExtremeMemoryProfile =    0x00000040,
        MemoryActualFrequency =   0x00000013,
        MemoryEffectiveFrequency =0x00000011,
        MemoryClockMultiplier =   0x00000049,
        MemoryActualVoltage =     0x00000005,
        MemoryEffectiveVoltage =  0xA2000013,
        ThermalThrottling       = 0x00000003,
        PowerLimitThrottling    = 0x00000016,
        CurrentLimitThrottling  = 0x00000017,
        IccMaxCurrent           = 0x00000066,
        CacheIccMaxCurrent      = 0x0000006A,
        TurboBoostPowerMax      = 0x00000030,
        TurboBoostShortPowerMax = 0x0000002F,
        TurboBoostShortPowerMaxEnable = 0x00000031 
    }
}
