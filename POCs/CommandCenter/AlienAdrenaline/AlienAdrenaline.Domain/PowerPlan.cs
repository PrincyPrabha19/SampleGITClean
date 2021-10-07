using System;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface PowerPlan
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string TempName { get; set; }
        string Description { get; set; }
    }
}

