namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
	public class SnapshotInfoClass : SnapshotInfo
	{
		public string Name { get; private set; }
		public string Date { get; private set; }
		public string FullPath { get; private set; }

		public SnapshotInfoClass(string name, string date, string fullPath)
		{
			Name = name;
			Date = date;
			FullPath = fullPath;
		}
	}
}