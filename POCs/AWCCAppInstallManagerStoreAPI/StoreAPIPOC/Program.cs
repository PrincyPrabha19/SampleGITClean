using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store.Preview.InstallControl;
using Windows.Foundation;
using Windows.Management.Deployment;
using Windows.Services.Store;
using Windows.System;

namespace StoreAPIPOC
{
    //case "9pmhp03nj9qp": return "Alienware Command Center"
    //case "9pg0px7tqwvk": return "Alienware FX Systems";     // Centauri R5, Centauri R6, Centauri R7, Cassini MLK 15, Cassini MLK17, Orion 15, Orion 17, Shadowcat CFL R
    //case "9p1w2jsncg39": return "Alienware FX Systems";     // Vulcan 15, Vulcan 15B, Vulcan 17, Serenity 17, Selek, Yamato
    //case "9npkv908zzk3": return "Alienware FX Systems";     // Starhawk, Starhawk AMD, Firefly ,Carnage CML
    //case "9msvccrxmlcp": return "Alienware Sound Center";
    //case "9nbpdgwx4txc": return "Alienware OC Controls";
    //case "9nd9q6nc16lm": return "Alienware Control Center";
    //case "9pmxjsp3qp8k": return "Alienware FXDisplay";

    public class StoreApp
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    class Program
    {
        static List<StoreApp> storeApps = new List<StoreApp>() {
            new StoreApp { Id = "9pmhp03nj9qp", Name = "AWCC" },
            new StoreApp { Id = "9pg0px7tqwvk", Name = "FX" },
            new StoreApp { Id = "9p1w2jsncg39", Name = "FX02" },
            new StoreApp { Id = "9npkv908zzk3", Name = "FXAW20" }, //"Not found (404). (Exception from HRESULT: 0x80190194)"
            new StoreApp { Id = "9msvccrxmlcp", Name = "Sound Center" },
            new StoreApp { Id = "9nbpdgwx4txc", Name = "OC Controls" },
            new StoreApp { Id = "9nd9q6nc16lm", Name = "Control Center" },
            new StoreApp { Id = "9pmxjsp3qp8k", Name = "FXDisplay" },
        };

        static ManualResetEvent opCompletedEvent;

        static void Main(string[] args)
        {
            for (var i = 0; i < storeApps.Count; i++)
            {
                Console.WriteLine($"{i} - {storeApps[i].Name}");
            }

            Console.Write("\nSelect option:");
            var input = Console.ReadLine();
            if (!Int32.TryParse(input, out int number) || number < 0 || number > storeApps.Count)
            {
                Console.WriteLine("Invalid option!");
                return;
            }

            var storeAppId = storeApps[number].Id;
            var storeAppName = storeApps[number].Name;

            //var entitlementStatus = getFreeUserEntitlement(storeAppId);
            //if (entitlementStatus != GetEntitlementStatus.Succeeded)
            //    Console.WriteLine($"Failed to get free user entitlement for {storeAppName} error: {entitlementStatus}");
            //else
            //{
            //    Console.WriteLine($"Successfully get free user entitlement for {storeAppName}, installing app...");
            //    installPackages(storeAppId);
            //}

            //installPackages(storeAppId);

            opCompletedEvent = new ManualResetEvent(false);
            Task.Run(async () => { await installPackagesAsync(storeAppId); }).Wait();
            opCompletedEvent.WaitOne();

            Console.WriteLine("Done!!!!");
        }

        static void installPackages(string productId)
        {
            var options = new AppInstallOptions();
            if (getWindowsBuild(out int windowsBuild) && windowsBuild >= 17763) //RS5+ supports RemovalOptions.RemoveForAllUsers
                options = new AppInstallOptions()
                {
                    AllowForcedAppRestart = true,
                    ForceUseOfNonRemovableStorage = true,
                    CompletedInstallToastNotificationMode = AppInstallationToastNotificationMode.Toast,
                    InstallInProgressToastNotificationMode = AppInstallationToastNotificationMode.Toast,
                    //InstallForAllUsers = false
                };

            AppInstallManager manager = new AppInstallManager();
            IAsyncOperation<IReadOnlyList<AppInstallItem>> operation = manager.StartProductInstallAsync(productId, "", "", "", options);
            ManualResetEvent opCompletedEvent = new ManualResetEvent(false);
            operation.Completed = (depProgress, status) =>
            {
                Console.WriteLine($"Status:{status}");
                opCompletedEvent.Set();
            };
            opCompletedEvent.WaitOne();
        }

        static async Task installPackagesAsync(string productId)
        {
            //Test whether the app we want to launch is installed
            var uri = $"https://www.microsoft.com/store/apps/{productId}";
            var supportStatus = await Launcher.QueryUriSupportAsync(new Uri(uri), LaunchQuerySupportType.Uri);
            //if (supportStatus != LaunchQuerySupportStatus.Available)
            //{
            Console.WriteLine($"Uri: {uri} SupportStatus: {supportStatus}");

            var options = new AppInstallOptions();
            if (getWindowsBuild(out int windowsBuild) && windowsBuild >= 17763) //RS5+ 
                options = new AppInstallOptions()
                {
                    AllowForcedAppRestart = true,
                    ForceUseOfNonRemovableStorage = true,
                    CompletedInstallToastNotificationMode = AppInstallationToastNotificationMode.Toast,
                    InstallInProgressToastNotificationMode = AppInstallationToastNotificationMode.Toast,
                    //InstallForAllUsers = true
                };

            AppInstallManager manager = new AppInstallManager();
            manager.ItemStatusChanged += manager_ItemStatusChanged;
            manager.ItemCompleted += manager_ItemCompleted;
            var installItems = await manager.StartProductInstallAsync(productId, "", "", "", options);

            //var installItems = await manager.StartAppInstallAsync(productId, "0010", true, false);
            //installItems.StatusChanged += new TypedEventHandler<AppInstallItem, System.Object>((app, obj) => StatusChangedUpdate(app, obj));
            //installItems.Completed += new TypedEventHandler<AppInstallItem, System.Object>((app, obj) => CompletedUpdate(app, obj));
            //}
        }

        private static void manager_ItemStatusChanged(AppInstallManager sender, AppInstallManagerItemEventArgs args)
        {
            var status = args.Item.GetCurrentStatus();
            Console.WriteLine($"StatusChanged: PFN: {args.Item.PackageFamilyName}\tTotalBytes: {status.DownloadSizeInBytes}\tBytesDownloaded: {status.BytesDownloaded}\t {status.PercentComplete}%\tInstallState:{status.InstallState}");
            if (status.InstallState == AppInstallState.Error)
                opCompletedEvent.Set();
        }

        private static void manager_ItemCompleted(AppInstallManager sender, AppInstallManagerItemEventArgs args)
        {
            var status = args.Item.GetCurrentStatus();
            Console.WriteLine($"Completed:     PFN: {args.Item.PackageFamilyName}\tTotalBytes: {status.DownloadSizeInBytes}\tBytesDownloaded: {status.BytesDownloaded}\t {status.PercentComplete}%\tInstallState:{status.InstallState}");
            opCompletedEvent.Set();
        }

        //static void StatusChangedUpdate(AppInstallItem app, object obj)
        //{
        //    Console.WriteLine($"StatusChangedUpdate: PFN: {app.PackageFamilyName} PID: {app.ProductId}");
        //}

        //static void CompletedUpdate(AppInstallItem app, object obj)
        //{
        //    Console.WriteLine($"CompletedUpdate: PFN: {app.PackageFamilyName} PID: {app.ProductId}");
        //}

        static GetEntitlementStatus getFreeUserEntitlement(string storeId)
        {
            AppInstallManager manager = new AppInstallManager();

            IAsyncOperation<GetEntitlementResult> operation = manager.GetFreeUserEntitlementAsync(storeId, "", "");
            ManualResetEvent opCompletedEvent = new ManualResetEvent(false);
            operation.Completed = (depProgress, status) =>
            {
                Console.WriteLine($"Status:{status}");
                opCompletedEvent.Set();
            };
            opCompletedEvent.WaitOne();

            return operation.GetResults().Status;
        }

        static bool getWindowsBuild(out int windowsBuild)
        {
            windowsBuild = 0;

            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false))
                {
                    if (key != null)
                    {
                        var _windowsBuild = key.GetValue("CurrentBuild");
                        if (_windowsBuild != null)
                        {
                            if (int.TryParse(_windowsBuild.ToString(), out int __windowsBuild))
                            {
                                windowsBuild = __windowsBuild;
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }

            return false;
        }
    }
}
