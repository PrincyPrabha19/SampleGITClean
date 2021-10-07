using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienLabs.GameDiscoveryHelper
{
    public interface GameOriginDiscovery : GameDiscovery
    {
        GameOriginWindows GameOriginWindows { get; set; }
    }
}
