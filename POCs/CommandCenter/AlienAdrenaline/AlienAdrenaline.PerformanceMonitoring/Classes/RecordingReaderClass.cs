using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
	public class RecordingReaderClass : RecordingReader
	{
		#region Properties
		public List<ChartData> CPUInfo { get; private set; }
		public List<ChartData> MemoryInfo { get; private set; }
		public List<ChartData> NetworkBytesReceived { get; private set; }
		public List<ChartData> NetworkBytesSent { get; private set; }
		public List<ChartData> VideoGPUUsage { get; private set; }
		public List<ChartData> VideoTemperature { get; private set; }
		public List<ChartData> VideoMemoryLoad { get; private set; }
		public List<ChartData> VideoFanSpeed { get; private set; }

		public DateTime Started { get; private set; }
		public DateTime Ended { get; private set; }
		public long Records { get; private set; }
        public string FileName { get; private set; }

		private List<ChartData> order;

		private FileStream s;
		private BinaryReader br;
		private long position;

		private readonly BackgroundWorker worker = new BackgroundWorker();
		private int recordIndex;
		#endregion

		#region Constructor
		public RecordingReaderClass()
		{
		}
		#endregion

		#region Events
		public event Action HeaderWasRead;
		public event Action ReadCompleted;
		public event Action<int> ProgressChanged;
		public event Action IncompatibleFileDetected;
		#endregion

		#region Methods
        public bool Load(string fileName)
        {
            FileName = fileName;
            return readFile(fileName);
        }

	    private void initData()
	    {
	        position = 0;

            CPUInfo = new List<ChartData>();
            MemoryInfo = new List<ChartData>();
            NetworkBytesReceived = new List<ChartData>();
            NetworkBytesSent = new List<ChartData>();
            VideoGPUUsage = new List<ChartData>();
            VideoTemperature = new List<ChartData>();
            VideoMemoryLoad = new List<ChartData>();
            VideoFanSpeed = new List<ChartData>();

            order = new List<ChartData>();
	    }

	    private bool readFile(string snapshotFilename)
		{
            initData();

			if (string.IsNullOrEmpty(snapshotFilename))
				return false;

			if (!readHeader(snapshotFilename))
				return false;

			return readData(snapshotFilename);
		}

		private void openStreams(string snapshotFilename)
		{
			s = new FileStream(snapshotFilename, FileMode.Open);
			br = new BinaryReader(s);
			br.BaseStream.Seek(position, SeekOrigin.Begin);
		}

		private void closeStreams()
		{
			if (br != null)
				br.Close();

			if (s != null)
				s.Close();
		}

		private bool readHeader(string snapshotFilename)
		{
			var result = false;
			try
			{
				openStreams(snapshotFilename);
				if (br != null)
				{
					var versionString = br.ReadString();
					var version = new Version(versionString);

					if (version.CompareTo(new Version(Properties.Resources.FileVersion)) == 0)
					{
						Started = DateTime.FromBinary(br.ReadInt64());
						Ended = DateTime.FromBinary(br.ReadInt64());
						Records = br.ReadInt64();

						// CPU
						var data = readSimpleDataHeaderSection(MonitoringCategoryInfo.CPU_Usage);
						CPUInfo.Add(data);
						order.Add(data);

						// Memory
						data = readSimpleDataHeaderSection(MonitoringCategoryInfo.Memory_Usage);
						MemoryInfo.Add(data);
						order.Add(data);

						// Network
						readMultipleDataHeaderSection();
						// Video
						readMultipleDataHeaderSection();

                        //Initialize frst position
					    initZeroValues();

						position = br.BaseStream.Position;
						onEventCall(HeaderWasRead);
						result = true;
					}
					else
						onEventCall(IncompatibleFileDetected);
				}
			}
			catch (Exception)
			{
				onEventCall(IncompatibleFileDetected);
			}
			finally
			{
				closeStreams();
			}

			return result;
		}

		private ChartData readSimpleDataHeaderSection(MonitoringCategoryInfo category)
		{
			return new ChartDataClass(br.ReadString(), br.ReadDouble(), category);
		}

		private void readMultipleDataHeaderSection()
		{
			var categoryTitle = br.ReadString();
			var maximum = br.ReadDouble();
			var count = br.ReadInt32();

			for (var i = 0; i < count; i++)
			{
				var chartTitle = br.ReadString();
				var categoryInfo = (MonitoringCategoryInfo) br.ReadInt32();
				var data = new ChartDataClass(chartTitle, maximum, categoryInfo);
				switch (data.MonitoringCategory)
				{
					case MonitoringCategoryInfo.Network_BytesReceived:
						NetworkBytesReceived.Add(data);
						order.Add(data);
						break;
					case MonitoringCategoryInfo.Network_BytesSent:
						NetworkBytesSent.Add(data);
						order.Add(data);
						break;
					case MonitoringCategoryInfo.Video_GPUUsage:
						VideoGPUUsage.Add(data);
						order.Add(data);
						break;
					case MonitoringCategoryInfo.Video_Temperature:
						VideoTemperature.Add(data);
						order.Add(data);
						break;
					case MonitoringCategoryInfo.Video_MemoryLoad:
						VideoMemoryLoad.Add(data);
						order.Add(data);
						break;
					case MonitoringCategoryInfo.Video_FanSpeed:
						VideoFanSpeed.Add(data);
						order.Add(data);
						break;
				}
			}
		}

		private void onEventCall(Action eventToCall)
		{
			if (eventToCall != null)
				eventToCall();
		}

		private bool readData(string snapshotFilename)
		{
			try
			{
				openStreams(snapshotFilename);
				worker.WorkerReportsProgress = true;

				worker.DoWork -= doWork;
				worker.DoWork += doWork;
				worker.RunWorkerCompleted -= complete;
				worker.RunWorkerCompleted += complete;
				worker.ProgressChanged -= progressChanged;
				worker.ProgressChanged += progressChanged;
				worker.RunWorkerAsync();
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

        private void initZeroValues()
        {
            foreach (var data in order)
                data.Add(0, null);
        }

        private void readValues()
        {
            foreach (var data in order)
                readValuesSection(data, data.MonitoringCategory == MonitoringCategoryInfo.CPU_Usage);
        }

		private void readValuesSection(ChartData chartData, bool readExtraData)
		{
			List<ProcessData> extraData = null;
			var value = (int)br.ReadDouble();
			if (readExtraData)
			{
				var processCount = br.ReadInt32();
				if (processCount > 0)
				{
					var data = new List<ProcessData>();
					for (var i = 0; i < processCount; i++)
						data.Add(new ProcessDataClass(br.ReadString(), br.ReadString()));

					extraData = data;
				}
			}

			chartData.Add(value, extraData);
		}

		private void callProgressChanged(int progress)
		{
			if (ProgressChanged != null)
				ProgressChanged(progress);
		}
		#endregion

		#region Event Handlers
		private void doWork(object sender, DoWorkEventArgs doWorkEventArgs)
		{
			recordIndex = 0;
			worker.ReportProgress(0);

			for (int i = 0; i < Records && br.BaseStream.Position < br.BaseStream.Length; i++)
			{
				readValues();
				worker.ReportProgress(++recordIndex);
			}
		}

		private void complete(object sender, RunWorkerCompletedEventArgs e)
		{
			closeStreams();
			onEventCall(ReadCompleted);
		}

		private void progressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
		{
			callProgressChanged(progressChangedEventArgs.ProgressPercentage);
		}
		#endregion
	}
}