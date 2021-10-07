


using System;
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles
{
    public interface ProfileActionInfo : ProfileActionInfoProcessor
    {
        Guid Guid { get; }

        int Id { get; set; }
        int ProfileActionId { get; set; }

        ProfileActionInfo Clone();
        bool Equals(ProfileActionInfo profileActionInfo);
        ProfileActionStatus GetStatus();
    }
}
