using System.Collections.Generic;

namespace Dominator.Domain
{
    public interface IProfileDiscovery
    {
        Dictionary<string, string> DiscoverPredefinedProfiles();
        Dictionary<string, string> DiscoverCustomProfiles();
    }
}