namespace Dominator.Domain.Classes.Factories
{
    public class CommandContainerFactory
    {
        //private static ICommandContainer commandContainer;
        //public static ICommandContainer CommandContainer => commandContainer ?? (commandContainer = new CommandContainer());
        public static ICommandContainer CommandContainer => new CommandContainer();
    }
}