using System;

namespace AlienAdrenaline.SupportService
{
    public interface PowerPlan
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
}
