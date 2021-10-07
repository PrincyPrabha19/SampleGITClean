namespace Dominator.Domain
{
    public interface IDataConfiguration
    {
        uint LevelID { get; set; }
        uint RiskLevel { get; set; }
        decimal MaxFrequency { get; set; }
        decimal MinFrequency { get; set; }
        decimal MaxVoltageOverride { get; set; }
        decimal MinVoltageOverride { get; set; }
        decimal DefaultVoltageOverride { get; set; }
        decimal MaxVoltageOffset { get; set; }
        decimal MinVoltageOffset { get; set; }
        decimal DefaultVoltageOffset { get; set; }
        decimal VoltageMode { get; set; }
        decimal Power { get; set; }
        decimal ICCMax { get; set; }
        decimal CacheICCMax { get; set; }
    }

    public interface IConfiguration
    {
        uint LevelID { get; set; }
        uint RiskLevel { get; set; }
        decimal Max { get; set; }
        decimal Min { get; set; }
    }

    public interface IRiskData
    {
        uint RiskLevel { get; set; }
        decimal Value { get; set; }
    }
}
