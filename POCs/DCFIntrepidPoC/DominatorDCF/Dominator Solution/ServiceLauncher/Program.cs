using Dominator.ServiceModel.Classes.Helpers;

namespace ServiceLauncher
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
                return 0;

            if(args[0] == "dstop")
                DominatorWindowsServiceHelper.Stop();
            else if (args[0] == "dstar")
                return DominatorWindowsServiceHelper.Start() ? 0:-1;
            else
               return XTUWindowsServiceHelper.Start() ? 0 : -1;
            return 0;
        }
    }
}
