using System;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class ThermalCapabilitiesClass : ThermalCapabilities
    {
        public bool IsThermalSupported()
        {
            return SysInfoAPIClass.Platform == Platform.Desktop &&
                   String.Compare(SysInfoAPIClass.PCModel, "Andromeda", StringComparison.InvariantCultureIgnoreCase) != 0 && !SysInfoAPIClass.PCModel.Contains("default");
        }
    }
}
