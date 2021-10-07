using System.Collections.Generic;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring
{
	public interface SnapshotFileManagement
	{
		List<SnapshotInfo> Snapshots { get; }

		void LoadSnapshots();
		void Remove(string fileName);
		SnapshotInfo Add(string fileName);
		bool Contains(string fileName);
	}
}