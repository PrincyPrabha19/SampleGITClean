
namespace RealTimePerformanceMonitorRecorder
{
    public interface PerformanceMonitoringRecordingWrapper
    {
		bool StartRecording(string profileName, string prefix);
        bool StopRecording();
    }
}
