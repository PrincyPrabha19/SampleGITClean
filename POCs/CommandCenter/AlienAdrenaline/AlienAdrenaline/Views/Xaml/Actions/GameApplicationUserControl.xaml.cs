
using AlienLabs.AlienAdrenaline.App.Helpers;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.CommandCenter.Tools;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using KeystrokesDetector;
using KeystrokesDetector.Classes;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml.Actions
{
    public partial class GameApplicationUserControl : GameModeActionGameApplicationView
    {
        #region GameModeActionWebLinksView Members
        public GameModeActionViewType ActionType { get { return GameModeActionViewType.GameApplication; } }
        public GameModeActionGameApplicationPresenter Presenter { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event Action ProfileAction_Changed;

        private string gameTitle;
        public string GameTitle
        {
            get { return gameTitle; }
            set
            {
                gameTitle = value;
                PropertyChanged(this, new PropertyChangedEventArgs("GameTitle"));
            }
        }

        private byte[] gameImage;
        public byte[] GameImage
        {
            get { return gameImage; }
            set
            {
                gameImage = value;
                PropertyChanged(this, new PropertyChangedEventArgs("GameImage"));
            }
        }

        private string gamePath;
        public string GamePath
        {
            get { return gamePath; }
            set
            {
                gamePath = value;
                PropertyChanged(this, new PropertyChangedEventArgs("GamePath"));
            }
        }

        private string gameRealPath;
        public string GameRealPath
        {
            get { return gameRealPath; }
            set
            {
                gameRealPath = value;
                if (!String.IsNullOrEmpty(gameRealPath))
                    imageUpdateGameRealPathStatus.Visibility = Visibility.Visible;

                PropertyChanged(this, new PropertyChangedEventArgs("GameRealPath"));
            }
        }

        public void Refresh()
        {
            Presenter.Refresh();
        }
        #endregion

        #region Private Properties
        private KeyboardRawInputManager keyboardRawInputManager;
        #endregion

        #region API
        [DllImport("user32", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int W, int H, uint uFlags);

        [DllImport("user32", SetLastError = true)]
        private static extern bool SetFocus(IntPtr hWnd);

        public static readonly uint SWP_NOSIZE = 0x0001, SWP_NOMOVE = 0x0002, SWP_SHOWWINDOW = 0x0040;
        #endregion

        #region Constructors
        public GameApplicationUserControl()
        {
            InitializeComponent();

            var description = String.Format(Properties.Resources.UpdateGameExecutableDescriptionText, Properties.Resources.CtrlShiftF3Text);
            richTextBoxDescription.AppendText(description);
            var index = description.IndexOf(Properties.Resources.CtrlShiftF3Text, StringComparison.InvariantCulture);
            if (index != -1)
            {
                richTextBoxDescription.SelectText(index + 1, Properties.Resources.CtrlShiftF3Text.Length + 1);
                var foreground = FindResource("AlienBlue") as SolidColorBrush ??
                                 new SolidColorBrush(Color.FromArgb(0xFF, 0x2E, 0xB8, 0xFF));
                richTextBoxDescription.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, foreground);
            }
        }
        #endregion

        #region Event Handlers
        private void btnUpdateRealApplicationPath_Click(object sender, RoutedEventArgs e)
        {
            var window = this.TryFindParent<Window>();
            if (window != null)
            {
                var helper = new WindowInteropHelper(window);
                keyboardRawInputManager = new KeyboardRawInputManagerClass(helper.Handle);
                keyboardRawInputManager.RegisterKeyboardRawInput(new ushort[] { 0x10, 0x11, 0x72 }); //SHIFT+CTRL+F3
                keyboardRawInputManager.RegisteredKeystrokesDetected += keyboardRawInputManager_RegisteredKeystrokesDetected;
                keyboardRawInputManager.StartWindowsMessageLoop();

                Process process;
                ApplicationLaunchHelper.Execute(GamePath, out process);
            }
        }

        private void keyboardRawInputManager_RegisteredKeystrokesDetected()
        {
            try
            {
                var processPath = ForegroundWindowHelper.GetForegroundProcessPath();
                if (!String.IsNullOrEmpty(processPath))
                {
                    foregroundWindowKeyPressed(processPath);
                    if (keyboardRawInputManager.WindowHandle != IntPtr.Zero)
                        activateWindow(keyboardRawInputManager.WindowHandle);
                }

                Presenter.WriteGameDefinitionToRegistry();
            }
            finally
            {
                keyboardRawInputManager.StopWindowsMessageLoop();
            }
        }

        private void foregroundWindowKeyPressed(string applicationPath)
        {
            GameRealPath = applicationPath;
            if (isGameRealPathValid())
                Presenter.SetApplicationInfo();

            if (ProfileAction_Changed != null)
                ProfileAction_Changed();
        }
        #endregion

        #region Private Methods
        private bool isGameRealPathValid()
        {
            //stackPanelApplicationError.Visibility = Visibility.Collapsed;
            //if (!Presenter.IsValidApplicationPath())
            //{
            //    stackPanelApplicationError.Visibility = Visibility.Visible;
            //    return false;
            //}

            return true;
        }

        public void activateWindow(IntPtr windowHandle)
        {
            //var newPos = new IntPtr(-1); // 0 puts it on top of Z order. Do -1 to force it to a topmost instead.
            //SetWindowPos(windowHandle, newPos, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);
            //SetFocus(windowHandle);
            SetForegroundWindow(windowHandle);
        }
        #endregion
    }
}
