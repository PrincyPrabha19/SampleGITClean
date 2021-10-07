using System;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileActionInfoPerformanceMonitoringClass : ProfileActionInfoProcessorClass, ProfileActionInfo
    {        
        #region ProfileActionInfo Members
        public Guid Guid { get; private set; }
        public int Id { get; set; }
        public int ProfileActionId { get; set; }

        public override void Execute()
        {
            ServiceFactory.PerformanceMonitoringService.StartRecording();
        }

        public override void Rollback()
        {
            ServiceFactory.PerformanceMonitoringService.StopRecording();
        }

        public ProfileActionInfo Clone()
        {
            return new ProfileActionInfoPerformanceMonitoringClass()
            {                
                Guid = Guid,
                Id = Id
            };
        }

        public bool Equals(ProfileActionInfo profileActionInfo)
        {
            return Guid == ((ProfileActionInfoPerformanceMonitoringClass) profileActionInfo).Guid;
        }

        public ProfileActionStatus GetStatus()
        {
            return ProfileActionStatus.None;
        }
        #endregion

        #region Constructors
        public ProfileActionInfoPerformanceMonitoringClass()
        {
            Guid = Guid.NewGuid();
        }

        public ProfileActionInfoPerformanceMonitoringClass(bool initialize) 
            : this()
        {
            if (initialize)
            {
            }                        
        }
        #endregion
    }
}
