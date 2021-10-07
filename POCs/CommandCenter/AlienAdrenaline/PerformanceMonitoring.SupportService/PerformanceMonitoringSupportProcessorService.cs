
namespace AlienLabs.PerformanceMonitoring.SupportService
{
    public interface PerformanceMonitoringSupportProcessorService
    {
        void StartRecording(string profileName);
        void StopRecording();
    }
}
