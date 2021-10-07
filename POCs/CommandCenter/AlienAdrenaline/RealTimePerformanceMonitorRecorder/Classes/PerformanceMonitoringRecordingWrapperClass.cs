using AlienLabs.AlienAdrenaline.PerformanceMonitoring;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes.Helpers;

namespace RealTimePerformanceMonitorRecorder.Classes
{
	public class PerformanceMonitoringRecordingWrapperClass : PerformanceMonitoringRecordingWrapper
    {
        #region Properties
        private PerformanceMonitoringService monitorService;
        private RecordingWriter recordingWriter;
		private bool recording;
        private readonly TimeRecordingRegistryHelper timeRecordingRegistryHelper = new TimeRecordingRegistryHelper();
        #endregion

        #region Methods
        public bool StartRecording(string profileName, string prefix)
        {
			if (recording)
				return false;

            startMonitoring();
            if (monitorService == null || !monitorService.Started)
				return false;

            timeRecordingRegistryHelper.SetRecordingTimeStarted();

            recording = true;
			recordingWriter = new RecordingWriterClass { SnapshotFileName = profileName, PrefixSnapshotFileName = prefix };
            recordingWriter.StartRecording(monitorService);            

			return true;
        }

        public bool StopRecording()
        {
        	if (!recording)
        		return false;

        	if (recordingWriter != null)
        		recordingWriter.StopRecording();

        	recording = false;
        	stopMonitoring();
        	return true;
        }

		private void startMonitoring()
        {
            if (monitorService == null)
                monitorService = new PerformanceMonitoringServiceClass();

            if (monitorService == null)
                return;

            monitorService.Start();
        }

        private void stopMonitoring()
        {
            monitorService.Stop();
        }
        #endregion
    }
}