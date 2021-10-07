using Dominator.Domain;

namespace Dominator.UI.Classes
{
    public class EventTrigger : IEventTrigger
    {
        private readonly object source;

        public EventTrigger(object source)
        {
            this.source = source;
        }

        public void Fire(IEquatableCommand equatableCommand)
        {
            CustomRoutedEvents.RaiseCommandEvent(equatableCommand, source);
        }
    }
}