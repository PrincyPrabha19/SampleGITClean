using System;
using AlienLabs.AlienAdrenaline.Domain.Helpers;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes.Helpers
{
	public static class SnapshotsFolderProvider
	{
		public const string EXTENSION = "pms";
		public static readonly string SnapshotsFolder = String.Format(@"{0}Snapshots\", AdrenalinePathProvider.AdrenalineFolder);
	}
}