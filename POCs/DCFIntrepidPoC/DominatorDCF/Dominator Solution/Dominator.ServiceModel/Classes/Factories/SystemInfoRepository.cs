using System.Threading.Tasks;

namespace Dominator.ServiceModel.Classes.Factories
{
    public static class SystemInfoRepository
    {
        private static ISystemInfo instance;

        public static ISystemInfo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SystemInfo.SystemInfo();
                    instance.Initialize();
                }

                return instance;
            }
        }

        public static async Task<ISystemInfo> GetInstanceAsync()
        {
            if (instance == null)
            {
                instance = new SystemInfo.SystemInfo();
                await instance.InitializeAsync();
            }

            return instance;
        }
    }
}
