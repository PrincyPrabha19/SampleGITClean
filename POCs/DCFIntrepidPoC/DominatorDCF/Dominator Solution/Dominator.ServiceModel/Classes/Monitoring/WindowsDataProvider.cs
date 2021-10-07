using System.Collections.Generic;
using System.Diagnostics;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.ServiceModel.Classes.Helpers;

namespace Dominator.ServiceModel.Classes.Monitoring
{
    public class WindowsDataProvider : IDataProvider<decimal>
    {
        private PerformanceCounter cpu;
        private PerformanceCounter memory;
        private Dictionary<uint, PerformanceCounter> usageLookup;

        public WindowsDataProvider()
        {
            init();
        }

        public decimal GetControlValue(uint controlID)
        {
            return usageLookup.ContainsKey(controlID) ? (decimal)usageLookup[controlID].NextValue() : 0;
        }

        public decimal[] GetAllControl(uint[] controlIDs)
        {
            return null;
        }

        private void init()
        {
            if (usageLookup != null) return;

            usageLookup = new Dictionary<uint, PerformanceCounter>();
            try
            {
                for (uint i = 0; i < getCoreCount(); i++)
                {
                    var coreID = i + SettingsIDRepository.WINDOWS_BASEID + 1;
                    if (usageLookup.ContainsKey(coreID)) continue;

                    var core = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
                    usageLookup.Add(coreID, core);
                    core.NextValue();
                }

                cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                usageLookup.Add(SettingsIDRepository.WINDOWS_BASEID, cpu);
                memory = new PerformanceCounter("Memory", "% Committed Bytes In Use", true);
                usageLookup.Add(SettingsIDRepository.WINDOWS_MEMORY_BASEID, memory);
                cpu.NextValue();
                memory.NextValue();
            }
            catch
            {
            }
        }

        private static uint getCoreCount()
        {
           ISystemInfo systemInfo = SystemInfoRepository.Instance;
           return systemInfo.CPUInfoData.PhysicalCpuCores;
        }
    }
}
