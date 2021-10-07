using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes.Helpers;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
	public class SnapshotFinderClass : SnapshotFinder
	{
		#region Methods
		public List<SnapshotInfo> GetExistingSnapshots()
		{
			if (!Directory.Exists(SnapshotsFolderProvider.SnapshotsFolder))
				Directory.CreateDirectory(SnapshotsFolderProvider.SnapshotsFolder);

			var files = Directory.GetFiles(SnapshotsFolderProvider.SnapshotsFolder, string.Format("*.{0}", SnapshotsFolderProvider.EXTENSION), SearchOption.AllDirectories);

			return (from file in files
						let fileName = Path.GetFileNameWithoutExtension(file) where !string.IsNullOrEmpty(fileName) 
						let date = File.GetCreationTime(file) let dateTime = date.ToString("G")
						orderby date descending 
					select new SnapshotInfoClass(fileName, dateTime, file)).Cast<SnapshotInfo>().ToList();
		}
		#endregion
	}
}

