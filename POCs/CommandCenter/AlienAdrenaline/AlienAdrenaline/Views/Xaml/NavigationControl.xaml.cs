#define DEBUG

using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AlienLabs.AlienAdrenaline.App.Classes.Navigation;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.App.Resources;
using AlienLabs.AlienAdrenaline.App.Views.Classes;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using DynamicLightingSupport.Enums;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml
{
    public partial class NavigationControl : NavigationView
    {
        #region NavigationView Members
        public event Action<string> PlugInViewActivated;
        public NavigationPresenter Presenter { get; set; }

        private IList<ViewType> links;
        public IList<ViewType> Links
        {
            get { return links; }
            set
            {
                links = value;
                if (links != null) 
                    ViewLinkSelected = links[0];
            }
        }

        public ViewType ViewLinkSelected { get; set; }
        public Profile GameModeSelected { get; set; }
        public ViewType Type { get { return ViewType.Navigation; } }
        public ObservableCollection<Profile> Profiles { get; set; }
        #endregion

        #region Private Properties
        private NavigationItem gamesNavigationItem;
        #endregion

        public NavigationControl()
        {
            InitializeComponent();
        }

        #region Public Methods
        public void Refresh()
        {
            Presenter.Refresh();

            if (gamesNavigationItem != null)
                createNavigation();
        }

        public void UpdateNavigationPluginLinks(string plugInName)
        {
            createPluginsNavigation(plugInName);
        }

        public void ShowDynamicLightingControl()
        {
            dynamicLightingPanel.Visibility = Visibility.Visible;
        }

        public void HideDynamicLightingControl()
        {
            dynamicLightingPanel.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Private Methods
        private void createNavigation()
        {
            stackPanelViewLinks.Children.Clear();

            var navigation = new Navigation {Margin = new Thickness(0, 0, 0, 0)};

            foreach (var viewlinkItem in links)
            {
                var navItem = createNavigationItem(translationFor(viewlinkItem), viewlinkItem, navigation_Click);                
                navigation.Items.Add(navItem);

                switch (viewlinkItem)
                {
                	case ViewType.GameMode:
                		createGameModeNavigation(navItem);
                        navItem.PreviewMouseLeftButtonDown += gamesNavigationItem_PreviewMouseLeftButtonDown;
                		gamesNavigationItem = navItem;                         
                		break;
					case ViewType.PerformanceMonitoring:
                		navItem.Click -= navigation_Click;
                		createPerformaceMonitoringNavigation(navItem);
						break;
                }                    
            }

            stackPanelViewLinks.Children.Add(navigation);

            updateGamesNavigationSelection();
        }

		private void createGameModeNavigation(NavigationItem navItem)
		{
			navItem.Items.Add(createNavigationItem(Properties.Resources.CreateNewModeText, null, createNewGameMode_Click));

			if (Profiles != null && Profiles.Count > 0)
			{
				navItem.Items.Add(new NavigationSeparator());

				foreach (var profile in Profiles)
				{
					var gameNavItem = createNavigationItem(profile.Name, profile, gameModeNavigation_Click);
					navItem.Items.Add(gameNavItem);
				}
			}
		}

		private void createPerformaceMonitoringNavigation(NavigationItem navItem)
		{
			navItem.Items.Add(createNavigationItem(Properties.Resources.RealTimePerformance, ViewType.RealTimePerformanceMonitoring, realTimePerformanceMode_Click));
			navItem.Items.Add(createNavigationItem(Properties.Resources.PerformanceSnapshots, ViewType.PerformanceSnapshots, performanceSnapshotMode_Click));
		}

    	private NavigationItem createNavigationItem(string header, object tag, RoutedEventHandler routedEventHandler = null)
        {
            var navigationItem = new NavigationItem
            {
                Tag = tag,
                Header = header,
                Foreground = Brushes.White                
            };

            if (routedEventHandler != null)
                navigationItem.Click += routedEventHandler;

            return navigationItem;
        }

        private void createPluginsNavigation(string plugInName)
        {
            stackPanelViewPlugins.Children.Clear();

            var navigation = new Navigation { Margin = new Thickness(0, 0, 0, 0) };
            var navItem = createNavigationItem(plugInName, plugInName, navigationPlugin_Click);
            navigation.Items.Add(navItem);
            stackPanelViewPlugins.Children.Add(navigation);
        }

        private void updateGamesNavigationSelection(bool plugInSelected = false)
        {
            if (!plugInSelected)
                clearPluginsNavigationSelection();

            if (gamesNavigationItem != null)
            {
                var currentProfile = Presenter.GetCurrentProfile();
                foreach (var ctrl in gamesNavigationItem.Items)
                {
                    var navItem = ctrl as NavigationItem;
                    if (navItem != null)
                        navItem.IsChecked = plugInSelected ? false : navItem.Tag != null && navItem.Tag == currentProfile;
                }
            }
        }

        private void clearPluginsNavigationSelection()
        {
            if (stackPanelViewPlugins.Children.Count <= 0)
                return;

            var navigation = stackPanelViewPlugins.Children[0] as Navigation;
            if (navigation != null)
                foreach (var navItem in navigation.Items.Cast<NavigationItem>())
                    navItem.IsChecked = false;
        }

        private string translationFor(ViewType item)
        {
            switch (item)
            {
                case ViewType.GameMode: return Properties.Resources.GameModesLinkText;
                case ViewType.PerformanceMonitoring: 
                case ViewType.RealTimePerformanceMonitoring: return Properties.Resources.PerformanceMonitoringLinkText;
                default: return item.ToString();
            }
        }

        private void createDynamicLightingCanvas()
        {
            try
            {
                //if (SysInfoAPIClass.PCManufacturer.ToLower().Contains("dell"))
                //{
                //    dynamicLightingPanel.Child = new Image() { Source = getSystemImageSource("dell") };
                //    return;
                //}

                var chassisCanvasManager = DynamicLightingSupport.Classes.Factories.ObjectFactory.NewChassisCanvasManager(
                    DynamicLightingCapabilitiesModule.AlienAdrenaline, SysInfoAPIClass.PCModel);

                if (chassisCanvasManager != null &&
                    chassisCanvasManager.CanvasView is UIElement)
                {
                    dynamicLightingPanel.Child = chassisCanvasManager.CanvasView as UIElement;
                    chassisCanvasManager.StartLightingColorsRegistryWatcher();
                    return;
                }

                dynamicLightingPanel.Child = new Image() { Source = getSystemImageSource(SysInfoAPIClass.PCModel) };                
            }
            catch { }
        }

        private ImageSource getSystemImageSource(string model)
        {
            var pathToModel = ModelSystemImageRepository.Instance.GetModelSystemImagePath(model);
            var uriFromPath = new Uri(pathToModel);
            var decoder = new PngBitmapDecoder(uriFromPath, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            return decoder.Frames[0];
        }
        #endregion 

        #region Event Handlers
        private void loading(object sender, RoutedEventArgs e)
        {
            Presenter.Load();
            createNavigation();
            createDynamicLightingCanvas();
        }

        private void navigation_Click(object sender, RoutedEventArgs e)
        {
            var navItem = (NavigationItem)sender;

            ViewLinkSelected = (ViewType)navItem.Tag;
            if (ViewLinkSelected != ViewType.GameMode) 
                Presenter.ChangeViewLinkSelected();      
        }

        private void gameModeNavigation_Click(object sender, RoutedEventArgs e)
        {
            var navItem = (NavigationItem)sender;

            var profile = navItem.Tag as Profile;
            if (profile != null &&
                Presenter.IsCurrentProfile(profile.Name))
                return;

            var parentNavItem = navItem.Parent as NavigationItem;
            if (parentNavItem != null)
                parentNavItem.IsChecked = true;

            ViewLinkSelected = ViewType.GameMode;

            if (Presenter.IsDirty)
            {
                var result = MsgBox.Show(
                    Properties.Resources.ChangeGameModeTitleText, 
                        String.Format(Properties.Resources.WantToSaveGameModeChangesToText, Presenter.GetCurrentProfileName()), MsgBoxIcon.Question, MsgBoxButtons.YesNo);
                if (result == MsgBoxResult.Yes)
                    Presenter.SaveProfile();
                else
                    Presenter.Cancel();
            }

            var currentProfile = navItem.Tag as Profile;
            if (currentProfile != null)
            {
                Presenter.RefreshCurrentProfile(currentProfile);
                Presenter.RefreshViews();

                updateGamesNavigationSelection();
            }
        }

        private void createNewGameMode_Click(object sender, RoutedEventArgs e)
        {
            var result = MsgBoxResult.No;
            if (Presenter.IsDirty)
            {
                result = MsgBox.Show(
                    Properties.Resources.CreateNewModeText,
                        String.Format(Properties.Resources.WantToSaveGameModeChangesToText, Presenter.GetCurrentProfileName()), MsgBoxIcon.Question, MsgBoxButtons.YesNoCancel);
                if (result == MsgBoxResult.Yes)
                {
                    Presenter.SaveProfile();
                }
            }

            if (result != MsgBoxResult.Cancel)
            {
                ViewFactory viewFactory = new ViewFactoryClass();
                var appWindow = viewFactory.NewView(ViewType.GameModeCreate) as Window;
                if (appWindow != null)
                {
                    var dialogResult = appWindow.ShowDialog();
                    if (dialogResult != null && dialogResult.Value)
                    {
                        Presenter.RefreshCurrentProfile();
                        Presenter.RefreshViews();

                        createNavigation();
                    }
                }
            }
        }

        private void gamesNavigationItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Presenter.ActivateGameModeProfileListView();
            updateGamesNavigationSelection();
        }

		private void realTimePerformanceMode_Click(object sender, RoutedEventArgs e)
		{
			ViewLinkSelected = ViewType.RealTimePerformanceMonitoring;
			Presenter.ChangeViewLinkSelected();
		}

		private void performanceSnapshotMode_Click(object sender, RoutedEventArgs e)
		{
			ViewLinkSelected = ViewType.PerformanceSnapshots;
			Presenter.ChangeViewLinkSelected();      
		}

        private void navigationPlugin_Click(object sender, RoutedEventArgs e)
        {
            var navItem = (NavigationItem)sender;
            if (!navItem.IsChecked)
                return;

            var plugInName = navItem.Tag.ToString();
            if (!String.IsNullOrEmpty(plugInName) && PlugInViewActivated != null)
            {
                PlugInViewActivated(plugInName);
                updateGamesNavigationSelection(true);
            }
        }
        #endregion
    }
}