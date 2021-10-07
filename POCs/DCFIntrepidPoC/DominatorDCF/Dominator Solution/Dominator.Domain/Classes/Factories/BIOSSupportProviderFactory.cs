namespace Dominator.Domain.Classes.Factories
{
    public static class BIOSSupportProviderFactory
    {
        private static IBIOSSupportProvider biosSupportProvider;
        public static IBIOSSupportProvider NewBIOSSupportProvider()
        {
            if (biosSupportProvider == null)
            {
#if BIOS_REGISTRY_SIMULATION
                biosSupportProvider = new BIOSSupportProviderTest();
#else
                biosSupportProvider = new BIOSSupportProvider();
#endif
                biosSupportProvider.Initialize();
            }

            return biosSupportProvider;
        }
    }
}