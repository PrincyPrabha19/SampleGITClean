using System.Collections.Generic;
using System.IO;
using AlienLabs.AlienAdrenaline.Domain.Properties;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
	public class SnapshotFileManagementClass : SnapshotFileManagement
	{
		#region Properties
		private readonly SnapshotFinder snapshotFinder = new SnapshotFinderClass();
		public List<SnapshotInfo> Snapshots { get; private set; }
		#endregion

		#region Methods
		public void LoadSnapshots()
		{
			Snapshots = snapshotFinder.GetExistingSnapshots();
		}

		public void Remove(string fileName)
		{
			if (!File.Exists(fileName))
				return;
			
			File.Delete(fileName);
			Snapshots.RemoveAll(x => x.FullPath == fileName);
		}

		public SnapshotInfo Add(string fileName)
		{
			if (Snapshots == null)
				Snapshots = new List<SnapshotInfo>();

			var date = File.GetCreationTime(fileName);
			var result = new SnapshotInfoClass(Path.GetFileNameWithoutExtension(fileName), 
				string.Format(Resources.DateTimeFormat, date.ToShortDateString(), date.ToLongTimeString()), fileName);

			Snapshots.Add(result);

			return result;
		}

		public bool Contains(string fileName)
		{
			return Snapshots.Find(x => x.FullPath == fileName) != null;
		}
		#endregion
		
	}
}
