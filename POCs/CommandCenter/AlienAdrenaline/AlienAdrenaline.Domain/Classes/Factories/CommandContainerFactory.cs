

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Factories
{
    public class CommandContainerFactory
    {
        private static CommandContainer commandContainer;
        public static CommandContainer CommandContainer
        {
            get { return commandContainer ?? (commandContainer = new CommandContainerClass()); }
        }
    }
}