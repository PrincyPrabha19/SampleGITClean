namespace Server
{
    public sealed class HostProcessManager
    {
        #region dllhost process
        public int ProcessId => System.Diagnostics.Process.GetCurrentProcess().Id;

        public void KillProcess()
        {
            System.Diagnostics.Process.GetProcessById(ProcessId).Kill();
        }
        #endregion
    }
}
