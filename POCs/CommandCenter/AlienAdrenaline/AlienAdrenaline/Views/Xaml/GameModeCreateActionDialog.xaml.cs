using System.Collections.ObjectModel;
using System.Windows;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml
{
    public partial class GameModeCreateActionDialog : GameModeCreateActionView
    {
        #region Public Properties
        public GameModeCreateActionPresenter Presenter { get; set; }
        #endregion

        #region GameModeCreateActionView Properties
        public ViewType Type { get { return ViewType.GameModeCreateAction; } }
        public ProfileAction ProfileAction { get; set; }

        public GameModeActionType GameModeActionTypeSelected 
        {
            get
            {
                return (GameModeActionType)comboBoxAvailableActions.SelectedValue;
            }
        }
        
        public ObservableCollection<GameModeActionType> AvailableActions
        {
            get { return comboBoxAvailableActions.ItemsSource as ObservableCollection<GameModeActionType>; }
            set { comboBoxAvailableActions.ItemsSource = value; }
        }        
        #endregion

        #region Constructors
        public GameModeCreateActionDialog()
        {
            InitializeComponent();
        }
        #endregion

        #region GameModeCreateActionView Methods
        public void Refresh()
        {
            Presenter.Refresh();
        }
        #endregion

        #region Event Handlers
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            Presenter.AddProfileAction();

            DialogResult = true;
            Close();
        }
        #endregion            
    }
}
