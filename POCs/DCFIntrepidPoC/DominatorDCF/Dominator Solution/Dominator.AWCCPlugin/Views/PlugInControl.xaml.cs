using System.Windows;
using System.Threading;
using AlienLabs.CC_PlugIn;
using Dominator.UI.Classes.Enums;
using Dominator.UI.ViewModels;
using Dominator.UI.Views;


namespace OCControls.AWCCPlugin.Views
{
    public partial class PlugInControl
    {
        #region Properties
        private static ShellViewModel shellViewModel;
        #endregion

        #region Constructor
        public PlugInControl()
        {
            initialize();
        }

        public PlugInControl(ICommandCenterPlugIn plugIn): base(plugIn)
        {
            initialize();
        }
        #endregion

        #region Methods
        private void initialize()
        {
            InitializeComponent();

            shellViewModel = new ShellViewModel { View = new ShellView() };
            shellViewModel.HostStartupView();
            pluginContainer.Child = (UIElement)shellViewModel.View;

            Dispatcher.InvokeAsync(delegate ()
            {
                shellViewModel.Initialize();
                shellViewModel.HostView(ViewTypes.Main);
            });
        }

        private void unloadViews()
        {
            shellViewModel.UnloadViews();
        }
        #endregion

        #region Overriding Methods
        public override bool Close(bool force)
        {
            unloadViews();
		    return true;
		}
        #endregion
    }
}
