using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
	public class MemoryInfoClass : ServiceWorkerBaseClass, HWInfo
	{
		#region Properties
		private readonly MEMORYSTATUSEX memoryData = new MEMORYSTATUSEX();

		public List<ModifiableKeyValueData> Results { get { return results; } }

		public string DataTitle { get; private set; }
		public double Maximum { get; private set; }

		public static readonly string UsedPhysicalKey = Properties.Resources.UsedPhysical;

		#endregion

		#region Methods
		public void Initialize()
		{
			initializeDataTitle();
			initializeData();
			InitializeWorker();
		}

		private void initializeDataTitle()
		{
			if (!string.IsNullOrEmpty(DataTitle))
				return;

			if (!GlobalMemoryStatusEx(memoryData))
				return;

			DataTitle = string.Format(Properties.Resources.PhysicalMemoryTitle, convertToGigs(memoryData.TotalPhysical));
		}

		private void initializeData()
		{
			waitingTime = 1000;
			Results.Add(new ModifiableKeyValueDataClass(UsedPhysicalKey, MonitoringCategoryInfo.Memory_Usage) { Value = 0, Index = 0 });

			if (GlobalMemoryStatusEx(memoryData))
				Maximum = convertToGigs(memoryData.TotalPhysical);
		}

		protected override void SetResultValues()
		{
			if (!GlobalMemoryStatusEx(memoryData))
				return;

			Results[0].Value = convertToGigs(memoryData.TotalPhysical - memoryData.AvailablePhysical);
		}

		private static double convertToGigs(ulong value)
		{
			return Math.Round((double)value / 1073741824, 1);
		}
		#endregion

		#region Win32 API members
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private class MEMORYSTATUSEX
		{
			public uint Length;
			public uint MemoryLoad;
			public ulong TotalPhysical;
			public ulong AvailablePhysical;
			public ulong TotalPageFile;
			public ulong AvailPageFile;
			public ulong TotalVirtual;
			public ulong AvailableVirtual;
			public ulong AvailableExtendedVirtual;

			public MEMORYSTATUSEX()
			{
				Length = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
			}
		}

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX data);
		#endregion
	}
}
