using System.Windows;
using AlienLabs.AlienAdrenaline.App.Classes;
using AlienLabs.AlienAdrenaline.App.Factories;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.App.Views.Xaml;

namespace AlienLabs.AlienAdrenaline.App.Views.Classes
{
    public class ViewFactoryClass : ViewFactory
    {
        public View NewView(ViewType type)
        {
            switch (type)
            {
                case ViewType.Navigation:
                    return NewNavigationView();
                case ViewType.GameMode:                    
                    return NewGameModeView();
                case ViewType.GameModeCreate:
                    return NewGameModeCreateView();
                case ViewType.GameModeCreateAction:
                    return NewGameModeCreateActionView();
                case ViewType.GameModeSaveAs:
                    return NewGameModeSaveAsView();
                case ViewType.GameModeActionSequence:
                    return NewGameModeActionSequenceView();
                case ViewType.GameModeProfileList:
                    return NewGameModeProfileListView();
				case ViewType.RealTimePerformanceMonitoring:
					return NewRealTimePerformanceMonitoringView();
				case ViewType.PerformanceSnapshots:
					return NewPerformanceSnapshotView();
				case ViewType.SnapshopView:
					return NewSnapshopView();
				case ViewType.CategorySelector:
					return NewCategorySelector();
                default:
                    return null;
            }
        }

        public NavigationView NewNavigationView()
        {
            var view = new NavigationControl();
            var presenter = ObjectFactory.NewNavigationPresenter(view);
            view.Presenter = presenter;
            view.Refresh();
            return view;
        }

        public GameModeView NewGameModeView()
        {
            var view = new GameModeControl();
            var presenter = ObjectFactory.NewGameModePresenter(view);
            presenter.Initialize();
            view.Presenter = presenter;
            //view.Refresh();
            return view;
        }

        public GameModeCreateDialog NewGameModeCreateView()
        {
            var view = new GameModeCreateDialog();
            var presenter = ObjectFactory.NewGameModeCreatePresenter(view);
            view.Presenter = presenter;
            view.Refresh();
            return view;
        }

        public GameModeCreateActionDialog NewGameModeCreateActionView()
        {
            var view = new GameModeCreateActionDialog();
            var presenter = ObjectFactory.NewGameModeCreateActionPresenter(view);
            view.Presenter = presenter;
            view.Refresh();
            return view;
        }

        public GameModeSaveAsDialog NewGameModeSaveAsView()
        {
            var view = new GameModeSaveAsDialog();
            var presenter = ObjectFactory.NewGameModeSaveAsPresenter(view);
            view.Presenter = presenter;
            view.Refresh();
            return view;
        }

        public GameModeActionSequenceView NewGameModeActionSequenceView()
        {
            var view = new GameModeActionSequenceControl();
            var presenter = ObjectFactory.NewGameModeActionSequencePresenter(view);
            presenter.Initialize();
            view.Presenter = presenter;
            //view.Refresh();
            return view;
        }

        public GameModeProfileListView NewGameModeProfileListView()
        {
            var view = new GameModeProfileListControl();
            var presenter = ObjectFactory.NewGameModeProfileListPresenter(view);
            view.Presenter = presenter;
            view.Refresh();
            return view;
        }

		public PerformanceMonitoringView NewRealTimePerformanceMonitoringView()
		{
			var view = new PerformanceMonitoringControl(ViewType.RealTimePerformanceMonitoring);
			var presenter = new PerformanceMonitoringPresenter(view)
			{
				EventTrigger = new EventTriggerClass(view)
			};

			view.Presenter = presenter;
			view.Refresh();
			return view;
		}

		public PerformanceSnapshotsView NewPerformanceSnapshotView()
		{
			var view = new PerformanceSnapshotsControl();
			var presenter = new PerformanceSnapshotListPresenter
			{
				View = view,
				EventTrigger = new EventTriggerClass(view)
			};
			presenter.Initialize();

			view.Presenter = presenter;
			view.Refresh();
			return view;
		}

		public PerformanceMonitoringView NewSnapshopView()
		{
			var view = new PerformanceMonitoringControl(ViewType.SnapshopView);
			var presenter = new SnapshotViewPresenter
			{
				View = view,
				EventTrigger = new EventTriggerClass(view)
			};

			view.Presenter = presenter;
			view.Refresh();
			return view;
		}

		public CategorySelectorView NewCategorySelector()
		{
			return new CategorySelectorControl {Visibility = Visibility.Collapsed};
		}	
	}
}