using AlienLabs.AlienAdrenaline.App.Classes;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.App.Views.Classes;
using AlienLabs.AlienAdrenaline.App.Views.Xaml;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes.Factories;
using AlienLabs.GameDiscoveryHelper.Providers.Classes.Factories;

namespace AlienLabs.AlienAdrenaline.App.Factories
{
    public class ObjectFactory
    {
        #region Public Methods
        public static FolderOperations NewFolderOperations()
        {
            return new FolderOperationsClass();
        }

        public static FileOperations NewFileOperations()
        {
            return new FileOperationsClass();
        }

        private static AlienAdrenalinePresenter alienAdrenalinePresenter;
        public static AlienAdrenalinePresenter NewAlienAdrenalinePresenter(AlienAdrenalineView view)
        {
            if (alienAdrenalinePresenter == null)
                alienAdrenalinePresenter = new AlienAdrenalinePresenter 
                {
                    View = view,
                    ViewRepository = newViewRepository(),
                    ProfileRepository = ProfileRepositoryFactory.ProfileRepository,
                    CommandContainer = CommandContainerFactory.CommandContainer,          
                    EventTrigger = new EventTriggerClass(view)
                };

            return alienAdrenalinePresenter;
        }

        public static NavigationPresenter NewNavigationPresenter(NavigationControl view)
        {
            return new NavigationPresenter
            {
                View = view,
                ProfileRepository = ProfileRepositoryFactory.ProfileRepository,
                CommandContainer = CommandContainerFactory.CommandContainer,
                EventTrigger = new EventTriggerClass(view)
            };
        }


        public static GameModePresenter NewGameModePresenter(GameModeView view)
        {
            return new GameModePresenter()
            {
                View = view,
                Model = ServiceFactory.GameModeService,
                ViewRepository = newViewRepository(),
                CommandContainer = CommandContainerFactory.CommandContainer,
                EventTrigger = new EventTriggerClass(view)
            };
        }

        public static GameModeCreatePresenter NewGameModeCreatePresenter(GameModeCreateView view)
        {
            return new GameModeCreatePresenter()
            {
                View = view,
                Model = ServiceFactory.GameModeCreateService
            };
        }

        public static GameModeSaveAsPresenter NewGameModeSaveAsPresenter(GameModeSaveAsView view)
        {
            return new GameModeSaveAsPresenter()
            {
                View = view,
                Model = ServiceFactory.GameModeCreateService
            };
        }

        public static GameModeCreateActionPresenter NewGameModeCreateActionPresenter(GameModeCreateActionView view)
        {
            return new GameModeCreateActionPresenter()
            {
                View = view,
                Model = ServiceFactory.GameModeActionSequenceService,
                ProfileService = ServiceFactory.ProfileService,
                ThermalCapabilities = new ThermalCapabilitiesClass()
            };
        }

        public static GameModeActionSequencePresenter NewGameModeActionSequencePresenter(GameModeActionSequenceView view)
        {
            return new GameModeActionSequencePresenter()
            {
                View = view,
                Model = ServiceFactory.GameModeActionSequenceService,
                ViewRepository = newGameModeActionViewRepository(),
                EventTrigger = new EventTriggerClass(view)
            };           
        }

        public static GameModeProfileListPresenter NewGameModeProfileListPresenter(GameModeProfileListView view)
        {
            return new GameModeProfileListPresenter()
            {
                View = view,
                ProfileRepository = ProfileRepositoryFactory.ProfileRepository,
                GameServiceProvider = GameObjectFactory.NewGameServiceProvider(),
                EventTrigger = new EventTriggerClass(alienAdrenalinePresenter.View)
            };
        }

		public static ApplicationButtonsPresenter NewApplicationButtonsPresenter(ApplicationButtonsView view)
    	{
			return new ApplicationButtonsPresenterClass
			{
				View = view
			};
    	}       
		#endregion

        #region Private Methods
        private static ViewRepository newViewRepository()
        {
            return new ViewRepositoryClass { ViewFactory = new ViewFactoryClass() };
        }

        private static GameModeActionViewRepository newGameModeActionViewRepository()
        {
            return new GameModeActionViewRepositoryClass { ViewFactory = new GameModeActionViewFactoryClass() };
        }
        #endregion
    }
}
