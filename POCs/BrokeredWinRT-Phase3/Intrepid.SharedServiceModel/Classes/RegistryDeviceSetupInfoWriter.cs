namespace Server
{
    public sealed class RegistryDeviceSetupInfoWriter : DeviceSetupInfoWriter
    {
        public RegistryService RegistryAPI { get; set; }

        public void Write(string vendorId, string productId)
        {
            try
            {
                var registryName = $"VID_{vendorId}&PID_{productId}";
                RegistryAPI.CreateSubKey(registryName);
            }
            catch
            {                
            }
        }
    }
}