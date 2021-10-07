using System.Collections.Generic;
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class CommandContainerClass : CommandContainer
    {
        private readonly IList<EquatableCommand> commands = new List<EquatableCommand>();

        public bool IsEmpty { get { return commands.Count == 0; } }
        public int Count { get { return commands.Count; } }
        public IEnumerable<EquatableCommand> Commands { get { return commands; } }

        public void Add(EquatableCommand equatableCommand)
        {
            RemoveIfPresent(equatableCommand);
            AddIfNotRedundant(equatableCommand);
        }

        private void AddIfNotRedundant(EquatableCommand equatableCommand)
        {
            if (!equatableCommand.IsRedundant)
            {
                if (equatableCommand.CommandType != CommandType.ProfileActions)
                    commands.Add(equatableCommand);
                else
                    commands.Insert(0, equatableCommand);
            }            
        }

        private void RemoveIfPresent(EquatableCommand equatableCommand)
        {
            commands.Remove(equatableCommand);
        }

        public bool Has(EquatableCommand equatableCommand)
        {
            return commands.Contains(equatableCommand);
        }

        public void Clear()
        {
            commands.Clear();
        }
    }
}