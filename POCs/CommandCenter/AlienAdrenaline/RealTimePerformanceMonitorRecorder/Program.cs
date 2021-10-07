using System;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes.Helpers;
using AlienLabs.Tools.Classes;
using RealTimePerformanceMonitorRecorder.Classes;
using System.Linq;

namespace RealTimePerformanceMonitorRecorder
{
	class Program
	{
		const string FILEPREFIX_PARAM = "/prefix:";
		const string FILENAME_PARAM = "/file:";

		static void Main(string[] args)
		{
            string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
			bool instanceRunning = ApplicationInstances.IsApplicationInstanceRunning(appName);

			if (!instanceRunning)
			{
				string prefix = args.FirstOrDefault(x => x.StartsWith(FILEPREFIX_PARAM));
				string filename = args.FirstOrDefault(x => x.StartsWith(FILENAME_PARAM));

				prefix = (prefix != null) ? prefix.Replace(FILEPREFIX_PARAM, "") : "";
				filename = (filename != null) ? filename.Replace(FILENAME_PARAM, "") : "";

				var eventMnager = new RecordingEventManager(filename, prefix);
				RTPMGadgetOperations.RunRTPMGadget();
			}
			else if (args.FirstOrDefault(x => x.ToLower() =="/htk") != null)
				RTPMRecorderWrapper.StopRecording();
		}
	}
}
