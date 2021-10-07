using System;
using AWCC.KeystrokesDetector.Enums;

namespace MacroKeysSimulator.Classes
{
    public static class KeyEmulator
    {
        public static void KeyDownWithScanCode(byte scanCode, uint flags, bool useVirtualKey = false)
        {
            if ((flags & ((uint)(Win32InputAPI.RawKeyboardFlags.KeyE0 | Win32InputAPI.RawKeyboardFlags.KeyE1))) != 0)
                flags = Win32InputAPI.EXTENDEDKEY_FLAG;

            byte virtualKey = (byte)(useVirtualKey ? Win32InputAPI.MapVirtualKey(scanCode, (uint)Win32InputAPI.MapVirtualKeyConstants.MapvkVscToVk) : 0xFF);
            Win32InputAPI.KeyBDEvent(virtualKey, scanCode, flags, UIntPtr.Zero);
        }

        public static void KeyUpWithScanCode(byte scanCode, uint flags, bool useVirtualKey = false)
        {
            if ((flags & ((uint)(Win32InputAPI.RawKeyboardFlags.KeyE0 | Win32InputAPI.RawKeyboardFlags.KeyE1))) != 0)
                flags = Win32InputAPI.EXTENDEDKEY_FLAG;

            byte virtualKey = (byte)(useVirtualKey ? Win32InputAPI.MapVirtualKey(scanCode, (uint)Win32InputAPI.MapVirtualKeyConstants.MapvkVscToVk) : 0xFF);
            Win32InputAPI.KeyBDEvent(virtualKey, scanCode, flags | Win32InputAPI.KEYUP_FLAG, UIntPtr.Zero);
        }
    }
}
