using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace AWCCSilentInstall
{
    public partial class Service1 : ServiceBase
    {
       
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //while (!Debugger.IsAttached)
            //    Thread.Sleep(1000);

            installApplication();
        }

        protected override void OnStop()
        {
        }

        private void installApplication()
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    //FileName = @"D:\DEVGIT\Samples\AWCCSilentInstall\AWCCSilentInstall\bin\Debug\AWCC\setup.exe",
                    FileName = @"C:\Temp\setup.exe",
                    Arguments = @"/s /z""nouwp"" /debuglog",
                    Verb = "runas",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };
            var result = process.Start();

            //var result = ApplicationLauncher.CreateProcessInConsoleSession("", @"D:\DEVGIT\Samples\AWCCSilentInstall\AWCCSilentInstall\bin\Debug\AddAppxPackageSample\setup.exe /debuglog", true);
            //var result = ApplicationLauncher.CreateProcessInConsoleSession("", @"D:\DEVGIT\Samples\AWCCSilentInstall\AWCCSilentInstall\bin\Debug\AWCC\setup.exe /s /z""fida"" /debuglog", true);
            //var result = ApplicationLauncher.CreateProcessInConsoleSession("", @"D:\DEVGIT\Samples\AWCCSilentInstall\AWCCSilentInstall\bin\Debug\AWCC\setup.exe /debuglog", true);
        }
    }
}
