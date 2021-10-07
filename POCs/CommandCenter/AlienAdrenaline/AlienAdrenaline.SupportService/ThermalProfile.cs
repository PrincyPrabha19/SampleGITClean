using System;

namespace AlienAdrenaline.SupportService
{
    public interface ThermalProfile
    {
        Guid Id { get; set; }
        string Name { get; set; }
    }
}
