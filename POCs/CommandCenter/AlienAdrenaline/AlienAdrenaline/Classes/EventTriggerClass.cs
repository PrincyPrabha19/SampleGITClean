using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;

namespace AlienLabs.AlienAdrenaline.App.Classes
{
    public class EventTriggerClass : EventTrigger
    {
        private readonly object source;
        
        public EventTriggerClass(object source)
        {
            this.source = source;
        }

        public void Fire(EquatableCommand equatableCommand)
        {
            CustomRoutedEvents.RaiseCommandEvent(equatableCommand, source);
        }

        public void Fire(ViewType viewType, object parameter)
        {
            CustomRoutedEvents.RaiseViewActivatedEvent(viewType, parameter, source);
        }

		public void Fire(ProfileActionType profileActionType)
		{
			CustomRoutedEvents.RaiseProfileViewEvent(profileActionType, source);
		}
    }
}