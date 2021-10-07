using System;
using System.Collections.Generic;

namespace AlienAdrenaline.SupportService
{
    public interface ThermalControlsSupportReaderService
    {
        List<ThermalProfile> GetAllThermalProfiles();
        Guid GetActiveThermalProfile();
        Guid GetDefaultThermalProfile();
        string GetThermalConfiguration();
        void Dispose();
    }
}
