using System;
using System.Text.RegularExpressions;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes.Helpers;

namespace AlienLabs.PerformanceMonitoring.SupportService.Classes
{
    public class PerformanceMonitoringSupportProcessorServiceClass : PerformanceMonitoringSupportProcessorService
    {
        #region Private Properties
        private bool recording;
        #endregion

        #region PerformanceMonitoringSupportProcessorService Members
        public void StartRecording(string profileName)
        {
            if (recording)
                return;

			recording = true;
            RTPMRecorderWrapper.StartRecording("", "Game Mode (" + profileName + ") ");
        }

        public void StopRecording()
        {
            if (!recording)
                return;

			RTPMRecorderWrapper.StopRecording();
            recording = false;
        }
        #endregion
    }
}