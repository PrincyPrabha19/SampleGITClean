using System.ComponentModel;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring
{
	public enum MonitoringCategories
	{
		CPU,
		Memory,
		Network,
		GPU
	}

    public enum MonitoringCategoryInfo
    {
		CPU_Usage,
		Memory_Usage,
        Network_BytesReceived,
        Network_BytesSent,
        Video_GPUUsage,
        Video_Temperature,
        Video_MemoryLoad,
        Video_FanSpeed
    }
}
