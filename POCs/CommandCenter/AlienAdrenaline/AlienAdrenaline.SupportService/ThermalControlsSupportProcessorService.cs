
using System;

namespace AlienAdrenaline.SupportService
{
    public interface ThermalControlsSupportProcessorService
    {
        void SetThermalProfile(Guid id);

        Guid LoadThermalProfile(string profilePath, string profileName);

        void Dispose();
    }
}
