using System;
using System.Runtime.InteropServices;

namespace KeyboardEventsWatcher
{
	public class KeyboardRawInputExtractorClass
	{
		public bool ReadKeyboardRawInput(IntPtr rawInputHandle, out Win32InputAPI.RAWINPUTKEYBOARD rawInput)
		{
			bool ok = false;
			var input = new Win32InputAPI.RAWINPUT();
			uint size = 0;
			Win32InputAPI.GetRawInputData(rawInputHandle, Win32InputAPI.RawInputCommand.Input, IntPtr.Zero, ref size, (uint)Marshal.SizeOf(typeof(Win32InputAPI.RAWINPUTHEADER)));

			var buffer = Marshal.AllocHGlobal((int)size);
			if (Win32InputAPI.GetRawInputData(rawInputHandle, Win32InputAPI.RawInputCommand.Input, buffer, ref size, (uint)Marshal.SizeOf(typeof(Win32InputAPI.RAWINPUTHEADER))) == size)
			{
				input = (Win32InputAPI.RAWINPUT)Marshal.PtrToStructure(buffer, typeof(Win32InputAPI.RAWINPUT));
				ok = true;
			}
			
			rawInput = input.Data.Keyboard;
			return (ok && (input.Header.Type == Win32InputAPI.RawInputType.Keyboard));
		}
	}
}
