using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Factories
{
    public class PerfMonBufferFactory
    {
        #region Static Properties
        public const int BUFFER_SIZE = 60;

        public static PerformanceMonitoringBuffer NewPerformanceMonitoringBuffer()
        {
            return new PerformanceMonitoringBufferClass();
        }
        #endregion
    }
}
