using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dell.Asimov.Configuration;
using Dell.Asimov.Update;
using Dell.Asimov.UpdateClient;
using Dell.Asimov.UserSettings.Configuration;
using Dell.Asimov.UserSettings.Configuration.Constants;

namespace DCUClient
{
    class Program
    {
        private static ServiceStateMonitor _monitor;
        public static ExtendedApplicableUpdatesData ApplicableUpdatesExtendendScanData { get; set; }
        public static ExtendedUpdatesApplyResult result { get; set; }
        //public static InventoryData CompleteInventoryDataForCurrentSystem { get; set; }
        static async Task<int> Main(string[] args)
        {
            int failedres_count = 0;
            try
            {
                
                using (CancellationTokenSource tokenSource = new CancellationTokenSource())
                {
                    ApplicationInfo AppInfo = new ApplicationInfo("AWCCInstaller", "1.0.0.0", true);
                    List<DellUpdatePackageInfo> PackagesToInstall = new List<DellUpdatePackageInfo>();
                    using (var updateClient = new UpdateClient(AppInfo))
                    {
                        var progress = new Progress<OperationProgress>();
                        progress.ProgressChanged += (sender, e) => { Console.WriteLine("Progress:" + e.Stage); };

                        _monitor = StateMonitorFactory.CreateClientInstance();
                        _monitor.StateChangedEvent += MonitorStateChangedEvent;
                        ServiceStateMonitor monitor = StateMonitorFactory.GetInstance();
                        var state = monitor.CurrentServiceState;


                        await Task.Run(() =>
                                ApplicableUpdatesExtendendScanData =
                                    updateClient.GetApplicableUpdatesDataAsync(tokenSource.Token, progress).Result)
                            .ConfigureAwait(false);
                        Console.Out.WriteLine("Total number of applicable packages=" + ApplicableUpdatesExtendendScanData.PdkUpdates.Count.ToString());
                        foreach (var pkgInfo in ApplicableUpdatesExtendendScanData.PdkUpdates)
                        {
                            
                            if (pkgInfo.Category == Category.Audio && pkgInfo.ComponentType == ComponentType.Driver)
                            {
                                Console.WriteLine("Audio driver package ready to Install" + pkgInfo.Name);
                                PackagesToInstall.Add(pkgInfo);
                            }
                           

                        }
                        if (PackagesToInstall.Count > 0)
                        {
                            Console.WriteLine("Downloading and Installing packages:" + PackagesToInstall.Count);
                        }
                        else
                        {
                            Console.Out.WriteLine("No Audio drivers to Install");
                        }
                        // Made it Synchronous waits for 5 min to complete the installation, in case the call hangs
                        Task.Run(() => result = updateClient.ApplyUpdatesAsync(PackagesToInstall, tokenSource.Token).Result).Wait(1000 * 60 * 5);
                        if (result != null)
                        {
                            var resultlist = result.UpdateComponentApplyResultList;
                            foreach (var res in resultlist)
                            {
                                if (res.Code == ExtensibleApplyResultCode.Failed)
                                {
                                    Console.Out.WriteLine("Failed to install package" + res.UpdateComponent.Name);
                                    failedres_count++;
                                }
                                else
                                {
                                    Console.Out.WriteLine("Sucessfully installed package" + res.UpdateComponent.Name);
                                }
                            }
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (failedres_count > 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
            
        }

        private static void MonitorStateChangedEvent(object sender, ServiceStateChangedEventArgs e)
        {
            Console.WriteLine("SSM new state:" + e.NewState);
        }
    }
}
