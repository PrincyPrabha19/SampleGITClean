using System.Collections.Generic;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring
{
	public interface SnapshotFinder
	{
		List<SnapshotInfo> GetExistingSnapshots();
	}
}