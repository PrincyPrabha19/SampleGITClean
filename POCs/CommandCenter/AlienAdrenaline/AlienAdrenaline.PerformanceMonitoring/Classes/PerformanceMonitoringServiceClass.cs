using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
	public class PerformanceMonitoringServiceClass : PerformanceMonitoringService
	{
		#region Properties
		// CPU
		private readonly HWInfo cpuInfo = new CPUInfoClass();
		public List<ModifiableKeyValueData> CPUInfoResults { get { return cpuInfo.Results; } }
		public string CPUDataTitle { get { return cpuInfo.DataTitle; } }
		public double CPUMaximum { get { return cpuInfo.Maximum; } }

		// Memory
		private readonly HWInfo memoryInfo = new MemoryInfoClass();
		public List<ModifiableKeyValueData> MemoryInfoResults { get { return memoryInfo.Results; } }
		public string MemoryTitle { get { return memoryInfo.DataTitle; } }
		public double MemoryMaximum { get { return memoryInfo.Maximum; } }

		// Video
		private readonly HWInfo videoInfo = new VideoCardInfoClass();
		public List<ModifiableKeyValueData> VideoInfoResults { get { return videoInfo.Results; } }
		public string VideoDataTitle { get { return videoInfo.DataTitle; } }
		public double VideoMaximum { get { return videoInfo.Maximum; } }

		// Network
		private readonly HWInfo networkInfo = new NetworkInfoClass();
		public List<ModifiableKeyValueData> NetworkInfoResults { get { return networkInfo.Results; } }
		public string NetworkDataTitle { get { return networkInfo.DataTitle; } }
		public double NetworkMaximum { get { return networkInfo.Maximum; } }

		public bool Started { get; private set; }

		private readonly BackgroundWorker worker = new BackgroundWorker();
		private readonly AutoResetEvent wait = new AutoResetEvent(false);
		private const int WAITING_TIME = 1000;
		#endregion

		#region Events
		public event Action DataWasUpdated;
		#endregion

		#region Constructor
		public PerformanceMonitoringServiceClass()
		{
			initializeCPUInfo();
			initializeMemoryInfo();
			initializeVideoCardInfo();
			initializeNetworkInfo();
			initializeWorker();
		}
		#endregion

		#region Methods
		public void Start()
		{
			if (Started)
				return;

			cpuInfo.Start();
			memoryInfo.Start();
			videoInfo.Start();
			networkInfo.Start();
			startPolling();
		}

		public void Stop()
		{
			if (!Started)
				return;

			stopPolling();
			cpuInfo.Stop();
			memoryInfo.Stop();
			videoInfo.Stop();
			networkInfo.Stop();
		}

		private void initializeCPUInfo()
		{
			cpuInfo.Initialize();
		}

		private void initializeMemoryInfo()
		{
			memoryInfo.Initialize();
		}

		private void initializeVideoCardInfo()
		{
			videoInfo.Initialize();
		}

		private void initializeNetworkInfo()
		{
			networkInfo.Initialize();
		}

		private void startPolling()
		{
			if (Started)
				return;

			Started = true;
			worker.RunWorkerAsync();
		}

		private void stopPolling()
		{
			if (!Started)
				return;

			Started = false;
			wait.Set();
		}

		private void initializeWorker()
		{
			worker.DoWork -= process;
			worker.DoWork += process;
		}

		private void sleep(int milliseconds)
		{
			wait.Reset();
			wait.WaitOne(milliseconds);
		}

		private void onDataUpdated(Action eventToCall)
		{
			if (eventToCall != null)
				eventToCall();
		}
		#endregion

		#region Event Hanlders
		private void process(object sender, DoWorkEventArgs e)
		{
			while (Started)
			{
				onDataUpdated(DataWasUpdated);
				if (Started)
					sleep(WAITING_TIME);
			}
		}
		#endregion
	}
}
