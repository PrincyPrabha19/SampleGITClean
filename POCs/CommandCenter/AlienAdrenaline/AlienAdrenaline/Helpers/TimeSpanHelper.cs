using System;

namespace AlienLabs.AlienAdrenaline.App.Helpers
{
    public class TimeSpanHelper
    {
        public static string ConvertTotalSecondsToHourMinutesSeconds(int totalSeconds)
        {
            var timeSpan = new TimeSpan(0, 0, totalSeconds);
            return String.Format("{0:0}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}
