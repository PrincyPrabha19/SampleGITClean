

using System;
using System.Diagnostics;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileActionInfoAdditionalApplicationClass : ProfileActionInfoProcessorClass, ProfileActionInfoApplication
    {
        public Guid Guid { get; private set; }

        public int Id { get; set; }
        public int ProfileActionId { get; set; }        
        public string ApplicationName { get; set; }
        public string ApplicationPath { get; set; }
        public bool LaunchIfNotOpen { get; set; }

        public ProfileActionInfoAdditionalApplicationClass()
        {
            Guid = Guid.NewGuid();
            ApplicationName = String.Empty;
            ApplicationPath = String.Empty;
            LaunchIfNotOpen = true;
        }

        public override void Execute()
        {
            Process process;
            ApplicationLaunchHelper.Execute(ApplicationPath, LaunchIfNotOpen, out process);
        }

        public ProfileActionInfo Clone()
        {
            return new ProfileActionInfoAdditionalApplicationClass()
            {
                Guid = Guid,
                ApplicationName = ApplicationName,
                ApplicationPath = ApplicationPath,
                LaunchIfNotOpen = LaunchIfNotOpen
            };
        }

        public bool Equals(ProfileActionInfo profileActionInfo)
        {
            return Guid == ((ProfileActionInfoAdditionalApplicationClass) profileActionInfo).Guid &&
                   ApplicationName == ((ProfileActionInfoAdditionalApplicationClass) profileActionInfo).ApplicationName &&
                   ApplicationPath == ((ProfileActionInfoAdditionalApplicationClass) profileActionInfo).ApplicationPath &&
                   LaunchIfNotOpen == ((ProfileActionInfoAdditionalApplicationClass) profileActionInfo).LaunchIfNotOpen;
        }

        public ProfileActionStatus GetStatus()
        {
            if (String.IsNullOrEmpty(ApplicationPath))
                return ProfileActionStatus.NotReady;
            return ProfileActionStatus.None;
        }
    }
}
