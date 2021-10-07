using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using AWCC.KeystrokesDetector.Enums;
using MacroKeysSimulator.Classes;

namespace MacroKeysSimulator
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void macroKeyPress(object sender, EventArgs e)
        {
            var scanCode = convertToInt((sender as Button)?.Tag as string);
            if (scanCode == 0)
                return;

            Debug.WriteLine($"ScanCode: {scanCode} == 0x{scanCode:X}");

            Thread.Sleep(5000);
            KeyEmulator.KeyDownWithScanCode(scanCode, Win32InputAPI.EXTENDEDKEY_FLAG);
            KeyEmulator.KeyUpWithScanCode(scanCode, Win32InputAPI.EXTENDEDKEY_FLAG);
        }

        private byte convertToInt(string hexStringValue)
        {
            try
            {
                return (byte)(string.IsNullOrEmpty(hexStringValue) ? 0 : Convert.ToInt32(hexStringValue, 16));
            }
            catch {}

            return 0;
        }
    }
}
