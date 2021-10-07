using System.Collections.Generic;

namespace Dominator.Domain
{
    public interface ICommandContainer
    {
        bool IsEmpty { get; }
        int Count { get; }
        IEnumerable<IEquatableCommand> Commands { get; }

        void Add(IEquatableCommand equatableCommand);
        bool Has(IEquatableCommand equatableCommand);
        void Clear();
    }
}