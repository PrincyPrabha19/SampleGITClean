using System.Collections.Generic;
using System.Linq;

namespace KeyboardEventsWatcher
{
    public class KeyboardRawInputProcessorClass
    {
        public List<int> Keystrokes { get; private set; }
        private readonly List<int> keystrokesReceived = new List<int>();

        public void RegisterKeystrokes(int[] keystrokes)
        {
            Keystrokes = new List<int>(keystrokes);
        }

        public bool DetectKeystrokes(int keystroke, ushort flags)
        {
            if (!Keystrokes.Exists(k => k == keystroke))
                return false;

            if ((flags & (ushort)Win32InputAPI.RawKeyboardFlags.KeyBreak) == (ushort)Win32InputAPI.RawKeyboardFlags.KeyBreak)
            {
                keystrokesReceived.Remove(keystroke);
                return false;
            }

            if ((flags & (ushort)Win32InputAPI.RawKeyboardFlags.KeyMake) == (ushort)Win32InputAPI.RawKeyboardFlags.KeyMake)
            {
                if (!keystrokesReceived.Exists(k => k == keystroke))
                    keystrokesReceived.Add(keystroke);
                return keystrokesReceived.Count == Keystrokes.Count;
            }

            return false;
        }
    }
}
