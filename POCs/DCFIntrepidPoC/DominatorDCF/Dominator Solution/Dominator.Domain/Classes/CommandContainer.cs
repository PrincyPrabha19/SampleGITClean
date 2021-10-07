using System.Collections.Generic;

namespace Dominator.Domain.Classes
{
    public class CommandContainer : ICommandContainer
    {
        private readonly IList<IEquatableCommand> commands = new List<IEquatableCommand>();

        public bool IsEmpty => commands.Count == 0;
        public int Count => commands.Count;
        public IEnumerable<IEquatableCommand> Commands => commands;

        public void Add(IEquatableCommand IEquatableCommand)
        {
            RemoveIfPresent(IEquatableCommand);
            AddIfNotRedundant(IEquatableCommand);
        }

        private void AddIfNotRedundant(IEquatableCommand IEquatableCommand)
        {
            if (!IEquatableCommand.IsRedundant)
                commands.Add(IEquatableCommand);
        }

        private void RemoveIfPresent(IEquatableCommand IEquatableCommand)
        {
            commands.Remove(IEquatableCommand);
        }

        public bool Has(IEquatableCommand IEquatableCommand)
        {
            return commands.Contains(IEquatableCommand);
        }

        public void Clear()
        {
            commands.Clear();
        }
    }
}