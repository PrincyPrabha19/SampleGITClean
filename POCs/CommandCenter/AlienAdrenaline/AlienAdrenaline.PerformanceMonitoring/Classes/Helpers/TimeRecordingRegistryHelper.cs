using System;
using Microsoft.Win32;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes.Helpers
{
    public interface TimeRecordingRegistryReader
    {
        long GetRecordingTimeStarted();
    }

    public interface TimeRecordingRegistryWriter
    {
        void SetRecordingTimeStarted();
    }

    public class TimeRecordingRegistryHelper : TimeRecordingRegistryReader, TimeRecordingRegistryWriter
    {
        private const string CustomPath = @"Software\Alienware\Command Center";

        public long GetRecordingTimeStarted()
        {
            using (RegistryKey root = Registry.CurrentUser.CreateSubKey(CustomPath))
            {
                if (root != null)
                {
                    var value = root.GetValue("RecordingStarted");
                    if (value != null)
                        return Convert.ToInt64(value);
                }
            }

            return 0;
        }

        public void SetRecordingTimeStarted()
        {
            using (RegistryKey root = Registry.CurrentUser.CreateSubKey(CustomPath))
            {
                if (root != null)
                    root.SetValue("RecordingStarted", DateTime.Now.Ticks);
            }
        }
    }
}
