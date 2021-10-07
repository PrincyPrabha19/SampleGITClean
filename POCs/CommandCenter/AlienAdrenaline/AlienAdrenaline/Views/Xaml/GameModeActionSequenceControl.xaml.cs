
using System;
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
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml
{
    /// <summary>
    /// Interaction logic for GameModeActionSequenceControl.xaml
    /// </summary>
    public partial class GameModeActionSequenceControl : GameModeActionSequenceView
    {
        #region GameModeActionListView Members
        public GameModeActionSequencePresenter Presenter { get; set; }
        public ViewType Type { get { return ViewType.GameModeActionSequence; } }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public GameModeProfileActions GameModeProfileActions { get; set; }

        public GameModeActionContentView ActionDetailsView
        {
            get { return borderActionDetails.Child as GameModeActionContentView; }
            set
            {
                borderActionDetails.Child = value as UIElement;
                value.ProfileAction_Changed += profileAction_Changed;
            }
        }

        private string currentProfileActionTitle;
        public string CurrentProfileActionTitle
        {
            get { return currentProfileActionTitle; }
            set
            {
                currentProfileActionTitle = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CurrentProfileActionTitle"));
            }
        }

        private byte[] currentProfileActionImage;
        public byte[] CurrentProfileActionImage
        {
            get { return currentProfileActionImage; }
            set
            {
                currentProfileActionImage = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CurrentProfileActionImage"));
            }
        }  

        public void Refresh()
        {
            Presenter.Refresh();
        }

        public void SetActionSequenceItemsSource()
        {
            listBoxActions.ItemsSource = GameModeProfileActions.ProfileActions;
        }

        public void ShowSummaryLink(bool show)
        {
            linkSummary.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion

        #region Public Properties
        private bool isMoveUpEnabled;
        public bool IsMoveUpEnabled
        {
            get { return isMoveUpEnabled; }
            set
            {
                isMoveUpEnabled = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsMoveUpEnabled"));
            }
        }

        private bool isMoveDownEnabled;
        public bool IsMoveDownEnabled
        {
            get { return isMoveDownEnabled; }
            set
            {
                isMoveDownEnabled = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsMoveDownEnabled"));
            }
        }
        #endregion

        #region Constructor
        public GameModeActionSequenceControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Private Methods
        private void updateNavigationButtons()
        {
            IsMoveUpEnabled = canMoveUp();
            IsMoveDownEnabled = canMoveDown();
        }

        private bool canMoveUp()
        {
            int index = listBoxActions.SelectedIndex;
            return index > 0 && canRelocate(index) && canRelocate(index - 1);
        }

        private bool canMoveDown()
        {
            int index = listBoxActions.SelectedIndex;
            if (index >= 0 && index < listBoxActions.Items.Count && canRelocate(index) && canRelocate(index + 1))
                return true;

            return false;
        }

        private bool canRelocate(int index)
        {
            if (index >= 0 && index < listBoxActions.Items.Count)
            {
                var profileAction = listBoxActions.Items[index] as ProfileAction;
                if (profileAction != null)
                    return EnumHelper.GetAttributeValue<AllowRelocationAttributeClass, bool>(profileAction.Type);
            }

            return false;
        }

        private bool canDelete(int index)
        {
            if (index >= 0 && index <= listBoxActions.Items.Count - 1)
            {
                var profileAction = listBoxActions.Items[index] as ProfileAction;
                if (profileAction != null)
                    return EnumHelper.GetAttributeValue<AllowDeletionAttributeClass, bool>(profileAction.Type);
            }

            return false;
        }

        private void refreshGameApplicationAction()
        {
            if (listBoxActions.SelectedIndex == -1) return;
            var profileAction = listBoxActions.Items[listBoxActions.SelectedIndex] as ProfileAction;
            if (profileAction == null) return;

            var lbi = listBoxActions.ItemContainerGenerator.ContainerFromIndex(listBoxActions.SelectedIndex) as ListBoxItem;
            if (lbi == null) return;

            var grid = VisualTreeHelper.GetChild(lbi, 0) as Grid;
            if (grid == null) return;

            var image = grid.FindName("imageConfigMissing") as Image;
            if (image != null)
                image.Visibility = profileAction.ProfileActionInfo.GetStatus() == ProfileActionStatus.NotReady ? Visibility.Visible : Visibility.Hidden;
        }
        #endregion

        #region Event Handlers
        private void buttonAction_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as ToggleButton;
            if (button != null &&
                button.IsChecked != null)
            {
                var lbi = button.TryFindParent<ListBoxItem>();
                if (lbi != null)
                    lbi.IsSelected = Convert.ToBoolean(button.IsChecked);
            }

            updateNavigationButtons();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var profileAction = button.Tag as ProfileAction;
                if (profileAction != null)
                {
                    var index = listBoxActions.Items.IndexOf(profileAction);
                    if (canDelete(index))
                        Presenter.DeleteProfileAction(profileAction);
                }
            }

            updateNavigationButtons();
        }

        private void buttonMoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (GameModeProfileActions != null && canMoveUp())
            {
                var index = listBoxActions.SelectedIndex;

                var profileAction = GameModeProfileActions[index];

                Presenter.RelocateProfileAction(profileAction, false);

                listBoxActions.SelectedItem = profileAction;
                listBoxActions.ScrollIntoView(listBoxActions.SelectedItem);
            }

            updateNavigationButtons();
        }

        private void buttonMoveDown_Click(object sender, RoutedEventArgs e)
        {
            if (GameModeProfileActions != null && canMoveDown())
            {
                var index = listBoxActions.SelectedIndex;

                var profileAction = GameModeProfileActions[index];

                Presenter.RelocateProfileAction(profileAction, true);

                listBoxActions.SelectedItem = profileAction;
                listBoxActions.ScrollIntoView(listBoxActions.SelectedItem);
            }

            updateNavigationButtons();
        }

        private void buttonAddAction_Click(object sender, RoutedEventArgs e)
        {
            ViewFactory viewFactory = new ViewFactoryClass();
            var appWindow = viewFactory.NewView(ViewType.GameModeCreateAction) as Window;
            if (appWindow != null)
            {
                var result = appWindow.ShowDialog();
                if (result != null && result.Value)
                {
                    var appWindowView = appWindow as GameModeCreateActionView;
                    if (appWindowView != null)
                    {
                        Presenter.AddProfileAction(appWindowView.ProfileAction);

                        listBoxActions.SelectedItem = appWindowView.ProfileAction;
                        listBoxActions.ScrollIntoView(listBoxActions.SelectedItem);
                    }
                }
            }
        }

        private void listBoxActions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProfileAction profileActionSelected = null;

            if (listBoxActions.SelectedIndex != -1)
                profileActionSelected = GameModeProfileActions[listBoxActions.SelectedIndex];

            Presenter.SelectProfileAction(profileActionSelected);                

            updateNavigationButtons();
        }

        private void linkSummary_Click(object sender, RoutedEventArgs e)
        {
            listBoxActions.SelectedIndex = -1;
        }

        private void profileAction_Changed()
        {
            Presenter.RefreshProfileActionNameAndImage();
            Presenter.RefreshGameApplicationAction();
            refreshGameApplicationAction();
        }
        #endregion
    }
}
