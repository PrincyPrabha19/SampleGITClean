using System;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface ThermalProfile
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string TempName { get; set; }
    }
}

