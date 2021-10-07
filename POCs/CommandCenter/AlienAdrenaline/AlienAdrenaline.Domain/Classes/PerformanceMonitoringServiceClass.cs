using System;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes.Factories;
using AlienLabs.PerformanceMonitoring.SupportService;
using AlienLabs.PerformanceMonitoring.SupportService.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class PerformanceMonitoringServiceClass : PerformanceMonitoringService
    {
        #region Private Properties
        private readonly PerformanceMonitoringSupportProcessorService performanceMonitoringSupportProcessorService;
        #endregion

        #region PerformanceMonitoringService Members
        public void StartRecording()
        {
            var profileName = String.Empty;
            if (ProfileRepositoryFactory.ProfileRepository != null &&
                ProfileRepositoryFactory.ProfileRepository.CurrentProfile != null)
                profileName = ProfileRepositoryFactory.ProfileRepository.CurrentProfile.Name;

            performanceMonitoringSupportProcessorService.StartRecording(profileName);
        }

        public void StopRecording()
        {
            performanceMonitoringSupportProcessorService.StopRecording();
        }
        #endregion

        #region Constructors
        public PerformanceMonitoringServiceClass()
        {
            performanceMonitoringSupportProcessorService = new PerformanceMonitoringSupportProcessorServiceClass();
        }
        #endregion
    }
}