using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.GameModeProcessor.Presenters;
using AlienLabs.Tools;

namespace AlienLabs.AlienAdrenaline.GameModeProcessor.Views.Xaml
{
    public partial class Main : MainView
    {
        #region Private Properties
        private delegate void ScrollIntoViewDelegate(int index);
        private delegate void SetWindowStateDelegate(WindowState windowState);
        private delegate void CloseWindowDelegate();
        #endregion

        #region MainView Members
        public MainPresenter Presenter { get; set; }       
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ObservableCollection<GameModeActionSummaryData> GameModeActionSummaryDataList { get; set; }
        public bool RollBackActionEnded { get; set; }
        
        private string gameModeName;
        public string GameModeName
        {
            get { return gameModeName; }
            set
            {
                gameModeName = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GameModeName"));
            }
        }

        private string gameModeActionExecutionSummary;
        public string GameModeActionExecutionSummary
        {
            get { return gameModeActionExecutionSummary; }
            set
            {
                gameModeActionExecutionSummary = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GameModeActionExecutionSummary"));
            }
        }

        private bool gameRealPathNotFound;
        public bool GameRealPathNotFound
        {
            get { return gameRealPathNotFound; }
            set
            {
                gameRealPathNotFound = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GameRealPathNotFound"));
            }
        }

        private bool gameApplicationNotEngaged;
        public bool GameApplicationNotEngaged
        {
            get { return gameApplicationNotEngaged; }
            set
            {
                gameApplicationNotEngaged = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("GameApplicationNotEngaged"));
            }
        }

        public void SetActionSummaryItemsSource()
        {
            listBoxActionSummary.Items.Clear();
            listBoxActionSummary.ItemsSource = GameModeActionSummaryDataList;
        }

        public void ScrollIntoView(int index)
        {
            if (listBoxActionSummary.Dispatcher.CheckAccess())
            {
                scrollIntoView(index);
                return;
            }

            listBoxActionSummary.Dispatcher.Invoke(new ScrollIntoViewDelegate(scrollIntoView), index);
        }

        public void MinimizeWindow()
        {
            if (Dispatcher.CheckAccess())
            {
                setWindowState(WindowState.Minimized);
                return;
            }

            Dispatcher.Invoke(new SetWindowStateDelegate(setWindowState), WindowState.Minimized);
        }

        public void RestoreWindow()
        {
            if (Dispatcher.CheckAccess())
            {
                setWindowState(WindowState.Normal);
                return;
            }

            Dispatcher.Invoke(new SetWindowStateDelegate(setWindowState), WindowState.Normal);
        }

        public void CloseWindow()
        {
            if (Dispatcher.CheckAccess())
            {
                closeWindow();
                return;
            }

            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new CloseWindowDelegate(closeWindow));
        }
        #endregion

        #region Constructors
        public Main()
        {
            InitializeComponent();
        }
        #endregion

        #region Private Methods
        private void scrollIntoView(int index)
        {
            listBoxActionSummary.SelectedIndex = index;
        }

        private void setWindowPosition()
        {
            Left = SystemParameters.PrimaryScreenWidth - (double) GetValue(WidthProperty) - 10;
            Top = 100;
        }

        private void setWindowState(WindowState windowState)
        {
            WindowState = windowState;
        }

        private void closeWindow()
        {
            Close();
        }
        #endregion

        #region Event Handlers
        private static bool activated;
        private void windowActivated(object sender, System.EventArgs e)
        {
            if (!activated)
            {
                activated = true;
                Presenter.Refresh();
            }            
        }

        private void windowLoaded(object sender, RoutedEventArgs e)
        {
            setWindowPosition();

            if (Presenter.AreThereGameModeActions())
            {
                Presenter.Execute();
                return;
            }

            MsgBox.Show(Properties.Resources.LaunchGameModeTitleText, Properties.Resources.InvalidGameModeActionsText, MsgBoxIcon.Error, MsgBoxButtons.Ok);
            closeWindow();
        }

        private void minimizeClick(object sender, RoutedEventArgs e)
        {
            setWindowState(WindowState.Minimized);
        }

        private void closeClick(object sender, RoutedEventArgs e)
        {
            if (!RollBackActionEnded)
            {
                var result = MsgBox.Show(Properties.Resources.LaunchGameModeTitleText, Properties.Resources.WantToExitGameModeText, MsgBoxIcon.Question, MsgBoxButtons.YesNo);
                if (result == MsgBoxResult.Yes)
                {
                    Presenter.Rollback();
                    return;
                }
            }
                
            closeWindow();
        }

        private void exitGameModeClick(object sender, RoutedEventArgs e)
        {
            Presenter.Rollback();
        }

        private void windowClosing(object sender, CancelEventArgs e)
        {
            Presenter.Close();
            Application.Current.Shutdown();
        }
		
        private void windowMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	DragMove();
        }		

        private void listBoxActionSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox != null &&
                listBox.SelectedItem != null)
                listBox.ScrollIntoView(listBox.SelectedItem);
        }
        #endregion
    }
}
