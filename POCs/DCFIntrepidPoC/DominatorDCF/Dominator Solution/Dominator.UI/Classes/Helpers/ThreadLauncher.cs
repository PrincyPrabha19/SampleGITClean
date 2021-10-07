using System;
using System.Threading;

namespace Dominator.UI.Classes.Helpers
{
    public static class ThreadLauncher
    {
        public static void Start(Action action)
        {
            var thread = new Thread(action.Invoke);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
