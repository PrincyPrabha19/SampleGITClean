using System;
using System.Collections.Generic;
using System.IO;
using AlienLabs.AlienAdrenaline.App.Factories;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes.Helpers;
using AlienLabs.Tools;
using Microsoft.Win32;
using UsageTacking.Domain;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
	public class PerformanceSnapshotListPresenter
    {
        #region Properties
		public PerformanceSnapshotsView View { get; set; }
		public PerformanceMonitoringView RecordingView { get; set; }

        public ViewRepository ViewRepository { get; set; }      
        public CommandContainer CommandContainer { get; set; }
        public EventTrigger EventTrigger { get; set; }

		public virtual bool IsDirty
        {
            get { return !CommandContainer.IsEmpty; }
        }

		private SnapshotFileManagement snapshotFileManagement;
        #endregion

		#region Events
		public event Action<int> SelectionChanged;
		public event Action ViewSelectionRequested;
		#endregion

        #region Methods
        public void Initialize()
        {
			if (snapshotFileManagement == null)
				snapshotFileManagement = new SnapshotFileManagementClass();

			snapshotFileManagement.LoadSnapshots();
        }

		public void Refresh()
		{
			View.RefreshData(snapshotFileManagement.Snapshots);
		}

		public bool DeleteCurrentSnapshot()
		{
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "PerformanceMonitoring", Features.PerformanceMonitoringDeleteCurrentSnapshot, DateTime.Now);
			var snapshotInfo = View.GetSelectedSnapshot();
			var procced = MsgBox.Show(Properties.Resources.DeleteSnapshotTitle.ToUpper(),
                    string.Format(Properties.Resources.DeleteSnapshotQuestion, snapshotInfo.Name), 
					MsgBoxIcon.Question, MsgBoxButtons.YesNo) == MsgBoxResult.Yes;

			if (!procced)
				return false;

			try
			{
				snapshotFileManagement.Remove(snapshotInfo.FullPath);
				View.RemoveCurrectSelection();
			}
			catch
			{
				return false;
			}

			return true;
		}

		public void SaveAsCurrentSnapshot()
		{
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "PerformanceMonitoring", Features.PerformanceMonitoringSaveAsCurrentSnapshot, DateTime.Now);
			var snapshotInfo = View.GetSelectedSnapshot();
			if (!File.Exists(snapshotInfo.FullPath))
				return;

			try
			{
				var saveDialog = new SaveFileDialog
				{
					DefaultExt = SnapshotsFolderProvider.EXTENSION,
					Filter = string.Format("{1} (.{0})|*.{0}", SnapshotsFolderProvider.EXTENSION, PerformanceMonitoring.Properties.Resources.PerformanceMonitoringSnapshotsText),
					InitialDirectory = SnapshotsFolderProvider.SnapshotsFolder
				};

				var proposedFileName = string.Format(string.Format(Properties.Resources.CopyOfText, snapshotInfo.Name));
				saveDialog.FileName = proposedFileName;

				bool ok = saveDialog.ShowDialog() == true;
				if (ok)
				{
					var fileName = saveDialog.FileName;
					File.Copy(snapshotInfo.FullPath, fileName, true);

					if (Path.GetDirectoryName(fileName) + "\\" != SnapshotsFolderProvider.SnapshotsFolder)
						return;

					if (snapshotFileManagement.Contains(fileName))
						View.SelectSnapshotWithFullPath(fileName);
					else
					{
						var snapshot = snapshotFileManagement.Add(snapshotInfo.FullPath);
						View.AddNewSnapshot(snapshot);
					}
				}
			}
			catch (Exception ex)
			{
				MsgBox.Show(Properties.Resources.SaveAsSnapshotTitle, ex.Message);
			}
		}

        //public string GetSelectedSnapshotFile()
        //{
        //    var snapshotInfo = View.GetSelectedSnapshot();
        //    return snapshotInfo == null || !File.Exists(snapshotInfo.FullPath) ? "" : snapshotInfo.FullPath;
        //}

        public List<string> GetSelectedSnapshotFiles()
        {
            var snapshotFiles = new List<string>();

            var snapshotInfos = View.GetSelectedSnapshots();            
            foreach (var snapshotInfo in snapshotInfos)
            {
                if (snapshotInfo != null && File.Exists(snapshotInfo.FullPath))
                    snapshotFiles.Add(snapshotInfo.FullPath);
            }

            return snapshotFiles;
        }

		public void SelectionWasChanged()
		{
		    int selectedSnapshotsCount = View.GetSelectedSnapshots().Count;
            View.ExportSnapshotEnabled = selectedSnapshotsCount == 1;
			if (SelectionChanged != null)
                SelectionChanged(selectedSnapshotsCount);
		}

		public void ViewSelection()
		{
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "PerformanceMonitoring", Features.PerformanceMonitoringViewSelection, DateTime.Now);

			if (View.GetSelectedSnapshot() != null && ViewSelectionRequested != null)
				ViewSelectionRequested();
		}

        public void ExportRecording()
        {
            var snapshotInfo = View.GetSelectedSnapshot();
            if (!File.Exists(snapshotInfo.FullPath))
                return;

            try
            {
                var saveDialog = new SaveFileDialog
                {
                    DefaultExt = SnapshotsFolderProvider.EXTENSION,
                    Filter = string.Format("{1} (.{0})|*.{0}", SnapshotsFolderProvider.EXTENSION, PerformanceMonitoring.Properties.Resources.PerformanceMonitoringSnapshotsText),
                    InitialDirectory = SnapshotsFolderProvider.SnapshotsFolder
                };

                var newFullFilename = getValidCopyFilename(snapshotInfo.FullPath);
                var proposedFileName = Path.GetFileName(newFullFilename);
                saveDialog.FileName = proposedFileName;

                bool ok = saveDialog.ShowDialog() == true;
                if (ok)
                {
                    var fileName = saveDialog.FileName;
                    File.Copy(snapshotInfo.FullPath, fileName, true);

                    if (Path.GetDirectoryName(fileName) + "\\" != SnapshotsFolderProvider.SnapshotsFolder)
                        return;

                    if (snapshotFileManagement.Contains(fileName))
                        View.SelectSnapshotWithFullPath(fileName);
                    else
                    {
                        var snapshot = snapshotFileManagement.Add(fileName);
                        View.AddNewSnapshot(snapshot);
                    }

                    Refresh();
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(Properties.Resources.ExportSnapshotTitle, ex.Message);
            }
        }

        public void ImportRecording()
        {
            try
            {
                var openDialog = new OpenFileDialog
                {
                    DefaultExt = SnapshotsFolderProvider.EXTENSION,
                    Filter = string.Format("{1} (.{0})|*.{0}", SnapshotsFolderProvider.EXTENSION, PerformanceMonitoring.Properties.Resources.PerformanceMonitoringSnapshotsText),
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                };

                bool ok = openDialog.ShowDialog() == true;
                if (ok)
                {
                    var fileName = openDialog.FileName;
                    var targetFileName = Path.Combine(SnapshotsFolderProvider.SnapshotsFolder, Path.GetFileName(fileName));
                    if (File.Exists(targetFileName))
                    {
                        var result = MsgBox.Show(Properties.Resources.ImportSnapshotTitle, Properties.Resources.ImportedSnapshotExists, MsgBoxIcon.Question, MsgBoxButtons.YesNo);
                        if (result == MsgBoxResult.No)
                            targetFileName = getValidCopyFilename(targetFileName);
                    }

                    File.Copy(fileName, targetFileName, true);

                    if (snapshotFileManagement.Contains(targetFileName))
                        View.SelectSnapshotWithFullPath(targetFileName);
                    else
                    {
                        var snapshot = snapshotFileManagement.Add(targetFileName);
                        View.AddNewSnapshot(snapshot);
                    }

                    Refresh();
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(Properties.Resources.ImportSnapshotTitle, ex.Message);
            }      
        }

        private string getValidCopyFilename(string fullFilename)
        {
            string newFullFilename;
            string extension = Path.GetExtension(fullFilename);
            string directory = Path.GetDirectoryName(fullFilename);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullFilename);

            do
            {
                fileNameWithoutExtension = String.Format(Properties.Resources.CopySuffix, fileNameWithoutExtension);
                newFullFilename = Path.Combine(directory, fileNameWithoutExtension + extension);
            } while (File.Exists(newFullFilename));

            return newFullFilename;
        }
	    #endregion
    }
}