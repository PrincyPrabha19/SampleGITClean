using System.Diagnostics;
using System.IO;
using AlienLabs.CommandCenter.Tools.Classes;

namespace RealTimePerformanceMonitorRecorder.Classes
{
	public class RTPMGadgetOperations
	{
		public static void RunRTPMGadget()
		{
			if (isGadgetOpened())
				return;

			var fileName = Path.Combine(ApplicationSettings.StartupPath, "RTPMGadget.exe");
			try { Process.Start(fileName); }
			catch { }
		}

		private static bool isGadgetOpened()
		{
			var programProcess = Process.GetProcessesByName("RTPMGadget");
			return (programProcess.Length != 0);
		}
	}
}
