using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace KeyboardEventsWatcher
{
	public partial class Form1 : Form
	{
		private readonly KeyboardRawInputRegisterClass keyboardRawInputRegister = new KeyboardRawInputRegisterClass();
		private readonly KeyboardRawInputExtractorClass keyboardRawInputExtractor = new KeyboardRawInputExtractorClass();
        private readonly KeyboardRawInputProcessorClass keyboardRawInputProcessor = new KeyboardRawInputProcessorClass();

		public Form1()
		{
			InitializeComponent();
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			switch (m.Msg)
			{
				case Win32InputAPI.WM_INPUT:
					processRawInput(m.LParam);
					break;
			}
		}

		private void load(object sender, EventArgs e)
		{
			keyboardRawInputRegister.RegisterKeyboard(Handle);
            //keyboardRawInputWatcher.RegisterKeystrokes(new[] { 0x5B, 0x72 }); //0x5B = WIN 0x72 = F3
            keyboardRawInputProcessor.RegisterKeystrokes(new[] { 0x10, 0x11, 0x72 }); // SHIFT+CTRL+F3
		}



		private void processRawInput(IntPtr rawInputHandle)
		{
			Win32InputAPI.RAWINPUTKEYBOARD data;
			if (keyboardRawInputExtractor.ReadKeyboardRawInput(rawInputHandle, out data))
			{
                if (keyboardRawInputProcessor.DetectKeystrokes(data.VirtualKey, (ushort)data.Flags))
			        Debug.WriteLine(DateTime.Now + " --> Key combination detected!");

                logTXT.Text += string.Format("MakeCode: 0x{0:X}, Flags: 0x{1:X}, Message: 0x{2:X}, VirtualKey: 0x{3:X}, ExtraInformation: {4}, Reserved: {5}{6}",
                    data.MakeCode, data.Flags, data.Message, data.VirtualKey, data.ExtraInformation, data.Reserved, Environment.NewLine);

                logTXT.SelectionStart = logTXT.TextLength;
                logTXT.SelectionLength = 0;
                logTXT.ScrollToCaret();
			}
		}

		private void buttonClear_Click(object sender, EventArgs e)
		{
			logTXT.Clear();
		}
	}
}
