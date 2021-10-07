using System.Diagnostics;

namespace Dominator.Tools.Helpers
{
    public static class ApplicationInstances
    {
        public static bool IsApplicationInstanceRunning(string applicationName)
        {
            var processes = Process.GetProcessesByName(applicationName);
            return processes.Length > 1;
        }
    }
}