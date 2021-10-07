using System;
using System.IO;

namespace Server
{
	public sealed class AlienFXAPIWrapper
	{
		#region Properties
		private static readonly bool is64BitsApp = (IntPtr.Size == 8);
		#endregion

		#region Methods
		public static int Initialize()
		{
            log("Initialize");
            return is64BitsApp
				? AlienFXBiosSupportAPI64Map.Initialize()
				: AlienFXBiosSupportAPI32Map.Initialize();
		}

		public static int Release()
		{
            log("Release");
            return is64BitsApp
				? AlienFXBiosSupportAPI64Map.Release()
				: AlienFXBiosSupportAPI32Map.Release();
		}

		public static int SetLightColor(uint leds, uint color)
		{
            log($"SetLightColor ===> LEDs: {leds:X2}, Color: {color:X8}");
            return is64BitsApp
				? AlienFXBiosSupportAPI64Map.SetLightColor(leds, color)
				: AlienFXBiosSupportAPI32Map.SetLightColor(leds, color);
		}

		public static int SetLightColorDataForPState(uint leds, uint color, int state)
		{
            log($"SetLightColorDataForPState ===> LEDs: {leds:X2}, Color: {color:X8}");
            return is64BitsApp
				? AlienFXBiosSupportAPI64Map.SetDefaultColor(leds, color)
				: AlienFXBiosSupportAPI32Map.SetDefaultColor(leds, color);
		}

        public static int SetDimligths(uint leds, int brightness)
        {
            log($"SetBrightness ===> LEDs: {leds:X2}, Brightness: {brightness:X2}");
            return is64BitsApp
                ? AlienFXBiosSupportAPI64Map.SetBrightness(brightness)
                : AlienFXBiosSupportAPI32Map.SetBrightness(brightness);
        }

        public static void log(string msg)
        {
            if (!Directory.Exists(@"c:\temp\")) return;

            using (var sw = new StreamWriter(@"c:\temp\apicalls.txt", true))
                sw.WriteLine(msg);
        }
	    #endregion
    }
}