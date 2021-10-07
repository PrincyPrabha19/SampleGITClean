using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes.Helpers;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Properties;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
	public class RecordingWriterClass : RecordingWriter
	{
		#region Properties
		private FileStream s;
		private BinaryWriter bw;
		private PerformanceMonitoringService monitorService;

		private Thread worker;
		private readonly AutoResetEvent wait = new AutoResetEvent(false);
		private bool stopped = true;
		private const int waitingTime = 1000;
		private DateTime started;
		private DateTime ended;
		private long records;

		private string defaultNameForCurrentSnapshot;
		public string PrefixSnapshotFileName { get; set; }
		public string SnapshotFileName { get; set; }
		#endregion

		#region Methods
		public void StartRecording(PerformanceMonitoringService perfmonService)
		{
			try
			{
				if (!initializeFile(perfmonService))
					return;

				initAndRunWorker();
			}
			catch
			{
			}
		}

		public void StopRecording()
		{
			stopped = true;
		}

		private bool initializeFile(PerformanceMonitoringService perfmonService)
		{
			monitorService = perfmonService;
			if (!Directory.Exists(SnapshotsFolderProvider.SnapshotsFolder))
				Directory.CreateDirectory(SnapshotsFolderProvider.SnapshotsFolder);

			defaultNameForCurrentSnapshot = getDefaultFileName();

			try
			{
				s = new FileStream(defaultNameForCurrentSnapshot, FileMode.Create);
				bw = new BinaryWriter(s);

				initRecordData();
				writeHeader();
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		private void initRecordData()
		{
			started = DateTime.Now;
			ended = started;
			records = 0;
		}

		private string getDefaultFileName()
		{
			if (!string.IsNullOrEmpty(SnapshotFileName))
				return SnapshotFileName;

			var index = 0;
			var fileNameFormat = string.Format("{0}{1}{2}.{3}", SnapshotsFolderProvider.SnapshotsFolder, PrefixSnapshotFileName, Resources.SnapshotDefaultFilename, SnapshotsFolderProvider.EXTENSION);
			var fileName = string.Format(fileNameFormat, (++index).ToString("D4"));
			bool ok = false;
			const int maximun = 9999;

			do
			{
				if (!File.Exists(fileName))
					ok = true;
				else
				{
					if (index <= maximun)
						fileName = string.Format(fileNameFormat, (++index).ToString("D4"));
					else
					{
						fileName = string.Format(fileNameFormat, Guid.NewGuid().ToString());
						ok = true;
					}

				}

			} while (!ok);

			return fileName;
		}

		private void writeHeader()
		{
			if (monitorService == null || bw == null)
				return;

			// File version, recording dates and # of records
			writeFileHeader();
			// CPU
			writeSimpleDataHeaderSection(monitorService.CPUDataTitle, monitorService.CPUMaximum);
			// Memory
			writeSimpleDataHeaderSection(monitorService.MemoryTitle, monitorService.MemoryMaximum);
			// Network
			writeHeaderSection(monitorService.NetworkDataTitle, monitorService.NetworkInfoResults, monitorService.NetworkMaximum);
			// Video
			writeHeaderSection(monitorService.VideoDataTitle, monitorService.VideoInfoResults, monitorService.VideoMaximum);
		}

		private void writeFileHeader()
		{
			bw.Write(Resources.FileVersion);
			bw.Write(started.ToBinary());
			bw.Write(ended.ToBinary());
			bw.Write(records);
		}

		private void writeSimpleDataHeaderSection(string title, double maximum)
		{
			bw.Write(title);
			bw.Write(maximum);
		}

		private void writeHeaderSection(string title, List<ModifiableKeyValueData> list, double maximum)
		{
			bw.Write(title);
			bw.Write(maximum);
			bw.Write(list.Count);
			foreach (var result in list)
			{
				bw.Write(result.Key);
				bw.Write((int)result.CategoryInfo);
			}
		}

		private void initAndRunWorker()
		{
			stopped = false;
			worker = new Thread(doWork);
			worker.Start();
		}

		private void writeValues()
		{
			// CPU
			writeValuesSection(monitorService.CPUInfoResults, true);
			// Memory
			writeValuesSection(monitorService.MemoryInfoResults, false);
			// Network
			writeValuesSection(monitorService.NetworkInfoResults, false);
			// Video
			writeValuesSection(monitorService.VideoInfoResults, false);
			// counting
			records++;
		}

		private void writeValuesSection(IEnumerable<ModifiableKeyValueData> list, bool writeExtraData)
		{
			foreach (var result in list)
			{
			    if (result == null)
			        bw.Write(0);
			    else
                    bw.Write(result.Value);

                if (!writeExtraData)
                    continue;			        
 
				if (result == null || result.ExtraData == null || result.ExtraData.Count == 0)
					bw.Write(0);
				else
				{
					bw.Write(result.ExtraData.Count);
					foreach (var data in result.ExtraData)
					{
						bw.Write(data.Name ?? String.Empty);
                        bw.Write(data.Description ?? String.Empty);
					}
				}
			}
		}

		private void sleep(int milliseconds)
		{
			wait.Reset();
			wait.WaitOne(milliseconds);
		}

		private void completeRecording()
		{
			ended = DateTime.Now;
			bw.Seek(0, SeekOrigin.Begin);
			writeFileHeader();

			bw.Close();
			s.Close();
		}
		#endregion

		#region Event Handlers
		private void doWork()
		{
			while (!stopped)
			{
				writeValues();
				sleep(waitingTime);
			}

			completeRecording();
		}
		#endregion
	}
}