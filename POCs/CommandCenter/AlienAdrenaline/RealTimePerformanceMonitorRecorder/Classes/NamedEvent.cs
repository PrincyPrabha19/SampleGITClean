using System;
using System.Runtime.InteropServices;

namespace RealTimePerformanceMonitorRecorder.Classes
{
    public class NamedEvent
    {
        #region Properties
        private readonly string eventName;
        private readonly IntPtr attributes = IntPtr.Zero;
        private readonly bool manualReset;
        private readonly bool initialState;

        public static uint INFINITE = 0xFFFFFFFF;

        private IntPtr handle;
        public IntPtr EventHnd 
        {
            get	{ return handle; }
        }
        #endregion

        #region API
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

        [DllImport("kernel32.dll", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        static extern bool SetEvent(IntPtr hEvent);

        [DllImport("kernel32.dll")]
        static extern bool ResetEvent(IntPtr hEvent);

        [DllImport("kernel32.dll")]
        static extern bool PulseEvent(IntPtr hEvent);

        [DllImport("kernel32", SetLastError=true, ExactSpelling=true)]
        internal static extern Int32 WaitForSingleObject(IntPtr handle, Int32 milliseconds);

        [DllImport("kernel32.dll")]
        static extern uint WaitForMultipleObjects(uint nCount, IntPtr [] lpHandles,
                                                  bool bWaitAll, uint dwMilliseconds);
        #endregion

        #region Constructors
        public NamedEvent(string eventName) : this(eventName, false, false)
        {}

		public NamedEvent(string eventName, bool manualReset, bool create)
        {
			this.eventName = eventName;
			this.manualReset = manualReset;
            initialState = false;
            handle = create ? CreateEvent(attributes, manualReset, initialState, eventName) : IntPtr.Zero;
        }
        #endregion

        #region Methods
        public bool Wait(int timeoutInSecs)
        {
            IntPtr localHandle = handle != IntPtr.Zero ? handle : CreateEvent(attributes, manualReset, initialState, eventName);
            int rc = WaitForSingleObject(localHandle, timeoutInSecs == -1 ? (int)INFINITE :  timeoutInSecs * 1000);
            closeHandle(localHandle);
            return rc == 0;
        }

        public void CloseHandle()
        {
            try
            {
                closeHandle(handle);
                handle = IntPtr.Zero;
            }
            catch
            {}
        }

    	private void closeHandle(IntPtr handleToClose)
        {
            try
            {
            	if (handleToClose.ToInt32() != 0)
            		CloseHandle(handleToClose);
            }
            catch
            {}
        }

        public bool Pulse()
        {
            IntPtr localHandle =  handle != IntPtr.Zero ? handle : CreateEvent(attributes, manualReset, initialState, eventName);
            PulseEvent(localHandle);
            CloseHandle(localHandle);
            return handle != IntPtr.Zero;
        }

        public void Set()
        {
            IntPtr localHandle = handle != IntPtr.Zero ? handle : CreateEvent(attributes, manualReset, initialState, eventName);
            SetEvent(localHandle);
            CloseHandle(localHandle);
        }

        public void Reset()
        {
            IntPtr localHandle =  handle != IntPtr.Zero ? handle : CreateEvent(attributes, manualReset, initialState, eventName);
            ResetEvent(localHandle);
            CloseHandle(localHandle);
        }

        public static bool Wait(int timeoutInSecs, string name)
        {
            return (new NamedEvent(name)).Wait(timeoutInSecs);
        }

        public static uint WaitForMultipleObjects(IntPtr[] handlers, int timeOutInSecs, bool waitForAll)
        {
            return WaitForMultipleObjects((uint)handlers.Length, handlers, waitForAll, (uint)timeOutInSecs*1000);
        }

        public static bool Pulse(string name) 
        { 
            return (new NamedEvent(name)).Pulse();
        }

        public static void Set(string name) 
        { 
            (new NamedEvent(name)).Set();
        }

        public static void Reset(string name) 
        { 
            (new NamedEvent(name)).Reset();
        }
        #endregion
    }
}