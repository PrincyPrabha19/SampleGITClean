using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;

namespace AlienLabs.AlienAdrenaline.App.Views
{
	public interface PerformanceSnapshotsView : ContentView, INotifyPropertyChanged
	{
		PerformanceSnapshotListPresenter Presenter { get; }
        bool ExportSnapshotEnabled { get; set; }

		void RefreshData(List<SnapshotInfo> snapshots);
		SnapshotInfo GetSelectedSnapshot();
	    List<SnapshotInfo> GetSelectedSnapshots();
		void RemoveCurrectSelection();
		void AddNewSnapshot(SnapshotInfo snapshot);
		void SelectSnapshotWithFullPath(string fileName);
	}
}