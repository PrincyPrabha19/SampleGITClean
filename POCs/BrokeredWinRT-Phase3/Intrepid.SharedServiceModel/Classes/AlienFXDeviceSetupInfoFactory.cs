namespace Server
{
	public sealed class AlienFXDeviceSetupInfoFactory
	{
		public static AlienFXDeviceSetupInfo NewAlienFXDeviceSetupInfo(string vId, string pID)
		{
			return new AlienFXDeviceSetupInfo { VendorId = vId, ProductId = pID };
		}
	}
}