using System.Runtime.InteropServices;

namespace Dominator.ServiceModel.Classes.BIOSSupport
{
	public class BIOSSupportAPIMap
    {
		/// <returns> 0 if successful, 1 if failed, 2 if no BIOS support</returns>
		[DllImport("DomOCBiosSupportAPI")]
		public static extern int Initialize();

		/// <returns> -1 DLL not initialized</returns>
		[DllImport("DomOCBiosSupportAPI")]
		public static extern int ReturnOverclockingReport();

        /// <returns> 0 if successful, 1 if failed, -1 DLL not initialized</returns>
        [DllImport("DomOCBiosSupportAPI")]
        public static extern int SetOCUIBIOSControl([MarshalAs(UnmanagedType.Bool)]bool enabled);

        /// <returns> 0 if successful, 1 if failed, -1 DLL not initialized</returns>
        [DllImport("DomOCBiosSupportAPI")]
        public static extern int ClearOCFailSafeFlag();

        /// <returns> 0 if successful, 1 if failed, -1 DLL not initialized</returns>
        [DllImport("DomOCBiosSupportAPI")]
		public static extern int Release();
    }
}