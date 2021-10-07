
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using AlienLabs.AlienAdrenaline.App.Helpers;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.App.Views.Classes;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml
{
    /// <summary>
    /// Interaction logic for GameModeProfileListControl.xaml
    /// </summary>
    public partial class GameModeProfileListControl : GameModeProfileListView
    {
        #region GameModeProfileListView Members
        public GameModeProfileListPresenter Presenter { get; set; }
        public ViewType Type { get { return ViewType.GameModeProfileList; } }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ObservableCollection<Profile> Profiles { get; set; }

        public void Refresh()
        {
            Presenter.Refresh();
        }

        public void SetProfileListItemsSource()
        {            
            listBoxProfiles.ItemsSource = Profiles;
        }
        #endregion

        #region Constructor
        public GameModeProfileListControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        private void listBoxProfiles_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (listBoxProfiles.SelectedItem != null)
            {
                var currentProfile = listBoxProfiles.SelectedItem as Profile;
                if (currentProfile != null)
                {
                    if (!Presenter.IsCreateNewProfileSelected(currentProfile))
                        Presenter.RefreshCurrentProfile(currentProfile);
                    else
                    {
                        currentProfile = null;

                        ViewFactory viewFactory = new ViewFactoryClass();
                        var appWindow = viewFactory.NewView(ViewType.GameModeCreate) as Window;
                        if (appWindow != null)
                        {
                            var dialogResult = appWindow.ShowDialog();
                            if (dialogResult != null && dialogResult.Value)
                            {
                                Presenter.RefreshCurrentProfile();

                                //var view = appWindow as GameModeCreateView;
                                //if (view != null)
                                //    currentProfile = view.ProfileCreated;
                            }
                        }
                    }

                    Presenter.RefreshViews();                        
                }
            }
        }
        #endregion
    }
}
