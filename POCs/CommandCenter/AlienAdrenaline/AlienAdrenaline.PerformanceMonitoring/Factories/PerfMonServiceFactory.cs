using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Factories
{
    public class PerfMonServiceFactory
    {
        #region Static Properties
        private static PerformanceMonitoringService instance;
        public static PerformanceMonitoringService Instance { get { return instance ?? (instance = new PerformanceMonitoringServiceClass()); } }
        #endregion
    }
}
