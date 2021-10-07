using System.Management;
using System.Threading.Tasks;
using Dominator.ServiceModel.Classes.Factories;

namespace Dominator.ServiceModel.Classes.SystemInfo
{
    public class SystemInfo : ISystemInfo
    {
        public ICPUInfoData CPUInfoData { get; private set; }
        public IPlatformInfoData PlatformInfoData { get; private set; }        
        public IMemoryInfoData MemoryInfoData { get; private set; }
        public IXMPInfoData XMPInfoData { get; private set;}
        public IWatchdogTimerInfo WatchdogTimerInfo { get; private set; }
        public string ProcessorBrand => CPUInfoData?.ProcessorBrand;

        public void Initialize()
        {
            CPUInfoData = ServiceRepository.XTUServiceInstance?.GetCPUInfoData() ?? new CPUInfoData();
            XMPInfoData = ServiceRepository.XTUServiceInstance?.GetXMPInfoData() ?? new XMPInfoData();

            PlatformInfoData = new PlatformInfoData();
            PlatformInfoData.Initialize();

            MemoryInfoData = new MemoryInfoData();
            MemoryInfoData.Initialize();
            MemoryInfoData.IsMemoryOCSupported = ServiceRepository.XTUServiceInstance?.IsMemoryOCSupported();

            WatchdogTimerInfo = ServiceRepository.XTUServiceInstance?.GetWatchdogTimerInfo();
        }

        public async Task InitializeAsync()
        {
            Initialize();
        }

        public override string ToString()
        {
            return CPUInfoData + "\n\n" + PlatformInfoData + "\n\n" + MemoryInfoData + "\n\n" + XMPInfoData + "\n\n" + WatchdogTimerInfo;
        }
    }
}
