

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using AlienLabs.AlienAdrenaline.App.Helpers;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.Domain.Classes;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml.Actions
{
    /// <summary>
    /// Interaction logic for AudioOutput.xaml
    /// </summary>
    public partial class AudioOutputUserControl : GameModeActionAudioOutputView
    {
        #region GameModeActionAudioOutputView Properties
        public GameModeActionViewType ActionType { get { return GameModeActionViewType.AudioOutput; } }
        public GameModeActionAudioOutputPresenter Presenter { get; set; }        
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event Action ProfileAction_Changed;

        public ObservableCollection<AudioDeviceData> AudioDevices
        {
            get { return listBoxDevices.ItemsSource as ObservableCollection<AudioDeviceData>; }
            set { listBoxDevices.ItemsSource = value; }
        }

        public AudioDeviceData AudioDeviceSelected
        {
            get { return listBoxDevices.SelectedItem as AudioDeviceData; }
            set { listBoxDevices.SelectedIndex = listBoxDevices.Items.IndexOf(value); }
        }

        public void Refresh()
        {
            Presenter.Refresh();
        }
        #endregion

        #region Constructors
        public AudioOutputUserControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        private void buttonSoundDevice_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as ToggleButton;
            if (button != null &&
                button.IsChecked != null && button.IsChecked.Value)
            {
                var lbi = button.TryFindParent<ListBoxItem>();
                if (lbi != null)
                    lbi.IsSelected = true;
            }
        }

        private void listBoxDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox != null &&
                listBox.SelectedItem != null)
            {
                listBox.ScrollIntoView(listBox.SelectedItem);

                Presenter.SetAudioOutputInfo();
            }            
        }
        #endregion
    }
}
