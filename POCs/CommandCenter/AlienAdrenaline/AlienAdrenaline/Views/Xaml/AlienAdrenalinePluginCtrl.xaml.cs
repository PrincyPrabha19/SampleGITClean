using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using AlienLabs.AlienAdrenaline.App.Classes;
using AlienLabs.AlienAdrenaline.App.Classes.PlugIn;
using AlienLabs.AlienAdrenaline.App.Factories;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.Tools;
using AlienLabs.CC_PlugIn;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml
{
    public partial class AlienAdrenalinePluginCtrl : AlienAdrenalineView
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

        #region AlienAdrenalineView Properties
        public AlienAdrenalinePresenter Presenter { get; set; }

        public NavigationView NavigationView
        {
            get { return NavigationViewPlaceholder.Child as NavigationView; }
            set { NavigationViewPlaceholder.Child = value as UIElement; }
        }

        public ContentView ActiveView
        {
            get { return ActiveViewPlaceholder.Child as ContentView; }
            set { ActiveViewPlaceholder.Child = value as UIElement; }
        }

        public UserControl ActivePluginView
        {
            get { return ActiveViewPlaceholder.Child as UserControl; }
            set { ActiveViewPlaceholder.Child = value; }
        }

    	public ApplicationButtonsView ApplicationButtonsView
    	{
			get { return appButtonsControl; }
    	}

	    public CategorySelectorView CategorySelectorView
		{
			get { return CategorySelectorPlaceHolder.Child as CategorySelectorView; }
			set { CategorySelectorPlaceHolder.Child = value as UIElement; }
		}

	    private bool isDirty;
        public bool IsDirty
        {
            get { return isDirty; }
            set
            {
                if (isDirty != value)
                {
                    isDirty = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("IsDirty"));
                }
            }
        }

        private bool isDeleteEnabled;
        public bool IsDeleteEnabled
        {
            get { return isDeleteEnabled; }
            set
            {
                if (isDeleteEnabled != value)
                {
                    isDeleteEnabled = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("IsDeleteEnabled"));
                }
            }
        }
        #endregion

        #region Private Properties
        private GraphicsAmplifierPlugInManager _graphicsAmplifierPlugInManager;
        #endregion

        #region Constructors
        public AlienAdrenalinePluginCtrl()
        {
            initialize();
            loadPlugIns();
        }

        public AlienAdrenalinePluginCtrl(ICommandCenterPlugIn plugIn) : base(plugIn)
        {           
            initialize();
            loadPlugIns();
        }
        #endregion

        #region Private Methods
        private void initialize()
        {
            InitializeComponent();

            //if (GeneralSettings.IsInDesignMode)
            //    return;

            Presenter = ObjectFactory.NewAlienAdrenalinePresenter(this);            
            addCustomEventHandlers();            
        }

        private void loadPlugIns()
        {
            _graphicsAmplifierPlugInManager = new GraphicsAmplifierPlugInManager();
            _graphicsAmplifierPlugInManager.LoadPlugIn();
        }

        private void addCustomEventHandlers()
        {
            AddHandler(CustomRoutedEvents.CommandEvent, new RoutedEventHandler(addCommand), true);
            AddHandler(CustomRoutedEvents.ViewActivatedEvent, new RoutedEventHandler(activateView), true);
            AddHandler(CustomRoutedEvents.ProfileViewEvent, new RoutedEventHandler(profileViewAction), true);
        }
        #endregion

        #region Event Handlers
        private void addCommand(object sender, RoutedEventArgs e)
        {
            var args = e as CustomRoutedEvents.CommandEventArgs;
            if (args != null)
                Presenter.Receive(args.equatableCommand);
        }

        private void activateView(object sender, RoutedEventArgs e)
        {           
            var args = e as CustomRoutedEvents.ViewActivatedEventArgs;
            if (args != null)
                Presenter.Activate(args.ViewType);
        }

        private void profileViewAction(object sender, RoutedEventArgs e)
        {
            var args = e as CustomRoutedEvents.ProfileViewEventArgs;
            if (args != null)
                Presenter.ProfileViewAction(args.profileViewAction);
        }

        public override bool Close(bool force)
        {
            if (Presenter != null)
                Presenter.ShutdownServices();
            return true;
        }

        private void loaded(object sender, RoutedEventArgs e)
        {
            if (GeneralSettings.IsInDesignMode)
                return;

            //Presenter.InitializeGameServiceProvider(this);

            Presenter.Load();

            NavigationView.PlugInViewActivated += navigationView_PlugInViewActivated;
            if (_graphicsAmplifierPlugInManager.IsPlugInInstalled)
                NavigationView.UpdateNavigationPluginLinks(_graphicsAmplifierPlugInManager.PlugInName);
        }

        private void navigationView_PlugInViewActivated(string plugInName)
        {
            if (_graphicsAmplifierPlugInManager.IsPlugInInstalled && 
                String.Compare(plugInName, _graphicsAmplifierPlugInManager.PlugInName, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                _graphicsAmplifierPlugInManager.LaunchApplication(_graphicsAmplifierPlugInManager.PlugIn);                
                Presenter.ActivatePlugin(_graphicsAmplifierPlugInManager.PlugIn.View);
            }
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            Presenter.ShowHelp();
        }
        #endregion
    }
}