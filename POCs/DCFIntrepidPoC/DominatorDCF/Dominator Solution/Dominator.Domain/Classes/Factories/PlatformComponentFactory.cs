using Dominator.Domain.Classes.Models;

namespace Dominator.Domain.Classes.Factories
{
    public static class PlatformComponentFactory
    {
        private static IPlatformComponentManager platformComponentManager;
        public static IPlatformComponentManager CreatePlatformComponentManager()
        {
            if (platformComponentManager == null)
            {
                platformComponentManager = new PlatformComponentManager();
                platformComponentManager.Initialize();
                return platformComponentManager;
            }
            return platformComponentManager;
        }
    }
}
