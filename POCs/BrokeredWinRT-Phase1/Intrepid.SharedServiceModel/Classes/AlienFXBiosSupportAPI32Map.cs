using System.Runtime.InteropServices;

namespace Server
{
	public sealed class AlienFXBiosSupportAPI32Map
	{
		/// <returns> 0 if successful, 1 if failed, -1 if no BIOS support</returns>
		[DllImport("AlienFXBiosSupportAPI32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Initialize();

		/// <returns> 0 if successful, 1 if failed</returns>
        [DllImport("AlienFXBiosSupportAPI32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Release();

		/// <returns> 0 if successful, 1 if failed, -1 DLL not initialized</returns>
		[DllImport("AlienFXBiosSupportAPI32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetLightColor(uint leds, uint color);

		/// <returns> 0 if successful, 1 if failed, -1 DLL not initialized</returns>
		[DllImport("AlienFXBiosSupportAPI32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetDefaultColor(uint leds, uint color);

        /// <returns> 0 if successful, 1 if failed, -1 DLL not initialized</returns>
        [DllImport("AlienFXBiosSupportAPI32", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetBrightness(int percent);
    }
}