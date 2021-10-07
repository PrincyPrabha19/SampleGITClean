
using System.Collections.Generic;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface WebBrowserService
    {
        string DefaultBrowserPath { get; }
        bool EnableTabbedBrowsing { get; set; }
        void Execute(IEnumerable<string> urls, bool enableTabbedBrowsing);
    }
}
