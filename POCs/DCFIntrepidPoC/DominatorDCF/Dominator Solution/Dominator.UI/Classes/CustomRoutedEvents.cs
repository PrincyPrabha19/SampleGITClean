using System.Windows;
using Dominator.Domain;
using Dominator.UI.Views;

namespace Dominator.UI.Classes
{
    public class CustomRoutedEvents
    {
        #region Command Event
        public static readonly RoutedEvent CommandEvent =
            EventManager.RegisterRoutedEvent("CommandEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ShellView));

        public static void RaiseCommandEvent(IEquatableCommand equatableCommand, object source)
        {
            if (equatableCommand == null) return;
            var newEventArgs = new CommandEventArgs(CommandEvent, source, equatableCommand);
            ((UIElement)source).RaiseEvent(newEventArgs);
        }

        public class CommandEventArgs : RoutedEventArgs
        {
            public IEquatableCommand equatableCommand;

            public CommandEventArgs(RoutedEvent routed_event, object source, IEquatableCommand equatableCommand)
                : base(routed_event, source)
            {
                this.equatableCommand = equatableCommand;
            }
        }
        #endregion
    }
}