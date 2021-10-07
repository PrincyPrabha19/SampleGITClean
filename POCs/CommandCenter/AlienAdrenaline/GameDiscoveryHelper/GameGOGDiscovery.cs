using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienLabs.GameDiscoveryHelper
{
    interface GameGOGDiscovery : GameDiscovery
    {
        GameGOGWindows GameGOGWindows { get; set; }
       
    }
}
