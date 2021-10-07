namespace Dominator.ServiceModel.Classes.Monitoring
{
    public class CPUMonitor : Monitor
    {
        protected override void InitDataProviders()
        {
            dataService = new DataService();
            dataService.Initialize();
        }

        protected override void CleanDataProviders()
        {
            dataService.Release();
            dataService = null;
        }
    }
}
