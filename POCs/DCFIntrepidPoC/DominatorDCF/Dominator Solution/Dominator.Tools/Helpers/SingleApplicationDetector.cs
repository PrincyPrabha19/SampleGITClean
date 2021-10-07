using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Dominator.Tools.Helpers
{
    public static class SingleApplicationDetector
    {
        public static bool InstanceWasCreated => semaphore != null;

        private static Semaphore semaphore;

        public static bool IsRunning()
        {
            string guid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value;
            var semaphoreName = @"Global\" + guid;
            try
            {
                bool created;
                var sm = new Semaphore(1, 1, semaphoreName, out created);
                if (!created) return true;
                semaphore = sm;
            }
            catch (Exception ex)
            {                                
            }

            return false;
        }        

        public static void Close()
        {
            if (semaphore != null)
            {
                semaphore.Close();
                semaphore = null;
            }
        }        
    }
}