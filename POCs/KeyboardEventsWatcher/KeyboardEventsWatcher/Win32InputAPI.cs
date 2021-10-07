﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace KeyboardEventsWatcher
{
	public class Win32InputAPI
	{
		#region Methods
		[DllImport("user32.dll")]
		public static extern bool RegisterRawInputDevices([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] RAWINPUTDEVICE[] pRawInputDevices, int uiNumDevices, int cbSize);

		[DllImport("user32.dll")]
		public static extern uint GetRawInputData(IntPtr hRawInput, RawInputCommand uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);

		[DllImport("user32.dll", EntryPoint = "keybd_event")]
		public static extern void KeyBDEvent(byte vk, byte scanCode, uint flags, UIntPtr extraInfo);

		[DllImport("user32.dll")]
		public static extern byte MapVirtualKey(uint code, uint mapType);

		[DllImport("user32.dll")]
		public static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

		[DllImport("user32.dll")]
		public static extern short VkKeyScanEx(char ch, IntPtr keyboardLayout);

		[DllImport("user32.dll")]
		public static extern IntPtr GetKeyboardLayout(uint idThread);

		[DllImport("user32", CharSet = CharSet.Unicode, EntryPoint = "GetKeyNameTextW")]
		public static extern int GetKeyName(uint param, StringBuilder keyName, int size);

		public const uint EXTENDEDKEY_FLAG = 0x0001;
		public const uint KEYUP_FLAG = 0x0002;
		public const uint UNICODE_FLAG = 0x0004;
		public const int WM_INPUT = 0x00FF;

		public const int WM_POWERBROADCAST = 0x0218;
		public const int PBT_APMRESUMEAUTOMATIC = 0x12;
		#endregion

		#region Structs
		[StructLayout(LayoutKind.Sequential)]
		public struct RAWINPUTDEVICE
		{
			public HIDUsagePage UsagePage;
			public HIDUsage Usage;
			public RawInputDeviceFlags Flags;
			public IntPtr WindowHandle;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RAWINPUTHEADER
		{
			public RawInputType Type;
			public int Size;
			public IntPtr Device;
			public IntPtr wParam;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RAWINPUTKEYBOARD
		{
			public short MakeCode;
			public RawKeyboardFlags Flags;
			public short Reserved;
			public ushort VirtualKey;
			public int Message;
			public int ExtraInformation;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RAWINPUT
		{
			public RAWINPUTHEADER Header;
			public RAWDATA Data;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct RAWDATA
		{
			/// <summary>
			/// Keyboard raw input data.
			/// </summary>
			[FieldOffset(0)]
			public RAWINPUTKEYBOARD Keyboard;
		}

		public struct KEYBDINPUT 
		{  
			public ushort VirtualKey;
			public ushort ScanCode;
			public uint Flags;
			public long Time;  
			public uint ExtraInfo; 
		};
		
		[StructLayout(LayoutKind.Explicit, Size=28)]
		public struct INPUT 
		{
			[FieldOffset(0)]
			public uint Type;
			[FieldOffset(4)]
			public KEYBDINPUT KeyBDInput;
		};
		#endregion

		#region Enums
		public enum HIDUsagePage : ushort
		{
			Undefined = 0x00,
			Generic = 0x01,
			Simulation = 0x02,
			VR = 0x03,
			Sport = 0x04,
			Game = 0x05,
			Keyboard = 0x07,
			LED = 0x08,
			Button = 0x09,
			Ordinal = 0x0A,
			Telephony = 0x0B,
			Consumer = 0x0C,
			Digitizer = 0x0D,
			PID = 0x0F,
			Unicode = 0x10,
			AlphaNumeric = 0x14,
			Medical = 0x40,
			MonitorPage0 = 0x80,
			MonitorPage1 = 0x81,
			MonitorPage2 = 0x82,
			MonitorPage3 = 0x83,
			PowerPage0 = 0x84,
			PowerPage1 = 0x85,
			PowerPage2 = 0x86,
			PowerPage3 = 0x87,
			BarCode = 0x8C,
			Scale = 0x8D,
			MSR = 0x8E
		}

		public enum HIDUsage : ushort
		{
			Pointer = 0x01,
			Mouse = 0x02,
			Joystick = 0x04,
			Gamepad = 0x05,
			Keyboard = 0x06,
			Keypad = 0x07,
			SystemControl = 0x80,
			X = 0x30,
			Y = 0x31,
			Z = 0x32,
			RelativeX = 0x33,
			RelativeY = 0x34,
			RelativeZ = 0x35,
			Slider = 0x36,
			Dial = 0x37,
			Wheel = 0x38,
			HatSwitch = 0x39,
			CountedBuffer = 0x3A,
			ByteCount = 0x3B,
			MotionWakeup = 0x3C,
			VX = 0x40,
			VY = 0x41,
			VZ = 0x42,
			VBRX = 0x43,
			VBRY = 0x44,
			VBRZ = 0x45,
			VNO = 0x46,
			SystemControlPower = 0x81,
			SystemControlSleep = 0x82,
			SystemControlWake = 0x83,
			SystemControlContextMenu = 0x84,
			SystemControlMainMenu = 0x85,
			SystemControlApplicationMenu = 0x86,
			SystemControlHelpMenu = 0x87,
			SystemControlMenuExit = 0x88,
			SystemControlMenuSelect = 0x89,
			SystemControlMenuRight = 0x8A,
			SystemControlMenuLeft = 0x8B,
			SystemControlMenuUp = 0x8C,
			SystemControlMenuDown = 0x8D,
			KeyboardNoEvent = 0x00,
			KeyboardRollover = 0x01,
			KeyboardPostFail = 0x02,
			KeyboardUndefined = 0x03,
			KeyboardaA = 0x04,
			KeyboardzZ = 0x1D,
			Keyboard1 = 0x1E,
			Keyboard0 = 0x27,
			KeyboardLeftControl = 0xE0,
			KeyboardLeftShift = 0xE1,
			KeyboardLeftALT = 0xE2,
			KeyboardLeftGUI = 0xE3,
			KeyboardRightControl = 0xE4,
			KeyboardRightShift = 0xE5,
			KeyboardRightALT = 0xE6,
			KeyboardRightGUI = 0xE7,
			KeyboardScrollLock = 0x47,
			KeyboardNumLock = 0x53,
			KeyboardCapsLock = 0x39,
			KeyboardF1 = 0x3A,
			KeyboardF12 = 0x45,
			KeyboardReturn = 0x28,
			KeyboardEscape = 0x29,
			KeyboardDelete = 0x2A,
			KeyboardPrintScreen = 0x46,
			LEDNumLock = 0x01,
			LEDCapsLock = 0x02,
			LEDScrollLock = 0x03,
			LEDCompose = 0x04,
			LEDKana = 0x05,
			LEDPower = 0x06,
			LEDShift = 0x07,
			LEDDoNotDisturb = 0x08,
			LEDMute = 0x09,
			LEDToneEnable = 0x0A,
			LEDHighCutFilter = 0x0B,
			LEDLowCutFilter = 0x0C,
			LEDEqualizerEnable = 0x0D,
			LEDSoundFieldOn = 0x0E,
			LEDSurroundFieldOn = 0x0F,
			LEDRepeat = 0x10,
			LEDStereo = 0x11,
			LEDSamplingRateDirect = 0x12,
			LEDSpinning = 0x13,
			LEDCAV = 0x14,
			LEDCLV = 0x15,
			LEDRecordingFormatDet = 0x16,
			LEDOffHook = 0x17,
			LEDRing = 0x18,
			LEDMessageWaiting = 0x19,
			LEDDataMode = 0x1A,
			LEDBatteryOperation = 0x1B,
			LEDBatteryOK = 0x1C,
			LEDBatteryLow = 0x1D,
			LEDSpeaker = 0x1E,
			LEDHeadset = 0x1F,
			LEDHold = 0x20,
			LEDMicrophone = 0x21,
			LEDCoverage = 0x22,
			LEDNightMode = 0x23,
			LEDSendCalls = 0x24,
			LEDCallPickup = 0x25,
			LEDConference = 0x26,
			LEDStandBy = 0x27,
			LEDCameraOn = 0x28,
			LEDCameraOff = 0x29,
			LEDOnLine = 0x2A,
			LEDOffLine = 0x2B,
			LEDBusy = 0x2C,
			LEDReady = 0x2D,
			LEDPaperOut = 0x2E,
			LEDPaperJam = 0x2F,
			LEDRemote = 0x30,
			LEDForward = 0x31,
			LEDReverse = 0x32,
			LEDStop = 0x33,
			LEDRewind = 0x34,
			LEDFastForward = 0x35,
			LEDPlay = 0x36,
			LEDPause = 0x37,
			LEDRecord = 0x38,
			LEDError = 0x39,
			LEDSelectedIndicator = 0x3A,
			LEDInUseIndicator = 0x3B,
			LEDMultiModeIndicator = 0x3C,
			LEDIndicatorOn = 0x3D,
			LEDIndicatorFlash = 0x3E,
			LEDIndicatorSlowBlink = 0x3F,
			LEDIndicatorFastBlink = 0x40,
			LEDIndicatorOff = 0x41,
			LEDFlashOnTime = 0x42,
			LEDSlowBlinkOnTime = 0x43,
			LEDSlowBlinkOffTime = 0x44,
			LEDFastBlinkOnTime = 0x45,
			LEDFastBlinkOffTime = 0x46,
			LEDIndicatorColor = 0x47,
			LEDRed = 0x48,
			LEDGreen = 0x49,
			LEDAmber = 0x4A,
			LEDGenericIndicator = 0x3B,
			TelephonyPhone = 0x01,
			TelephonyAnsweringMachine = 0x02,
			TelephonyMessageControls = 0x03,
			TelephonyHandset = 0x04,
			TelephonyHeadset = 0x05,
			TelephonyKeypad = 0x06,
			TelephonyProgrammableButton = 0x07,
			SimulationRudder = 0xBA,
			SimulationThrottle = 0xBB
		}

		[Flags]
		public enum RawInputDeviceFlags
		{
			None = 0,
			Remove = 0x00000001,
			Exclude = 0x00000010,
			PageOnly = 0x00000020,
			NoLegacy = 0x00000030,
			InputSink = 0x00000100,
			CaptureMouse = 0x00000200,
			NoHotKeys = 0x00000200,
			AppKeys = 0x00000400
		}

		public enum RawInputType
		{
			Mouse = 0,
			Keyboard = 1,
			HID = 2
		}

		[Flags]
		public enum RawKeyboardFlags : ushort
		{
			KeyMake = 0,
			KeyBreak = 1,
			KeyE0 = 2,
			KeyE1 = 4,
			TerminalServerSetLED = 8,
			TerminalServerShadow = 0x10
		}

		public enum RawInputCommand
		{
			Input = 0x10000003,
			Header = 0x10000005
		}

		public enum SpecialVirtualKeys : ushort
		{
			Shift		 = 0x10,
			Control		 = 0x11,
			Menu		 = 0x12,
			LeftWindows  = 0x5B,
			RightWindows = 0x5C,
			LeftShift	 = 0xA0,
			RightShift	 = 0xA1,
			LeftControl  = 0xA2,
			RightControl = 0xA3,
			LeftMenu	 = 0xA4,
			RightMenu	 = 0xA5,
		}

		public enum MapVirtualKeyConstants : uint
		{
			MapvkVkToVsc   = 0x00,
			MapvkVscToVk   = 0x01,
			MapvkVkToChar  = 0x02,
			MapvkVscToVkEx = 0x03,
			MapvkVkToVscEx = 0x04	
		}
		#endregion
	}
}
