using System;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileActionInfoEnergyBoosterClass : ProfileActionInfoProcessorClass, ProfileActionInfo
    {        
        #region ProfileActionInfo Members
        public Guid Guid { get; private set; }
        public int Id { get; set; }
        public int ProfileActionId { get; set; }

        private bool? autoUpdateDefinitions;
        public bool AutoUpdateDefinitions
        {
            get
            {
                return !autoUpdateDefinitions.HasValue ? Id <= 0 : autoUpdateDefinitions.Value && Id <= 0; 
            }

            set { autoUpdateDefinitions = value; }
        }

        public override void Execute()
        {
            ServiceFactory.EnergyBoosterService.DoStopProgressed += EnergyBoosterServiceDoStopProgressed;
            //ServiceFactory.EnergyBoosterService.InitDefinitions();
            ServiceFactory.EnergyBoosterService.DoStop();
        }

        public override void Rollback()
        {
            ServiceFactory.EnergyBoosterService.DoRestartProgressed += EnergyBoosterServiceDoRestartProgressed;
            ServiceFactory.EnergyBoosterService.DoRestart();
        }

        public ProfileActionInfo Clone()
        {
            return new ProfileActionInfoEnergyBoosterClass()
            {                
                Guid = Guid,
                Id = Id
            };
        }

        public bool Equals(ProfileActionInfo profileActionInfo)
        {
            return Guid == ((ProfileActionInfoEnergyBoosterClass) profileActionInfo).Guid;
        }

        public ProfileActionStatus GetStatus()
        {
            return ProfileActionStatus.None;
        }
        #endregion

        #region Constructors
        public ProfileActionInfoEnergyBoosterClass()
        {
            Guid = Guid.NewGuid();
        }

        public ProfileActionInfoEnergyBoosterClass(bool initialize) 
            : this()
        {
            if (initialize)
            {
            }                        
        }
        #endregion

        #region Event Handlers
        private void EnergyBoosterServiceDoStopProgressed(int completed, int total)
        {
            OnExecuteProgressed(completed, total);
        }

        private void EnergyBoosterServiceDoRestartProgressed(int completed, int total)
        {
            OnRollbackProgressed(completed, total);
        }
        #endregion
    }
}
