namespace Server
{
	public interface DeviceSetupInfoWriter
	{
		void Write(string vendorId, string productId);
	}
}