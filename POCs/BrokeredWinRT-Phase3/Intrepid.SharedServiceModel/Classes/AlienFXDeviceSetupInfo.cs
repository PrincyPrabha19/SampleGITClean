namespace Server
{
	public struct AlienFXDeviceSetupInfo
	{
	    public string VendorId;
	    public string ProductId;
	    public bool IsPresent;
	    public bool IsInstalled;
	}

    public static class AFXSetupInfoHelper
    {
        public static AlienFXDeviceSetupInfo Empty => new AlienFXDeviceSetupInfo
        {
            VendorId = string.Empty,
            ProductId = string.Empty,
            IsInstalled = false,
            IsPresent = false
        };

        public static bool AreEqual(AlienFXDeviceSetupInfo device1, AlienFXDeviceSetupInfo device2)
        {
            return device1.VendorId == device2.VendorId && device1.ProductId == device2.ProductId;
        }
    }
}