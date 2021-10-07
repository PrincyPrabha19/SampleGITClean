namespace Dominator.Domain.Classes.Helpers
{
    public class DataConfiguration : IDataConfiguration
    {
        public uint LevelID { get; set; }
        public uint RiskLevel { get; set; }
        public decimal MaxFrequency { get; set; }
        public decimal MinFrequency { get; set; }
        public decimal MaxVoltageOverride { get; set; }
        public decimal MinVoltageOverride { get; set; }
        public decimal DefaultVoltageOverride { get; set; }
        public decimal MaxVoltageOffset { get; set; }
        public decimal MinVoltageOffset { get; set; }
        public decimal DefaultVoltageOffset { get; set; }
        public decimal VoltageMode { get; set; }
        public decimal Power { get; set; }
        public decimal ICCMax { get; set; }
        public decimal CacheICCMax { get; set; }
    }

    public class Configuration : IConfiguration
    {
        public uint LevelID { get; set; }
        public uint RiskLevel { get; set; }
        public decimal Max { get; set; }
        public decimal Min { get; set; }
    }

    public class RiskData : IRiskData
    {
        public uint RiskLevel { get; set; }
        public decimal Value { get; set; }
    }
}