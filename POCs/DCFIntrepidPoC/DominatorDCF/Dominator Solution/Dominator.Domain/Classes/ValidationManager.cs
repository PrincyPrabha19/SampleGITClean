using System;
using System.ComponentModel;
using System.Threading;
using Dominator.Domain.Enums;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Classes.Monitoring;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes
{
    public class ValidationManager : IValidationManager
    {
        public ICPUValidation CPUValidation { private get; set; }
        public IMemoryValidation MemoryValidation { private get; set; }
        public IMonitorManager MonitorManager { get; set; }
        public IBasicStressTest StressTest { get; set; }
        public ICPUDataConfigService CPUDataConfigService { private get; set; }
        public bool IsValidationRunning { get; private set; }

        public event Action<int, bool> ProgressChanged;

        private readonly IDataService dataService;
        private readonly ISettingIDGenerator idGenerator;
        private BackgroundWorker checkThrottlingWorker;        
        private ValidationStatus validationStatus;
        private static readonly AutoResetEvent resetEvent = new AutoResetEvent(false);
        private const int WAIT_DURATION = 5;

        public ValidationManager()
        {
            idGenerator = new SettingIDGenerator();
            dataService = new DataService();
            dataService.Initialize();
        }

        public ValidationStatus StartValidation(IProfile currentProfile)
        {
            if (!CPUValidation.ApplySettings(currentProfile)) return ValidationStatus.Invalidated;
            if (!MemoryValidation.ApplySettings(currentProfile)) return ValidationStatus.Invalidated;

            IsValidationRunning = true;

            seconds = 0;
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += timer_Elapsed;
            timer.Start();

            if (resetEvent.WaitOne(5000))
            {
                timer.Stop();
                timer.Dispose();
                IsValidationRunning = false;
                return ValidationStatus.ValidationCancelled;
            }

            ProgressChanged?.Invoke(100, false);
            timer.Stop();
            timer.Dispose();

            if (reportThrottling())
            {
                IsValidationRunning = false;
                return ValidationStatus.Invalidated;
            }

            StressTest.ProgressChanged -= stressTest_ProgressChanged;
            StressTest.ProgressChanged += stressTest_ProgressChanged;
            StressTest.Completed -= stressTest_Completed;
            StressTest.Completed += stressTest_Completed;
            StressTest.Start();

            keepCheckingThrottling = true;
            checkThrottlingWorker = new BackgroundWorker();
            checkThrottlingWorker.DoWork += checkThrottlingWorker_DoWork;
            checkThrottlingWorker.RunWorkerCompleted += checkThrottlingWorker_RunWorkerCompleted;
            checkThrottlingWorker.WorkerSupportsCancellation = true;
            checkThrottlingWorker.RunWorkerAsync();

            resetEvent.WaitOne();
            IsValidationRunning = false;

            return validationStatus;
        }

        public void StopValidation()
        {
            resetEvent.Set();
            if (checkThrottlingWorker != null && checkThrottlingWorker.IsBusy)
                checkThrottlingWorker.CancelAsync();
        }

        private int seconds;
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (++seconds < WAIT_DURATION)
            {
                int perc = seconds * 100 / WAIT_DURATION;
                ProgressChanged?.Invoke(perc, false);
            }
        }

        private bool keepCheckingThrottling;
        private void checkThrottlingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (keepCheckingThrottling)
            {
                if (checkThrottlingWorker.CancellationPending)
                {
                    validationStatus = ValidationStatus.ValidationCancelled;
                    e.Cancel = true;
                    return;
                }

                if (reportThrottling())
                {
                    validationStatus = ValidationStatus.Invalidated;
                    e.Cancel = true;
                    return;
                }

                Thread.Sleep(1000);
            }

            validationStatus = ValidationStatus.Validated;
        }

        private void checkThrottlingWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled) StressTest.Stop();
            resetEvent.Set();
        }

        private void stressTest_ProgressChanged(int percentage)
        {
            ProgressChanged?.Invoke(percentage, true);
        }

        private void stressTest_Completed()
        {
            keepCheckingThrottling = false;
        }

        private bool reportThrottling()
        {
            try
            {
                if (CPUDataConfigService.IsCheckPowerLimitThrottling() && dataService.GetControlValue(idGenerator.GetID(HWComponentType.CPU, SettingType.PowerLimitThrottling, 0)) > 0 ||
                    CPUDataConfigService.IsCheckCurrentLimitThrottling() && dataService.GetControlValue(idGenerator.GetID(HWComponentType.CPU, SettingType.CurrentLimitThrottling, 0)) > 0 ||
                    CPUDataConfigService.IsCheckThermalThrottling() && dataService.GetControlValue(idGenerator.GetID(HWComponentType.CPU, SettingType.ThermalThrottling, 0)) > 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }

            return false;
        }
    }
}