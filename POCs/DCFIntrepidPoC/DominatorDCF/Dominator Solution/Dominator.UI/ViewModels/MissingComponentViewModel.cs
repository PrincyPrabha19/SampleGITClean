using System;
using System.Windows;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.UI.Classes.Enums;
using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.ViewModels
{
    public class MissingComponentViewModel : ViewModelBase
    {
        #region Properties
        private IViewWithDataContextAndVisibility view;
        public IViewWithDataContextAndVisibility View
        {
            get { return view; }
            set
            {
                if (view == value) return;

                view = value;
                if (!DesignerProperties.IsInDesignMode)
                    view.DataContext = this;
            }
        }

        private ErrorCode errorType = ErrorCode.None;

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value, "ErrorMessage"); }
        }

        private string msgTitle;
        public string MsgTitle
        {
            get { return msgTitle; }
            set { SetProperty(ref msgTitle, value, "MsgTitle"); }
        }

        private Visibility showRestartService;
        public Visibility ShowRestartService
        {
            get { return showRestartService; }
            set
            {
                SetProperty(ref showRestartService, value, "ShowRestartService");
            }
        }

        public RelayCommand<object> StartServiceCommand { get; set; }
        #endregion

        public MissingComponentViewModel(ErrorCode errorType)
        {
            setLayout(errorType);
            initializeCommands();
        }

        #region Methods
        private void setLayout(ErrorCode error)
        {
            errorType = error;
            switch (errorType)
            {
                case ErrorCode.ServiceNotFound:
                    ErrorMessage = Properties.Resources.ServiceNotFoundErrorMsg;
                    break;
                case ErrorCode.ServiceNotRunning:
                    ErrorMessage = Properties.Resources.ServiceNotRunningErrorMsg;
                    break;
                case ErrorCode.AnotherInstanceIsRunning:
                    ErrorMessage = Properties.Resources.AnotherInstanceIsRunningErrorMsg;
                    break;
                case ErrorCode.BIOSInterfaceNotSupported:
                    ErrorMessage = Properties.Resources.BIOSUpdateRequiredErrorMsg;
                    break;
                default:
                    ErrorMessage = "";
                    break;
            }

            ShowRestartService = errorType == ErrorCode.ServiceNotRunning ? Visibility.Visible : Visibility.Collapsed;
        }

        private void initializeCommands()
        {
            StartServiceCommand = new RelayCommand<object>(executeStartService, canExecuteStartService);
        }

        private void executeStartService(object obj)
        {
            DominatorWindowsServiceHelper.Start();
            NavigationViewModel.Initialize();
            NavigationViewModel.HostView(ViewTypes.Main);
        }

        private bool canExecuteStartService(object obj)
        {
            return errorType == ErrorCode.ServiceNotRunning;
        }
        #endregion
    }
}