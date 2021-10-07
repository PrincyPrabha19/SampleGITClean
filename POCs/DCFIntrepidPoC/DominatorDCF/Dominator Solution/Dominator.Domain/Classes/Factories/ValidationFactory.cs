using System;
using Dominator.Domain.Classes.Helpers;
using Microsoft.Win32;

namespace Dominator.Domain.Classes.Factories
{
    public static class ValidationFactory
    {
        private static IValidationManager validationManager;
        public static IValidationManager NewValidationManager()
        {
            var tuningManager = TuningFactory.NewTuningManager();

            if (validationManager == null)
            {
                int validationDuration;
                if (!readValidationDuration(out validationDuration))
                    validationDuration = 30;

                validationManager = new ValidationManager
                {
                    CPUValidation = new CPUValidation { TuningManager = tuningManager, MonitorManager = MonitorFactory.NewMonitorManager() },
                    MemoryValidation = new MemoryValidation { TuningManager = tuningManager },
                    MonitorManager = MonitorFactory.NewMonitorManager(),
                    StressTest = new BasicStressTest(validationDuration),
                    CPUDataConfigService = CPUDataConfigServiceFactory.NewCPUDataConfigService()
                };
            }

            return validationManager;
        }

        private static bool readValidationDuration(out int seconds)
        {
            seconds = 0;

            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(@"SOFTWARE\Alienware\OC Controls", false))
                {
                    var o = key?.GetValue("ValidationDuration");
                    if (o != null)
                    {
                        seconds = Convert.ToInt32(o);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
            }

            return false;
        }
    }
}
