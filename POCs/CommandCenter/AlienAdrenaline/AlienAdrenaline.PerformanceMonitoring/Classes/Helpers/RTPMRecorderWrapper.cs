using System.Diagnostics;
using System.IO;
using AlienLabs.CommandCenter.Tools.Classes;
using AlienLabs.Tools.Classes;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes.Helpers
{
	public static class RTPMRecorderWrapper
	{
		#region Properties
		const string FILEPREFIX_PARAM = "/prefix:";
		const string FILENAME_PARAM = "/file:";
		#endregion

		#region Methods
		public static void StartRecording()
		{
			StartRecording("", "");
		}

		public static void StartRecording(string profileName, string prefix)
		{
			if (IsRecording())
				return;

			runRTPMRecorder(profileName, prefix);
		}

		public static void StopRecording()
		{
			if (!IsRecording())
				return;

			var stopEvent = new NamedEvent("STOP_RECORDING", false, true);
			stopEvent.Set();
		}

		public static bool IsRecording()
		{
			var programProcess = Process.GetProcessesByName("RTPMRecorder");
			return (programProcess.Length != 0);
		}

		private static void runRTPMRecorder(string profileName, string prefix)
		{
			var fileName = Path.Combine(ApplicationSettings.StartupPath, "RTPMRecorder.exe");
			var arg = (string.IsNullOrEmpty(profileName)) ? "" : string.Format("\"{0}{1}\"", FILENAME_PARAM, profileName);
			arg += (string.IsNullOrEmpty(prefix)) ? "" : string.Format(" \"{0}{1}\"", FILEPREFIX_PARAM, prefix);

			try { Process.Start(fileName, arg); }
			catch { }
		}
		#endregion
	}
}
