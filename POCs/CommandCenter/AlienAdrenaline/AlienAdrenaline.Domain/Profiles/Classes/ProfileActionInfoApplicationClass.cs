

using System;
using System.Diagnostics;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileActionInfoApplicationClass : ProfileActionInfoProcessorClass, ProfileActionInfoApplication
    {
        public Guid Guid { get; private set; }

        public int Id { get; set; }
        public int ProfileActionId { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationPath { get; set; }

        public ProfileActionInfoApplicationClass()
        {
            Guid = Guid.NewGuid();
            ApplicationName = String.Empty;
            ApplicationPath = String.Empty;
        }

        public override void Execute()
        {
            Process process;
            ApplicationLaunchHelper.Execute(ApplicationPath, out process);
        }

        public ProfileActionInfo Clone()
        {
            return new ProfileActionInfoApplicationClass()
            {
                Guid = Guid,
                ApplicationName = ApplicationName,
                ApplicationPath = ApplicationPath,
            };
        }

        public bool Equals(ProfileActionInfo profileActionInfo)
        {
            return Guid == ((ProfileActionInfoApplicationClass) profileActionInfo).Guid &&
                   ApplicationName == ((ProfileActionInfoApplicationClass) profileActionInfo).ApplicationName &&
                   ApplicationPath == ((ProfileActionInfoApplicationClass) profileActionInfo).ApplicationPath;
        }

        public ProfileActionStatus GetStatus()
        {
            if (String.IsNullOrEmpty(ApplicationPath))
                return ProfileActionStatus.NotReady;
            return ProfileActionStatus.None;
        }
    }
}
