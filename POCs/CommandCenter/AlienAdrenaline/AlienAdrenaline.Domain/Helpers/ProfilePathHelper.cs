using System;

namespace AlienLabs.AlienAdrenaline.Domain.Helpers
{
    public abstract class ProfilesPathHelper
    {
		public string ProfilesFile = String.Format(@"{0}Profiles.xml", AdrenalinePathProvider.AdrenalineFolder);               
    }
}
