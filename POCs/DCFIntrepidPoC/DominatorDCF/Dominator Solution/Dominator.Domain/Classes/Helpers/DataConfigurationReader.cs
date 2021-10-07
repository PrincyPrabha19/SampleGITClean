using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using Dominator.Domain.Classes.Factories;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes.Helpers
{
    public class DataConfigurationReader : IDataConfigurationReader
    {
        public ITuningManager TuningManager { get; set; }
        public string Model { get; set; }
        public bool ThermalThrottling { get;  set; }
        public bool PowerLimitThrottling { get;  set; }
        public bool CurrentLimitThrottling { get;  set; }
        public bool VoltageOffsetAlwaysOn { get; set; }
        public decimal FanType { get; set; }
        public decimal FanMaxValue { get; set; }
        public decimal FanMinValue { get; set; }
        public List<IDataConfiguration> FrequencyConfiguration { get; private set; }
        public List<IConfiguration> CoreVoltageConfiguration { get; private set; }
        public List<IConfiguration> VoltageOffsetConfiguration { get; private set; }
        public int NumberOfFreqLevels  { get; set; }
        public int NumberOfCoreVoltageLevels { get; set; }
        public int NumberOfVoltageOffsetLevels { get; set; }

        private static readonly string DataConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Alienware\OCControls\DataConfig.occ");
        

        public void Initialize()
        {
            TuningManager = TuningFactory.NewTuningManager();
            FrequencyConfiguration = new List<IDataConfiguration>();
            CoreVoltageConfiguration = new List<IConfiguration>();
            VoltageOffsetConfiguration = new List<IConfiguration>();
            try
            {
                var dataConfig =  deserializeConfigData(DataConfigPath);
                if (dataConfig != null)
                {
                    Model = dataConfig.Model;
                    ThermalThrottling = dataConfig.ThrottlingIgnored == null ? true : !dataConfig.ThrottlingIgnored.Thermal;
                    PowerLimitThrottling = dataConfig.ThrottlingIgnored == null ? true : !dataConfig.ThrottlingIgnored.PowerLimit;
                    CurrentLimitThrottling = dataConfig.ThrottlingIgnored == null ? true : !dataConfig.ThrottlingIgnored.CurrentLimit;
                    VoltageOffsetAlwaysOn = dataConfig.VoltageOffset.AlwaysOn;
                    FanType = Convert.ToDecimal(dataConfig.Fan.Type, CultureInfo.InvariantCulture);
                    FanMaxValue = Convert.ToDecimal(dataConfig.Fan.Max, CultureInfo.InvariantCulture);
                    FanMinValue = Convert.ToDecimal(dataConfig.Fan.Min, CultureInfo.InvariantCulture);
                    NumberOfFreqLevels = dataConfig.Frequency.Length;
                    NumberOfCoreVoltageLevels = dataConfig.CoreVoltage.Length;
                    NumberOfVoltageOffsetLevels = dataConfig.VoltageOffset.Level.Length;
                    foreach (var level in dataConfig.Frequency)
                        addLevelConfiguration(level);
                    foreach (var level in dataConfig.CoreVoltage)
                        addVoltageConfiguration(level);
                    foreach (var level in dataConfig.VoltageOffset.Level)
                        addVoltageOffsetConfiguration(level);
                }
            }
            catch (Exception e)
            {
            }
        }

        private void addVoltageOffsetConfiguration(Level level)
        {
            VoltageOffsetConfiguration.Add(new Configuration
            {
                LevelID = Convert.ToUInt16(level.Id, CultureInfo.InvariantCulture),
                RiskLevel = Convert.ToUInt16(level.Risk, CultureInfo.InvariantCulture),
                Max = Convert.ToDecimal(level.Max, CultureInfo.InvariantCulture),
                Min = Convert.ToDecimal(level.Min, CultureInfo.InvariantCulture)
            });
        }

        private void addVoltageConfiguration(Level level)
        {
            CoreVoltageConfiguration.Add(new Configuration
            {
                LevelID = Convert.ToUInt16(level.Id, CultureInfo.InvariantCulture),
                RiskLevel = Convert.ToUInt16(level.Risk, CultureInfo.InvariantCulture),
                Max = Convert.ToDecimal(level.Max, CultureInfo.InvariantCulture),
                Min = Convert.ToDecimal(level.Min, CultureInfo.InvariantCulture)
            });
        }

        private void addLevelConfiguration(Level level)
        {
            var idGenerator = new SettingIDGenerator();
            FrequencyConfiguration.Add(new DataConfiguration
            {
                LevelID = Convert.ToUInt16(level.Id, CultureInfo.InvariantCulture),
                RiskLevel = Convert.ToUInt16(level.Risk, CultureInfo.InvariantCulture),
                MaxFrequency = Convert.ToDecimal(level.Max, CultureInfo.InvariantCulture),
                MinFrequency = Convert.ToDecimal(level.Min, CultureInfo.InvariantCulture),
                VoltageMode = Convert.ToDecimal(level.Voltage.Mode, CultureInfo.InvariantCulture),
                MaxVoltageOverride = Convert.ToDecimal(level.Voltage.Core.Max, CultureInfo.InvariantCulture),
                MinVoltageOverride = Convert.ToDecimal(level.Voltage.Core.Min, CultureInfo.InvariantCulture),
                DefaultVoltageOverride = Convert.ToDecimal(level.Voltage.Core.Default, CultureInfo.InvariantCulture),
                MaxVoltageOffset = Convert.ToDecimal(level.Voltage.Offset.Max, CultureInfo.InvariantCulture),
                MinVoltageOffset = Convert.ToDecimal(level.Voltage.Offset.Min, CultureInfo.InvariantCulture),
                DefaultVoltageOffset = Convert.ToDecimal(level.Voltage.Offset.Default, CultureInfo.InvariantCulture),
                Power = Convert.ToDecimal(level.Power, CultureInfo.InvariantCulture),
                ICCMax = level.ICCMax == "Auto" ? TuningManager.GetDefaultValue(idGenerator.GetID(HWComponentType.CPU, SettingType.IccMaxCurrent, 0)) : Convert.ToDecimal(level.ICCMax, CultureInfo.InvariantCulture),
                CacheICCMax = level.CacheICCMax == "Auto" ? TuningManager.GetDefaultValue(idGenerator.GetID(HWComponentType.CPU, SettingType.CacheIccMaxCurrent, 0)) : Convert.ToDecimal(level.CacheICCMax, CultureInfo.InvariantCulture)
            });
        }

        private DataConfig deserializeConfigData(string path)
        {
            if (!File.Exists(DataConfigPath)) return null;

            DataConfig dataConfig;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataConfig));
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                dataConfig = xmlSerializer.Deserialize(fs) as DataConfig;
                fs.Close();
            }

            return dataConfig;
        }
    }
}