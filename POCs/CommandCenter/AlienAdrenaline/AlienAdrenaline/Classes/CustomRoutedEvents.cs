using System.Windows;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.App.Views.Xaml;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;

namespace AlienLabs.AlienAdrenaline.App.Classes
{
    public class CustomRoutedEvents
    {
        #region ViewActivated Event
        public static readonly RoutedEvent ViewActivatedEvent =
            EventManager.RegisterRoutedEvent(
                "ViewActivatedEvent",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(AlienAdrenalinePluginCtrl));

        public static void RaiseViewActivatedEvent(ViewType viewType, object viewParameter, object source)
        {
            if (source == null) return;
            var newEventArgs = new ViewActivatedEventArgs(viewType, viewParameter, source);
            ((UIElement)source).RaiseEvent(newEventArgs);
        }

        public class ViewActivatedEventArgs : RoutedEventArgs
        {
            public ViewType ViewType;
            public object ViewParameter;

            public ViewActivatedEventArgs(ViewType viewType, object viewParameter, object source)
                : base(ViewActivatedEvent, source)
            {
                ViewType = viewType;
                ViewParameter = viewParameter;
            }
        } 
        #endregion

        #region Command Event
        public static readonly RoutedEvent CommandEvent =
            EventManager.RegisterRoutedEvent(
                "CommandEvent",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(AlienAdrenalinePluginCtrl));

        public static void RaiseCommandEvent(EquatableCommand equatableCommand, object source)
        {
            if (equatableCommand == null) return;
            var newEventArgs = new CommandEventArgs(CommandEvent, source, equatableCommand);
            ((UIElement)source).RaiseEvent(newEventArgs);
        }

        public class CommandEventArgs : RoutedEventArgs
        {
            public EquatableCommand equatableCommand;

            public CommandEventArgs(RoutedEvent routed_event, object source, EquatableCommand equatableCommand)
                : base(routed_event, source)
            {
                this.equatableCommand = equatableCommand;
            }
        }
        #endregion

        #region ProfileView Event
        public static readonly RoutedEvent ProfileViewEvent =
            EventManager.RegisterRoutedEvent(
                "ProfileViewEvent",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(AlienAdrenalinePluginCtrl));

        public static void RaiseProfileViewEvent(ProfileActionType profileViewAction, object source)
        {
            if (source == null) return;
            var newEventArgs = new ProfileViewEventArgs(profileViewAction, source);
            ((UIElement)source).RaiseEvent(newEventArgs);
        }

        public class ProfileViewEventArgs : RoutedEventArgs
        {
            public ProfileActionType profileViewAction;

            public ProfileViewEventArgs(ProfileActionType profileViewAction, object source)
                : base(ProfileViewEvent, source)
            {
                this.profileViewAction = profileViewAction;
            }
        }
        #endregion
    }
}