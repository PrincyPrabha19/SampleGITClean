using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
	public class NetworkInfoClass : ServiceWorkerBaseClass, HWInfo
	{
		#region Properties
		private readonly List<PerformanceCounter> counters = new List<PerformanceCounter>();

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

		private void initializeDataTitle()
		{
			if (!string.IsNullOrEmpty(DataTitle))
				return;

			DataTitle = Properties.Resources.NetworkUtilization;
		}

		private void initializeData()
		{
			waitingTime = 1000;
			var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

			foreach (NetworkInterface ni in networkInterfaces)
			{
				if (ni.OperationalStatus != OperationalStatus.Up)
					continue;

				switch (ni.NetworkInterfaceType)
				{
					case NetworkInterfaceType.Ethernet:
						string lanControllerName = ni.Description.Replace("(", "[").Replace(")", "]").Replace("/", "_").Replace("#", "_");
						counters.Add(new PerformanceCounter("Network Interface", "Bytes Received/sec", lanControllerName));
						counters.Add(new PerformanceCounter("Network Interface", "Bytes Sent/sec", lanControllerName));

						results.Add(new ModifiableKeyValueDataClass(string.Format(Properties.Resources.WiredBytesReceived, ni.Name), MonitoringCategoryInfo.Network_BytesReceived));
						results.Add(new ModifiableKeyValueDataClass(string.Format(Properties.Resources.WiredBytesSent, ni.Name), MonitoringCategoryInfo.Network_BytesSent));

						Maximum = Math.Max(Maximum, ni.Speed / 8192f);
						break;
					case NetworkInterfaceType.Wireless80211:
						string wifiControllerName = ni.Description.Replace("(", "[").Replace(")", "]").Replace("/", "_").Replace("#", "_");
						counters.Add(new PerformanceCounter("Network Interface", "Bytes Received/sec", wifiControllerName));
						counters.Add(new PerformanceCounter("Network Interface", "Bytes Sent/sec", wifiControllerName));

						results.Add(new ModifiableKeyValueDataClass(string.Format(Properties.Resources.WirelessBytesReceived, ni.Name), MonitoringCategoryInfo.Network_BytesReceived));
						results.Add(new ModifiableKeyValueDataClass(string.Format(Properties.Resources.WirelessBytesSent, ni.Name), MonitoringCategoryInfo.Network_BytesSent));

                        Maximum = Math.Max(Maximum, ni.Speed / 8192f);
						break;
				}
			}
		}

		protected override void SetResultValues()
		{
			if (counters.Count != results.Count)
				return;

			for (var i = 0; i < results.Count; i++)
                results[i].Value = counters[i].NextValue() / 1024f;
		}
		#endregion
	}
}
