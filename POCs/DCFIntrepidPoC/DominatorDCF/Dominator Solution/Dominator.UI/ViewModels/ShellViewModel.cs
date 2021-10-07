using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Dominator.Domain.Classes.Factories;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.Tools.Helpers;
using Dominator.UI.Classes;
using Dominator.UI.Classes.Enums;
using Dominator.UI.Classes.Helpers;
using Dominator.UI.Views;

namespace Dominator.UI.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        #region Properties
        private ViewModelBase viewModel;
        public ViewModelBase ViewModel
        {
            get { return viewModel; }
            set { SetProperty(ref viewModel, value, "ViewModel"); }
        }

        private IViewWithDataContextAndVisibility view;
        public IViewWithDataContextAndVisibility View
        {
            get { return view; }
            set
            {
                SetProperty(ref view, value, "View");
                if (!DesignerProperties.IsInDesignMode)
                    view.DataContext = this;
            }
        }

        private readonly Dictionary<ViewTypes, ViewModelBase> views = new Dictionary<ViewTypes, ViewModelBase>();
        private ErrorCode errorCode;
        #endregion

        #region Methods
        public override async void Initialize()
        {
            //XTULibraryLoader.TryLoadAssemblies();

            errorCode = checkForMultipleInstances();
            if (errorCode != ErrorCode.None)
                return;                       

            errorCode = checkDominatorService();
            if (errorCode != ErrorCode.None)
                return;

            errorCode = checkBIOSSupport();
            if (errorCode != ErrorCode.None)
                return;

            if (errorCode == ErrorCode.None)
                await SystemInfoRepository.GetInstanceAsync();            
        }

        public void HostStartupView()
        {
            ViewModel = new StartupOCViewModel
            {
                View = new StartupOCView(),
                NavigationViewModel = this
            };
        }

        public void HostView(ViewTypes viewType)
        {
            if (errorCode == ErrorCode.None)
                errorCode = checkDominatorService();

            if (errorCode != ErrorCode.None)
                viewType = ViewTypes.MissingComponent;

            ViewModel?.Unload();
            if (views.ContainsKey(viewType))
                ViewModel = views[viewType];
            else
            {
                switch (viewType)
                {
                    case ViewTypes.Main:
                        ViewModel = new MainOCViewModel
                        {
                            View = new MainOCView(),
                            NavigationViewModel = this
                        };

                        ((MainOCViewModel)ViewModel).ProfileNavigationViewModel = new ProfileNavigationViewModel
                        {
                            View = new ProfileNavigationView(),
                            Model = OverclockingFactory.NewOverclockingModel(),
                            AdvancedOCModel = AdvancedOCFactory.NewAdvancedOCModel(),
                            NavigationViewModel = this,
                            HostViewModel = ViewModel
                        };

                        ((MainOCViewModel)ViewModel).ProfileDetailsViewModel = new ProfileDetailsViewModel
                        {
                            View = new ProfileDetailsView(),
                            Model = OverclockingFactory.NewOverclockingModel(),
                            NavigationViewModel = this,
                            HostViewModel = ViewModel
                        };

                        break;

                    case ViewTypes.Advanced:                       
                        ViewModel = new AdvancedOCViewModel
                        {
                            View = new AdvancedOCView(),
                            AdvancedOCModel = AdvancedOCFactory.NewAdvancedOCModel(),
                            ProfileNameRepository  = ProfileFactory.NewProfileNameRepository(),
                            NavigationViewModel = this
                        };

                        ((AdvancedOCViewModel)ViewModel).CategoryNavigationViewModel = new CategoryNavigationViewModel
                        {
                            View = new CategoryNavigationView(),
                            Model = OverclockingFactory.NewOverclockingModel(),
                            NavigationViewModel = this,
                            HostViewModel = ViewModel
                        };
                        break;

                    case ViewTypes.MissingComponent:
                        ViewModel = new MissingComponentViewModel(errorCode)
                        {
                            View = new MissingComponentView(),
                            NavigationViewModel = this,
                            MsgTitle = Properties.Resources.ApplicationName.ToUpper()
                        };
                        break;
                }

                if (ViewModel != null)
                {
                    if (viewType != ViewTypes.MissingComponent)
                        ViewModel.Model = OverclockingFactory.NewOverclockingModel();

                    ViewModel.Initialize();      
                    
                    if (viewType != ViewTypes.Advanced)              
                        views[viewType] = ViewModel;
                }
            }

            ViewModel?.Load();
        }

        private ErrorCode checkDominatorService()
        {
            if (!DominatorWindowsServiceHelper.IsServiceInstalled())
                return ErrorCode.ServiceNotFound;

            if (DominatorWindowsServiceHelper.IsRunning())
                return ErrorCode.None;

            var result = new Notifier().Show(Properties.Resources.ApplicationName.ToUpper(), Properties.Resources.ServiceNotRunningQuestion, NotifierIcon.Question, NotifierButtons.YesNo);
            if (result == NotifierResult.Yes)
                DominatorWindowsServiceHelper.Start();

            if (!DominatorWindowsServiceHelper.IsRunning())
                return ErrorCode.ServiceNotRunning;

            return ErrorCode.None;
        }

        private static ErrorCode checkForMultipleInstances()
        {
            if (SingleApplicationDetector.InstanceWasCreated)
                return ErrorCode.None;

            return SingleApplicationDetector.IsRunning() ? ErrorCode.AnotherInstanceIsRunning : ErrorCode.None;
        }

        private ErrorCode checkBIOSSupport()
        {
            var shellModel = ShellModelFactory.NewShellModel();
            if (shellModel.IsBIOSInterfaceSupported) return ErrorCode.None;

            if (new Notifier().Show(
                Properties.Resources.ApplicationName.ToUpper(), string.Format(Properties.Resources.BIOSInterfaceNotSupportedLaunchSupportDellMessage, Properties.Resources.ApplicationName), NotifierIcon.Question, NotifierButtons.YesNo) == NotifierResult.Yes)
                Process.Start("http://support.dell.com");

            return ErrorCode.BIOSInterfaceNotSupported;
        }

        public void UnloadViews()
        {
            errorCode = ErrorCode.None;

            SingleApplicationDetector.Close();

            ViewModel.Unload();
            views.Clear();
            ViewModel = null;
        }
        #endregion
    }
}
