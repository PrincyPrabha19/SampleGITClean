using System.ServiceProcess;
using System.Threading;
using Intrepid.WindowsService.Classes;

namespace Intrepid.WindowsService
{
    public partial class IntrepidWindowsService : ServiceBase
    {
        public IntrepidWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //while (!System.Diagnostics.Debugger.IsAttached) Thread.Sleep(100);

            BIOSSupportHelper.StartWCFService();
        }

        protected override void OnStop()
        {
            BIOSSupportHelper.StopWCFService();
        }
    }
}
