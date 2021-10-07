namespace Server
{
	public sealed class ObjectFactory
	{
		public IAlienFXDeviceDiscoveryService NewAlienFXDeviceDiscovery()
		{
			var discoveryService = new AlienFXDeviceDiscoveryServiceClass
			{
				RegistryDeviceSetupInfoReader = newRegistryDeviceSetupInfoReader(),
				RegistryDeviceSetupInfoWriter = newRegistryDeviceSetupInfoWriter(),
				ModelProvider = newModelProvider()
			};

			return discoveryService;
		}

        private DeviceSetupInfoReader newRegistryDeviceSetupInfoReader()
		{
			return new RegistryDeviceSetupInfoReader { RegistryAPI = newRegistryService() };
		}

        private DeviceSetupInfoWriter newRegistryDeviceSetupInfoWriter()
		{
			return new RegistryDeviceSetupInfoWriter { RegistryAPI = newRegistryService() };
		}

        private RegistryService newRegistryService()
        {
            return new RegistryServiceClass(false,  @"Software", @"Alienware\System\AFXCapableDevices");
        }

	    private ModelProvider newModelProvider()
	    {
	        return new ModelProviderClass
	        {
	            RegistryModelReader = new RegistryModelReader()
	        };
	    }
    }
}