using System;
using System.Runtime.InteropServices;

namespace KeyboardEventsWatcher
{
	public class KeyboardRawInputRegisterClass
	{
		public virtual Win32InputAPI.RAWINPUTDEVICE CreateKeyboardRawInputDevice(IntPtr windowHandle, Win32InputAPI.RawInputDeviceFlags flags)
		{
			return new Win32InputAPI.RAWINPUTDEVICE
			{
				WindowHandle = windowHandle,
				UsagePage = Win32InputAPI.HIDUsagePage.Generic,
				Usage = Win32InputAPI.HIDUsage.Keyboard,
				Flags = flags
			};
		}

		public virtual bool RegisterKeyboard(IntPtr windowHandle)
		{
			var devices = new Win32InputAPI.RAWINPUTDEVICE[1] { CreateKeyboardRawInputDevice(windowHandle, Win32InputAPI.RawInputDeviceFlags.InputSink) };
			return RegisterRawInputDevices(devices, 1, Marshal.SizeOf(typeof(Win32InputAPI.RAWINPUTDEVICE)));
		}

		public virtual bool UnRegisterKeyboard()
		{
			var devices = new Win32InputAPI.RAWINPUTDEVICE[1] { CreateKeyboardRawInputDevice(IntPtr.Zero, Win32InputAPI.RawInputDeviceFlags.Remove) };
			return RegisterRawInputDevices(devices, 1, Marshal.SizeOf(typeof(Win32InputAPI.RAWINPUTDEVICE)));
		}

		public virtual bool RegisterRawInputDevices(Win32InputAPI.RAWINPUTDEVICE[] devices, int numDevices, int size)
		{
			return Win32InputAPI.RegisterRawInputDevices(devices, numDevices, size);
		}
	}
}
