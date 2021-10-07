using System.Windows;
using AlienLabs.AlienAdrenaline.App.Presenters;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml
{
    public partial class GameModeSaveAsDialog : GameModeSaveAsView
    {
        #region Public Properties
        public GameModeSaveAsPresenter Presenter { get; set; }
        #endregion

        #region GameModeCreateActionView Properties
        public ViewType Type { get { return ViewType.GameModeSaveAs; } }
        
        public string GameModeName
        {
            get { return textBoxGameModeName.Text; }
            set { textBoxGameModeName.Text = value; }
        }
        #endregion

        #region Constructors
        public GameModeSaveAsDialog()
        {
            InitializeComponent();
        }
        #endregion

        #region GameModeSaveAsView Methods
        public void Refresh()
        {
            Presenter.Refresh();
        }
        #endregion

        #region Event Handlers
        private void textBoxGameModeName_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            Presenter.GameModeNameChanged(GameModeName);
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            Presenter.SaveProfile();

            DialogResult = true;
            Close();
        }
        #endregion            
    }
}
