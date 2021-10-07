using System.Collections.Generic;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface CommandContainer
    {
        bool IsEmpty { get; }
        int Count { get; }
        IEnumerable<EquatableCommand> Commands { get; }

        void Add(EquatableCommand equatableCommand);
        bool Has(EquatableCommand equatableCommand);
        void Clear();
    }
}