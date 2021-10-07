using System;
using System.Collections.Generic;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain
{
    public interface IDataComponent
    {
        bool IsOCEnabled { get; set; }
        bool IsCompatibleWithCurrentHW { get; set; }
        //bool IsRestartRequired { get; set; }
        HWComponentType Type { get; }

        //TODO: Add CPUDataCompoment, MemoryDataComponent
    }

    public interface ICoreData
    { 
        decimal Multiplier { get; }
        decimal Frequency { get; }

        void UpdateValuesToProfile(decimal baseClock, decimal frequency);
    }

    public class CoreData : ICoreData
    {
        public decimal Multiplier { get; private set; }
        public decimal Frequency { get; private set; }

        public CoreData(decimal baseClock, decimal multiplier)
        {
            Multiplier = multiplier;
            Frequency = multiplier * baseClock;
        }

        public void UpdateValuesToProfile(decimal baseClock, decimal frequency)
        {
            Frequency = frequency;
            Multiplier = frequency/baseClock;
        }
    }

    public interface ICPUDataComponent : IDataComponent
    {
        decimal BaseClock { get; set; }
        string XtuPath { get; set; }
        string Brand { get; set; }
        decimal Voltage { get; set; }
        decimal VoltageOffset { get; set; }
        int VoltageMode { get; set; }
        decimal Power { get; set;}
        decimal ICCMax { get; set; }
        decimal CacheICCMax { get; set; }
        decimal Frequency { get; set; }
        decimal Multiplier { get; set; }
        List<ICoreData> CoreDataList { get; set; }
    }

    public class CPUDataComponent : ICPUDataComponent
    {                
        public bool IsOCEnabled { get; set; }
        public bool IsCompatibleWithCurrentHW { get; set; }
        public HWComponentType Type => HWComponentType.CPU;

        private decimal baseClock;
        public decimal BaseClock
        {
            get { return baseClock; }
            set { baseClock = value; }
        }

        public string XtuPath { get; set; }
        public string Brand { get; set; }
        public decimal Voltage { get; set; }
        public decimal VoltageOffset { get; set; }
        public int VoltageMode { get; set; }
        public decimal Power { get; set;}
        public decimal ICCMax { get; set; }
        public decimal CacheICCMax { get; set; }
        private decimal multiplier;
        public decimal Multiplier
        {
            get { return multiplier; }
            set
            {
                multiplier = value;
                frequency = (multiplier * baseClock) / 1000;
            }
        }

        private decimal frequency;
        public decimal Frequency
        {
            get { return frequency; }
            set
            {
                frequency = value;
                multiplier = Math.Round((frequency / baseClock) * 1000, 0);
            }
        }

        public List<ICoreData> CoreDataList { get; set; }
    }

    public interface IMemoryDataComponent : IDataComponent
    {
        decimal BaseClock { get; set; }
        int ProfileID { get; set; }
        decimal Voltage { get; set; }
        decimal Multiplier { get; set; }
        decimal Frequency { get; set; }
    }

    public class MemoryDataComponent : IMemoryDataComponent
    {
        public bool IsOCEnabled { get; set; }
        public bool IsCompatibleWithCurrentHW { get; set; }
        public HWComponentType Type => HWComponentType.Memory;

        private decimal baseClock;
        public decimal BaseClock
        {
            get { return baseClock; }
            set { baseClock = value; }
        }

        public int ProfileID { get; set; }
        public decimal Voltage { get; set; }

        private decimal clockMultiplier;
        public decimal ClockMultiplier
        {
            get { return clockMultiplier; }
            set { clockMultiplier = value; }
        }

        private decimal multiplier;
        public decimal Multiplier
        {
            get { return multiplier; }
            set
            {
                multiplier = value;
                frequency = (multiplier * baseClock * clockMultiplier) / 1000;
            }
        }

        private decimal frequency;
        public decimal Frequency
        {
            get { return frequency; }
            set
            {
                frequency = value;
                multiplier = Math.Round((frequency / baseClock / clockMultiplier) * 1000, 0);
            }
        }
    }
}