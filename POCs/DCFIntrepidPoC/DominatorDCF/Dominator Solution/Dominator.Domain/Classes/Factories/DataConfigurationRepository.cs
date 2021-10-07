using Dominator.Domain.Classes.Helpers;

namespace Dominator.Domain.Classes.Factories
{
    public static class DataConfigurationRepository
    {
        public static IDataConfigurationReader ConfigurationReader
        {
            get
            {
                var configurationReader = new DataConfigurationReader();
                configurationReader.Initialize();
                return configurationReader;
            }
        }
    }
}
