namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring
{
	public interface RecordingWriter
	{
        string PrefixSnapshotFileName { get; set; }
		string SnapshotFileName { get; set; }

        void StartRecording(PerformanceMonitoringService monitorService);
		void StopRecording();
	}
}