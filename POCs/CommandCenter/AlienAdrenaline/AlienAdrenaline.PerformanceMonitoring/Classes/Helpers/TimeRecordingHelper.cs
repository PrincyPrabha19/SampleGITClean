using System;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes.Helpers
{
    public class TimeRecordingHelper
    {
        private long startedTicks;
        private readonly TimeRecordingRegistryHelper timeRecordingRegistryHelper = new TimeRecordingRegistryHelper();

        public void Start()
        {
            startedTicks = timeRecordingRegistryHelper.GetRecordingTimeStarted();
        }

        public int GetStartedSeconds()
        {
            var currentTicks = DateTime.Now.Ticks;
            return (int) Math.Round(TimeSpan.FromTicks(currentTicks - startedTicks).TotalSeconds);
        }
    }
}
