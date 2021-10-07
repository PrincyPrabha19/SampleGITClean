
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface PerformanceMonitoringService
    {
        void StartRecording();
        void StopRecording();
    }
}
