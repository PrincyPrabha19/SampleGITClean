
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AlienLabs.AlienAdrenaline.App.Helpers;
using AlienLabs.AlienAdrenaline.App.Presenters;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml.Actions
{
    /// <summary>
    /// Interaction logic for WebLinks.xaml
    /// </summary>
    public partial class WebLinksUserControl : GameModeActionWebLinksView
    {
        #region GameModeActionWebLinksView Members
        public GameModeActionViewType ActionType { get { return GameModeActionViewType.AudioOutput; } }
        public GameModeActionWebLinksPresenter Presenter { get; set; }
        public ObservableCollection<string> Urls { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event Action ProfileAction_Changed;

        public bool EnableTabbedBrowsing
        {
            get { return checkBoxUseUseMultiTabbed.IsChecked.Value; }
            set { checkBoxUseUseMultiTabbed.IsChecked = value; }
        }

        public void Refresh()
        {
            Presenter.Refresh();
        }

        public void SetWebLinksItemsSource()
        {
            listBoxWebLinks.Items.Clear();
            listBoxWebLinks.ItemsSource = Urls;
            updateListBoxItemButtonsVisibility();
        }

        public void ScrollIntoView(int index)
        {
            if (index < listBoxWebLinks.Items.Count)
                listBoxWebLinks.SelectedIndex = index;
        }
        #endregion

        #region Constructors
        public WebLinksUserControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var lbi = button.TryFindParent<ListBoxItem>();
                if (lbi != null)
                {
                    var index = findListBoxItemIndex(lbi);
                    if (index != -1)
                    {
                        Presenter.DeleteWebLink(index);
                        updateListBoxItemButtonsVisibility();

                        if (ProfileAction_Changed != null)
                            ProfileAction_Changed();
                    }
                }                
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                Presenter.AddWebLink();
                updateListBoxItemButtonsVisibility();

                if (ProfileAction_Changed != null)
                    ProfileAction_Changed();
            }
        }

        private void textBoxLink_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                var lbi = textBox.TryFindParent<ListBoxItem>();
                if (lbi != null)
                {
                    var index = findListBoxItemIndex(lbi);
                    if (index != -1)
                    {
                        var stackPanelError = lbi.Template.FindName("stackPanelApplicationError", lbi) as UIElement;
                        if (stackPanelError != null)
                        {
                            stackPanelError.Visibility = Visibility.Collapsed;

                            if (!Presenter.IsValidWebLink(textBox.Text))
                            {
                                stackPanelError.Visibility = Visibility.Visible;
                                return;
                            }
                        }

                        Presenter.UpdateWebLink(textBox.Text, index);

                        if (ProfileAction_Changed != null)
                            ProfileAction_Changed();
                    }                    
                }
            }
        }

        private void checkBoxUseUseMultiTabbed_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Presenter.ToggleEnableTabbedBrowsing();
        }
        #endregion

        #region Private Methods
        private int findListBoxItemIndex(ListBoxItem listBoxItem)
        {
            var view = ItemsControl.ItemsControlFromItemContainer(listBoxItem) as ListBox;

            if (view != null)
                return view.ItemContainerGenerator.IndexFromContainer(listBoxItem);
            return -1;
        }

        private void updateListBoxItemButtonsVisibility()
        {
            for (int i=0; i < listBoxWebLinks.Items.Count; i++)
            {
                var lbi = listBoxWebLinks.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                if (lbi != null)
                {
                    var grid = VisualTreeHelper.GetChild(lbi, 0) as Grid;
                    if (grid != null)
                    {
                        var buttonAdd = grid.FindName("buttonAdd") as Button;
                        if (buttonAdd != null)
                            buttonAdd.Visibility = (i >= listBoxWebLinks.Items.Count - 1) ? Visibility.Visible : Visibility.Hidden;

                        var textBlockWebLink = grid.FindName("textBlockWebLink") as TextBlock;
                        if (textBlockWebLink != null)
                            textBlockWebLink.Text = String.Format(Properties.Resources.WebLinkItemizedText, i + 1);
                    }
                }
            }                      
        }
        #endregion
    }
}
