using System.Threading.Tasks;

namespace Dominator.ServiceModel
{
    public interface ISystemInfo
    {
        IPlatformInfoData PlatformInfoData { get; }
        ICPUInfoData CPUInfoData { get; }
        IMemoryInfoData MemoryInfoData { get; }
        IXMPInfoData XMPInfoData { get; }
        IWatchdogTimerInfo WatchdogTimerInfo { get; }
        void Initialize();
        Task InitializeAsync();

        string ProcessorBrand { get; }
    }
}
