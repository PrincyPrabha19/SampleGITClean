#define DEBUG
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml
{
	public partial class PerformanceSnapshotsControl : PerformanceSnapshotsView
	{
		#region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

        #region PerformanceSnapshotsView Properties
        public PerformanceSnapshotListPresenter Presenter { get; set; }
		public ViewType Type { get { return ViewType.PerformanceSnapshots; } }
        public Action<ExtendedPoint, ExtendedPoint> CPUChartProcessDataSelectionChanged;

		private ObservableCollection<SnapshotInfo> data;	            
        #endregion

		#region Properties
		private static readonly DependencyProperty thereIsSnapshotsProperty = DependencyProperty.Register("ThereIsSnapshots", typeof(bool), typeof(PerformanceSnapshotsControl));
		public bool ThereIsSnapshots
		{
			get { return (bool)GetValue(thereIsSnapshotsProperty); }
			set { SetValue(thereIsSnapshotsProperty, value); }
		}

        private static readonly DependencyProperty exportSnapshotEnabledProperty = DependencyProperty.Register("ExportSnapshotEnabled", typeof(bool), typeof(PerformanceSnapshotsControl), new UIPropertyMetadata(false));
        public bool ExportSnapshotEnabled
        {
            get { return (bool)GetValue(exportSnapshotEnabledProperty); }
            set { SetValue(exportSnapshotEnabledProperty, value); }
        } 
		#endregion

		#region Constructor
		public PerformanceSnapshotsControl()
		{
			InitializeComponent();
		}
		#endregion

        #region Methods
        public void Refresh()
        {
            Presenter.Refresh();
        }

		public void RefreshData(List<SnapshotInfo> snapshots)
		{
			data = new ObservableCollection<SnapshotInfo>(snapshots);
			listboxSnapshots.ItemsSource = data;
			refreshThereIsSnapshotProperty();
		}

		public SnapshotInfo GetSelectedSnapshot()
		{
			return listboxSnapshots.SelectedItem as SnapshotInfo;
		}

        public List<SnapshotInfo> GetSelectedSnapshots()
        {
            return listboxSnapshots.SelectedItems.Cast<SnapshotInfo>().ToList();
        }

		public void RemoveCurrectSelection()
		{
			if (data == null)
				return;

			var current = GetSelectedSnapshot();
			if (current == null)
				return;

			data.Remove(current);
			refreshThereIsSnapshotProperty();
		}

		public void AddNewSnapshot(SnapshotInfo snapshot)
		{
			data.Add(snapshot);
			selectSnapshot(snapshot);
			refreshThereIsSnapshotProperty();
		}

		private void refreshThereIsSnapshotProperty()
		{
			ThereIsSnapshots = data.Count > 0;
		}

		public void SelectSnapshotWithFullPath(string fileName)
		{
			var snapshot = data.FirstOrDefault(x => x.FullPath == fileName);
			if (snapshot == null)
				return;

			selectSnapshot(snapshot);
		}

		private void selectSnapshot(SnapshotInfo snapshot)
		{
			listboxSnapshots.SelectedItem = snapshot;
			listboxSnapshots.ScrollIntoView(snapshot);
		}
		#endregion

		#region Event Handlers
        private bool selectionChangedExecuting;
		private void selectionChanged(object sender, SelectionChangedEventArgs e)
		{
		    if (selectionChangedExecuting) return;

		    try
		    {
                selectionChangedExecuting = true;

                if (doubleClickedSnapshot != null)
		        {
                    listboxSnapshots.SelectedItems.Clear();
		            listboxSnapshots.SelectedItems.Add(doubleClickedSnapshot);
                    //listboxSnapshots.SelectedItem = doubleClickedSnapshot;
                    Presenter.ViewSelection();
		        }
                else
		        if (listboxSnapshots.SelectedItems.Count > 2)
		            listboxSnapshots.SelectedItems.RemoveAt(0);

		        Presenter.SelectionWasChanged();
		    }
		    finally
		    {
		        selectionChangedExecuting = false;
		    }
		}

        private SnapshotInfo doubleClickedSnapshot;
        private void snapshotInfoOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            doubleClickedSnapshot = null;
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
                doubleClickedSnapshot = ((StackPanel)sender).Tag as SnapshotInfo;
        }
		
		private void exportRecording(object sender, RoutedEventArgs e)
		{
            Presenter.ExportRecording();
		}

		private void importRecording(object sender, RoutedEventArgs e)
		{
            Presenter.ImportRecording();
		}

        private void deleteRecording(object sender, KeyEventArgs e)
        {
            if (listboxSnapshots.SelectedItems.Count == 1 && e.Key == Key.Delete)
            {
                var currentIndex = listboxSnapshots.SelectedIndex;

                Presenter.DeleteCurrentSnapshot();

                if (listboxSnapshots.Items.Count <= 0) return;
                listboxSnapshots.SelectedIndex = currentIndex < listboxSnapshots.Items.Count ? currentIndex : currentIndex - 1;
            }
        }
		#endregion
	}
}








