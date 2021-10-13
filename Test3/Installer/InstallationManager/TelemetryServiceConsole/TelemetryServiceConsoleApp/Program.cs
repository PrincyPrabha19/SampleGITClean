using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AWCC.TelemetryClientWin32;

namespace TelemetryServiceConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TelemetryClientWin32 lib = new TelemetryClientWin32();

            lib.SendSimpleEvent("App Uninstall");
        }
    }
}
