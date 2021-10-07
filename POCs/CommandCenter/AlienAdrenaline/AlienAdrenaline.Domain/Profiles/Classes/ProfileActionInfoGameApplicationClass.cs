

using System;
using System.Diagnostics;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileActionInfoGameApplicationClass : ProfileActionInfoProcessorClass, ProfileActionInfoGameApplication, ProfileActionInfoApplicationProcessor
    {
        public Guid Guid { get; private set; }

        public int Id { get; set; }
        public int ProfileActionId { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationPath { get; set; }
        public string ApplicationRealPath { get; set; }

        public ProfileActionInfoGameApplicationClass()
        {
            Guid = Guid.NewGuid();
            ApplicationName = String.Empty;
            ApplicationPath = String.Empty;
            ApplicationRealPath = String.Empty;
        }

        public override void Execute()
        {
            Process process;
            Execute(out process);
        }

        public void Execute(out Process process)
        {
            ApplicationLaunchHelper.Execute(ApplicationPath, out process);
        }

        public ProfileActionInfo Clone()
        {
            return new ProfileActionInfoGameApplicationClass()
            {
                Guid = Guid,
                ApplicationName = ApplicationName,
                ApplicationPath = ApplicationPath,
                ApplicationRealPath = ApplicationRealPath
            };
        }

        public bool Equals(ProfileActionInfo profileActionInfo)
        {
            return Guid == ((ProfileActionInfoGameApplicationClass)profileActionInfo).Guid &&
                   ApplicationName == ((ProfileActionInfoGameApplicationClass)profileActionInfo).ApplicationName &&
                   ApplicationPath == ((ProfileActionInfoGameApplicationClass)profileActionInfo).ApplicationPath &&
                   ApplicationRealPath == ((ProfileActionInfoGameApplicationClass)profileActionInfo).ApplicationRealPath;
        }

        public ProfileActionStatus GetStatus()
        {
            if (String.IsNullOrEmpty(ApplicationPath) || String.IsNullOrEmpty(ApplicationRealPath))
                return ProfileActionStatus.NotReady;
            return ProfileActionStatus.None;
        }
    }
}
