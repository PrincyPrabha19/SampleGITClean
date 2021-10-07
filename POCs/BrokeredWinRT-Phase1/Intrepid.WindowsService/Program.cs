using System.ServiceProcess;

namespace Intrepid.WindowsService
{
    static class Program
    {
        static void Main()
        {
            var servicesToRun = new ServiceBase[] { new IntrepidWindowsService() };
            ServiceBase.Run(servicesToRun);
        }
    }
}
