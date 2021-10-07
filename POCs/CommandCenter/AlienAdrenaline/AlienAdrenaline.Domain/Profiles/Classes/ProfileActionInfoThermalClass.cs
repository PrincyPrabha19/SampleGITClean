

using System;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileActionInfoThermalClass : ProfileActionInfoProcessorClass, ProfileActionInfo
    {
        #region Public Properties
        public Guid ThermalProfileId { get; set; }
        public string ThermalProfileName { get; set; }
        #endregion

        #region ProfileActionInfo Members
        public Guid Guid { get; private set; }
        public int Id { get; set; }
        public int ProfileActionId { get; set; }

        public override void Execute()
        {
            if (!ServiceFactory.ThermalProfileService.ExistsThermalProfile(ThermalProfileId))
                throw new Exception(Properties.Resources.ThermalProfileDoesNotExistsErrorText);

            ServiceFactory.ThermalProfileService.SetThermalProfile(ThermalProfileId);
        }

        public ProfileActionInfo Clone()
        {
            return new ProfileActionInfoThermalClass()
            {
                Guid = Guid,
                ThermalProfileId = ThermalProfileId,
                ThermalProfileName = ThermalProfileName
            };
        }

        public bool Equals(ProfileActionInfo profileActionInfo)
        {
            return Guid == ((ProfileActionInfoThermalClass)profileActionInfo).Guid &&
                   ThermalProfileId == ((ProfileActionInfoThermalClass)profileActionInfo).ThermalProfileId &&
                   ThermalProfileName == ((ProfileActionInfoThermalClass)profileActionInfo).ThermalProfileName;
        }

        public ProfileActionStatus GetStatus()
        {
            return ProfileActionStatus.None;
        }
        #endregion

        #region Constructors
        public ProfileActionInfoThermalClass()
        {
            Guid = Guid.NewGuid();
            ThermalProfileId = Guid.Empty;
            ThermalProfileName = String.Empty;
        }

        public ProfileActionInfoThermalClass(bool initialize)
            : this()
        {
            if (initialize)
            {
                var thermalProfileService = ServiceFactory.ThermalProfileService;
                if (thermalProfileService != null)
                    ThermalProfileId = thermalProfileService.GetActiveThermalProfile();
            }
        }
        #endregion
    }
}
