using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Text.RegularExpressions;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
	public class CPUInfoClass : ServiceWorkerBaseClass, HWInfo
	{
		#region Properties
		private int iteration;
		private const int secondBetweenExtraData = 30;

		private readonly List<PerformanceCounter> cpuCounters = new List<PerformanceCounter>();

		public List<ModifiableKeyValueData> Results { get { return results; } }

		public string DataTitle { get; private set; }
		public double Maximum { get; private set; }
		#endregion

		#region Methods
		public void Initialize()
		{
			initializeDataTitle();
			initializeData();
			InitializeWorker();
		}

		private void initializeData()
		{
			waitingTime = 1000;

			cpuCounters.Add(new PerformanceCounter("Processor", "% Processor Time", "_Total", true));
			results.Add(new ModifiableKeyValueDataClass(Properties.Resources.CPUAverage, MonitoringCategoryInfo.CPU_Usage) { Value = 0 });

			Maximum = 100;
		}

		private void initializeDataTitle()
		{
			if (!string.IsNullOrEmpty(DataTitle))
				return;

			var cpuTitle = string.Format(Properties.Resources.CPUTitle, getProcessorID());
			cpuTitle = Regex.Replace(cpuTitle, @"\s{2,}", " ");
			DataTitle = cpuTitle;
		}

		protected override void SetResultValues()
		{
			if (cpuCounters.Count != results.Count)
				return;

			for (var i = 0; i < results.Count; i++)
				results[i].Value = cpuCounters[i].NextValue();

			if (++iteration < secondBetweenExtraData)
				results[0].ExtraData = null;
			else
			{
				iteration = 0;
				results[0].ExtraData = getProcesses();
			}
		}

		private List<ProcessData> getProcesses()
		{
			var data = new List<ProcessData>();

			foreach (var process in Process.GetProcesses())
			{
				var descrip = "";
				try
				{
					descrip = FileVersionInfo.GetVersionInfo(process.MainModule.FileName).FileDescription;
				}
				catch (Exception) { }

				data.Add(new ProcessDataClass(process.ProcessName, descrip));
			}

			return data.Count == 0 ? null : data;
		}

		private string getProcessorID()
		{
			var cpuInfo = String.Empty;

			var objCol = new ManagementClass("Win32_Processor").GetInstances();
			foreach (ManagementObject mgtObj in objCol)
			{
				cpuInfo = mgtObj.Properties["Name"].Value.ToString();
				break;
			}

			return cpuInfo;
		}
		#endregion
	}
}
