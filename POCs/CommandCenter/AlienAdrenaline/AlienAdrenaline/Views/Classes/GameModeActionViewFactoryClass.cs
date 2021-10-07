using AlienLabs.AlienAdrenaline.App.Classes;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.App.Views.Xaml.Actions;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;

namespace AlienLabs.AlienAdrenaline.App.Views.Classes
{
    public class GameModeActionViewFactoryClass : GameModeActionViewFactory
    {
        public GameModeActionView NewView(GameModeActionViewType type)
        {
            switch (type)
            {
                case GameModeActionViewType.MediaPlayerApplication:
                case GameModeActionViewType.FrapsApplication:
                case GameModeActionViewType.VoIPApplication:
                    return NewApplicationView(type);

                case GameModeActionViewType.GameApplication:
                    return NewGameApplicationView();

                case GameModeActionViewType.AdditionalApplication:
                    return NewAdditionalApplicationView();

                case GameModeActionViewType.AudioOutput:
                    return NewAudioOutputView();

                case GameModeActionViewType.WebLinks:
                    return NewWebLinksView();
                
                case GameModeActionViewType.PowerPlan:
                    return NewPowerPlanView();

                case GameModeActionViewType.Thermal:
                    return NewThermalView();

                case GameModeActionViewType.AlienFX:
                    return NewAlienFXView();

                case GameModeActionViewType.EnergyBooster:
                    return NewEnergyBoosterView();

                case GameModeActionViewType.PerformanceMonitoring:
                    return NewPerformanceMonitoringView();

                case GameModeActionViewType.Summary:
                    return NewSummaryView();
                default:
                    return null;
            }
        }

        public virtual GameModeActionView NewAudioOutputView()
        {
            var view = new AudioOutputUserControl();
            var presenter = new GameModeActionAudioOutputPresenter
            {
                View = view,                
                Model = ServiceFactory.GameModeActionSequenceService,
                AudioDeviceService = new AudioDeviceServiceClass(),
                EventTrigger = new EventTriggerClass(view)
            };
            view.Presenter = presenter;
            view.Refresh();
            return view;
        }

        public virtual GameModeActionView NewWebLinksView()
        {
            var view = new WebLinksUserControl();
            var presenter = new GameModeActionWebLinksPresenter
            {
                View = view,
                Model = ServiceFactory.GameModeActionSequenceService,
                EventTrigger = new EventTriggerClass(view)
            };
            presenter.Initialize();
            view.Presenter = presenter;
            view.Refresh();
            return view;
        }

        public virtual GameModeActionView NewApplicationView(GameModeActionViewType type)
        {
            var view = new ApplicationUserControl() { ActionType = type };
            var presenter = new GameModeActionApplicationPresenter
            {
                View = view,
                Model = ServiceFactory.GameModeActionSequenceService,
                EventTrigger = new EventTriggerClass(view)
            };
            presenter.Initialize();
            view.Presenter = presenter;
            view.Refresh();
            return view;
        }

        public virtual GameModeActionView NewGameApplicationView()
        {
            var view = new GameApplicationUserControl();
            var presenter = new GameModeActionGameApplicationPresenter
            {
                View = view,
                Model = ServiceFactory.GameModeActionSequenceService,
                //GameModeService = ServiceFactory.GameModeService,                
                EventTrigger = new EventTriggerClass(view)
            };
            presenter.Initialize();
            view.Presenter = presenter;
            view.Refresh();
            return view;
        }

        public virtual GameModeActionView NewAdditionalApplicationView()
        {
            var view = new AdditionalApplicationUserControl();
            var presenter = new GameModeActionAdditionalApplicationPresenter
            {
                View = view,
                Model = ServiceFactory.GameModeActionSequenceService,
                EventTrigger = new EventTriggerClass(view)
            };
            presenter.Initialize();
            view.Presenter = presenter;
            view.Refresh();
            return view;
        }

        public virtual GameModeActionView NewPowerPlanView()
        {
            var view = new PowerPlanUserControl();
            var presenter = new GameModeActionPowerPlanPresenter
            {
                View = view,
                Model = ServiceFactory.GameModeActionSequenceService,
                PowerPlanService = ServiceFactory.PowerPlanService,
                EventTrigger = new EventTriggerClass(view)
            };
            presenter.Initialize();
            view.Presenter = presenter;
            view.Refresh();
            return view;
        }

        public virtual GameModeActionView NewThermalView()
        {
            var view = new ThermalUserControl();
            var presenter = new GameModeActionThermalPresenter
            {
                View = view,
                Model = ServiceFactory.GameModeActionSequenceService,
                ThermalProfileService = ServiceFactory.ThermalProfileService,
                EventTrigger = new EventTriggerClass(view)
            };
            presenter.Initialize();
            view.Presenter = presenter;
            view.Refresh();
            return view;
        }

        public virtual GameModeActionView NewAlienFXView()
        {
            var view = new AlienFXUserControl();
            var presenter = new GameModeActionAlienFXPresenter
            {
                View = view,
                Model = ServiceFactory.GameModeActionSequenceService,
                AlienFXThemeService = ServiceFactory.AlienFXThemeService,
                EventTrigger = new EventTriggerClass(view)
            };
            presenter.Initialize();
            view.Presenter = presenter;
            view.Refresh();
            return view;
        }

        public virtual GameModeActionView NewEnergyBoosterView()
        {
            var view = new EnergyBoosterUserControl();
            var presenter = new GameModeActionEnergyBoosterPresenter
            {
                View = view,
                Model = ServiceFactory.GameModeActionSequenceService,
                EnergyBoosterService = ServiceFactory.EnergyBoosterService,
                EventTrigger = new EventTriggerClass(view)
            };
            presenter.Initialize();
            view.Presenter = presenter;
            view.Refresh();
            return view;
        }

        public virtual GameModeActionView NewPerformanceMonitoringView()
        {
            var view = new PerformanceMonitoringUserControl();
            var presenter = new GameModeActionPerformanceMonitoringPresenter
            {
                View = view,
                Model = ServiceFactory.GameModeActionSequenceService,
                //PerformanceMonitoringService = ServiceFactory.PerformanceMonitoringService,
                EventTrigger = new EventTriggerClass(view)
            };
            presenter.Initialize();
            view.Presenter = presenter;
            view.Refresh();
            return view;
        }

        public virtual GameModeActionView NewSummaryView()
        {
            var view = new SummaryUserControl();
            var presenter = new GameModeActionSummaryPresenter()
            {
                View = view,
                Model = ServiceFactory.GameModeActionSummaryService
            };
            presenter.Initialize();
            view.Presenter = presenter;
            view.Refresh();
            return view;
        }
    }
}