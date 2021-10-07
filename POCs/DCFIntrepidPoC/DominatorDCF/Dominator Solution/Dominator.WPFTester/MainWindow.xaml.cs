using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Dominator.Domain;
using Dominator.Domain.Classes;
using Dominator.Domain.Classes.Factories;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;
using DominatorTester.Controls;

namespace DominatorTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBasicStressTest _basicBasicStressTest;
        private readonly IBIOSSupportProvider biosSupportProvider = BIOSSupportProviderFactory.NewBIOSSupportProvider();

        public MainWindow()
        {
            InitializeComponent();
            initSlider();

            buttonRefreshFlags_Click(null, null);
        }

        private void initSlider()
        {
            sl.TransitionPercent = 5;
            sl.Ranges = new List<ColourSliderRange>
            {
                new ColourSliderRange {Min = 0M   , Max = 0.5M , Color = Colors.Red},
                new ColourSliderRange {Min = 0.5M , Max = 0.75M, Color = Colors.Yellow},
                new ColourSliderRange {Min = 0.75M, Max = 1.2M , Color = Colors.Green},
                new ColourSliderRange {Min = 1.2M , Max = 1.5M , Color = Colors.Yellow},
                new ColourSliderRange {Min = 1.5M , Max = 2M   , Color = Colors.Red},
            };

            sl.SelectedValue = 1;
        }

        private void buttonStartStressTest_Click(object sender, RoutedEventArgs e)
        {
            _basicBasicStressTest = new BasicStressTest(5);
            _basicBasicStressTest.ProgressChanged += basicStressTestProgressChanged;

            updateStressTestProgress(0);
            new Thread(_basicBasicStressTest.Start).Start();
        }

        private void basicStressTestProgressChanged(int percentage)
        {
            if (!Dispatcher.CheckAccess())
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<int>(updateStressTestProgress), percentage);
        }

        private void updateStressTestProgress(int percentage)
        {
            labelStressTest.Content = $"{percentage} %";
            validationControl2.ValidationPercentage = percentage;
        }

        private void buttonStopStressTest_Click(object sender, RoutedEventArgs e)
        {
            _basicBasicStressTest?.Stop();
        }

        private void buttonGetSystemInfo_Click(object sender, RoutedEventArgs e)
        {
            var result = SystemInfoRepository.GetInstanceAsync().Result.ToString();
            textBlockSystemInfo.Text = result;
        }

        private void radioButtonFailsafeFlagON_Click(object sender, RoutedEventArgs e)
        {
            BIOSSupportRegistryHelper.WriteOCFailsafeFlagStatus(1);
        }

        private void radioButtonFailsafeFlagOFF_Click(object sender, RoutedEventArgs e)
        {
            BIOSSupportRegistryHelper.WriteOCFailsafeFlagStatus(0);
        }

        private void OCUIBIOSControlFlagON_Click(object sender, RoutedEventArgs e)
        {
            BIOSSupportRegistryHelper.WriteOCUIBIOSControlStatus(1);
        }

        private void OCUIBIOSControlFlagOFF_Click(object sender, RoutedEventArgs e)
        {
            BIOSSupportRegistryHelper.WriteOCUIBIOSControlStatus(0);
        }
        private void radioButtonInitSuccessfull_Click(object sender, RoutedEventArgs e)
        {
            BIOSSupportRegistryHelper.WriteBIOSInitStatus((int) BIOSInitializationStates.InitializationSuccessful);
        }

        private void radioButtonInitFailed_Click(object sender, RoutedEventArgs e)
        {
            BIOSSupportRegistryHelper.WriteBIOSInitStatus((int)BIOSInitializationStates.InitializationFailed);
        }

        private void radioButtonInitNotSupportedBIOSInterface_Click(object sender, RoutedEventArgs e)
        {
            BIOSSupportRegistryHelper.WriteBIOSInitStatus((int)BIOSInitializationStates.NotSupportedBIOSInterface);
        }

        private void buttonRefreshFlags_Click(object sender, RoutedEventArgs e)
        {
            biosSupportProvider.Initialize();
            radioButtonInitSuccessfull.IsChecked = biosSupportProvider.IsBIOSSupportAPIWrapperInitialized;
            radioButtonInitFailed.IsChecked = !biosSupportProvider.IsBIOSSupportAPIWrapperInitialized;
            radioButtonInitNotSupportedBIOSInterface.IsChecked = biosSupportProvider.IsBIOSInterfaceNotSupported;

            int overclockingReport;
            if (biosSupportProvider.RefreshOverclockingReport(out overclockingReport, false))
            {
                radioButtonFailsafeFlagON.IsChecked = biosSupportProvider.IsOCFailsafeFlagStatusEnabled;
                radioButtonFailsafeFlagOFF.IsChecked = !biosSupportProvider.IsOCFailsafeFlagStatusEnabled;

                radioButtonOCUIBIOSControlFlagON.IsChecked = biosSupportProvider.IsOCUIBIOSControlStatusEnabled;
                radioButtonOCUIBIOSControlFlagOFF.IsChecked = !biosSupportProvider.IsOCUIBIOSControlStatusEnabled;
            }
        }

        private void buttonPushNotification_Click(object sender, RoutedEventArgs e)
        {
            //var notification = new WindowsNotification()
            //{
            //    Caption = "Dominator Notification",
            //    Description = "Overclocking settings have been disabled. Review the active profile settings.",
            //    Action1Text = "Review Now",
            //    Action2Text = "Dismiss",
            //    ApplicationName = "Dominator",
            //    ApplicationPath = @"D:\DEVGIT2\Dominator\Dominator Solution\Dominator\bin\Debug\Dominator.exe"
            //};

            //notification.Show();
        }
    }
}
