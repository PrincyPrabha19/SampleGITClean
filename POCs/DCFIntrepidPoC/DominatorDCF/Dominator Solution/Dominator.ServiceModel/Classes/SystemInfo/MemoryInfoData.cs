using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.Serialization;
using Dominator.ServiceModel.Enums;

namespace Dominator.ServiceModel.Classes.SystemInfo
{
    [DataContract]
    public class MemoryInfoData : IMemoryInfoData
    {
        public double InstalledMemorySize { get; private set; }        
        public string Mode { get; private set; }
        public int NumberBanks { get; private set; }
        public bool? IsMemoryOCSupported { get; set; }
        public List<BankMemoryData> BankMemoryList { get; private set; }

        public void Initialize()
        {
            BankMemoryList = new List<BankMemoryData>();

#if IS_SYSTEM_INFO_REQUIRED
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
            foreach (var queryObj in searcher.Get())
            {
                var bankMemory = new BankMemoryData();
                try { bankMemory.Manufacturer = queryObj["Manufacturer"].ToString(); } catch { }
                try { bankMemory.DefaultSpeed = Convert.ToInt32(queryObj["ConfiguredClockSpeed"]); } catch { }
                try { bankMemory.Capacity = convertToGigs((ulong)queryObj["Capacity"]); } catch { }
                try { bankMemory.Type = (MemoryType)Convert.ToInt32(queryObj["MemoryType"]); } catch { }

                BankMemoryList.Add(bankMemory);
            }

            InstalledMemorySize = BankMemoryList.Sum(x => x.Capacity);
            NumberBanks = BankMemoryList.Count();
#endif
        }

        public override string ToString()
        {
            return $"InstalledMemorySize: {InstalledMemorySize:#.0} GB\nNumberBanks: {NumberBanks}\nBanks:\n{string.Join("\n", BankMemoryList)}";
        }

        private static double convertToGigs(ulong value)
        {
            return Math.Round((double)value / 1073741824, 0);
        }
    }

    public class BankMemoryData
    {
        public string Manufacturer { get; set; }
        public int DefaultSpeed { get; set; }
        public double Capacity { get; set; }
        public MemoryType Type { get; set; }

        public override string ToString()
        {
            return $"\tManufacturer: {Manufacturer}\n\tDefaultSpeed: {DefaultSpeed} MHz\n\tCapacity: {Capacity:#.0} GB\n\tType: {Type}";
        }
    }
}