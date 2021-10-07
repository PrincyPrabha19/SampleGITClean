using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI;
using LoadingExternalPackage.Domain;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Core;
using LoadingExternalPackage.ViewModel;
using LoadingExternalPackage.Controls;
using System.Collections.Generic;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;

namespace LoadingExternalPackage.UnitTest
{
    [TestClass]
    public class CanvasViewModelTest
    {
        public SolidColorBrush ZoneDefaultBrush { get; set; }
        public SolidColorBrush SelectedZoneBrush { get; set; }
        public SolidColorBrush ControlBackgroundBrush { get; set; }
        public double BackgroundOpacity { get; set; }
        public SolidColorBrush ControlBorderBrush { get; set; }
        public Thickness BorderThickness { get; set; }
        public string TestHotspot { get; set; }
        public string TestMask { get; set; }

        [TestInitialize]
        public async Task InitializeTest()
        {
            await RunInUIThread(new DispatchedHandler(() =>
            {
                ControlBackgroundBrush = new SolidColorBrush(Colors.LightBlue);
                BackgroundOpacity = 0.5;
                ControlBorderBrush = new SolidColorBrush(Colors.Gold);
                BorderThickness = new Thickness(2);

                ZoneDefaultBrush = new SolidColorBrush(Colors.SteelBlue);
                SelectedZoneBrush = new SolidColorBrush(Colors.LightGray);
            }));

            TestHotspot = "hotspot03";
            TestMask = "mask03";
        }
        
        [TestMethod]
        public async Task InitControl_ShouldLoadControlData()
        {
            await RunInUIThread(new DispatchedHandler(() =>
            {
                // Arrange
                var canvasViewModel = new CanvasViewModel(HotspotClickHandler, MaskClickHandler);
                SetControlSettings(canvasViewModel);

                // Assert
                Assert.IsNotNull(canvasViewModel.DeviceControl);
                Assert.IsNotNull(canvasViewModel.DeviceControlSettings);
                Assert.IsNotNull(canvasViewModel.DeviceControlSettings.GroupMappings);
                Assert.IsInstanceOfType(canvasViewModel.DeviceControl, typeof(Alienware15R3));
                Assert.IsInstanceOfType(canvasViewModel.DeviceControlSettings, typeof(IDeviceControlSettings));
                Assert.AreEqual(ZoneDefaultBrush, canvasViewModel.DeviceControlSettings.ZoneDefaultBrush);
                Assert.AreEqual(SelectedZoneBrush, canvasViewModel.DeviceControlSettings.SelectedZoneBrush);
            }));
        }        

        [TestMethod]
        public async Task SelectHotspot_ShouldSelectHotspotWithCorrectName()
        {
            await RunInUIThread(new DispatchedHandler(() =>
            {
                // Arrange
                var canvasViewModel = new CanvasViewModel(HotspotClickHandler, MaskClickHandler);
                SetControlSettings(canvasViewModel);

                // Act
                canvasViewModel.DeviceControlSettings.SelectZone(TestHotspot);
                var groupMap = canvasViewModel.DeviceControlSettings.GroupMappings.FirstOrDefault(gmap => string.Equals(gmap.Hotspot.Name, TestHotspot, StringComparison.CurrentCultureIgnoreCase));

                // Assert
                if (!string.IsNullOrEmpty(groupMap?.Hotspot?.Name))
                    Assert.AreEqual(Constants.SELECTED_TAG, groupMap.Hotspot.Element.Tag);

                groupMap.Masks.ForEach(mask => Assert.AreEqual(Constants.SELECTED_TAG, mask.Element.Tag));
            }));
        }

        [TestMethod]
        public async Task SelectMask_ShouldSelectMaskWithCorrectName()
        {
            await RunInUIThread(new DispatchedHandler(() =>
            {
                // Arrange
                var canvasViewModel = new CanvasViewModel(HotspotClickHandler, MaskClickHandler);
                SetControlSettings(canvasViewModel);

                // Act
                canvasViewModel.DeviceControlSettings.SelectZone(TestMask);
                var masks = from gmap in canvasViewModel.DeviceControlSettings.GroupMappings
                            from mask in gmap.Masks
                            where string.Equals(mask.Name, TestMask)
                            select mask.Element;

                // Assert
                foreach (var mask in masks)
                {
                    Assert.AreEqual(Constants.SELECTED_TAG, mask.Tag);
                }
            }));
        }

        [TestMethod]
        public async Task HighlightAllSelectedZones_ShouldSelectAllHotspotAndMasksInSelectedZones()
        {
            await RunInUIThread(new DispatchedHandler(() =>
            {
                // Arrange
                var canvasViewModel = new CanvasViewModel(HotspotClickHandler, MaskClickHandler);
                SetControlSettings(canvasViewModel);
                var selectedZones = new List<string> { "hotspot01", "hotspot02", "hotspot03" };

                // Act
                canvasViewModel.DeviceControlSettings.SelectedZones = selectedZones;
                var hotspotMaps = canvasViewModel.DeviceControlSettings.GroupMappings.Where(gmap => selectedZones.Contains(gmap.Hotspot.Name));

                // Assert
                foreach (var hmap in hotspotMaps)
                {
                    Assert.AreEqual(Constants.SELECTED_TAG, hmap.Hotspot.Element.Tag);
                    hmap.Masks.ForEach(mask => Assert.AreEqual(Constants.SELECTED_TAG, mask.Element.Tag));
                }
            }));
        }

        [TestMethod]
        public async Task ResetAllZones_ShouldResetAllHotspotsAndMasksToDefaultColor()
        {
            await RunInUIThread(new DispatchedHandler(() =>
            {
                // Arrange
                var canvasViewModel = new CanvasViewModel(HotspotClickHandler, MaskClickHandler);
                SetControlSettings(canvasViewModel);

                // Act
                canvasViewModel.DeviceControlSettings.ResetAllZones();

                // Assert
                foreach (var gmap in canvasViewModel.DeviceControlSettings.GroupMappings)
                {
                    if (!string.IsNullOrEmpty(gmap.Hotspot.Name))
                    {
                        Assert.AreEqual(gmap.Hotspot.Element.Tag, string.Empty);
                        Assert.AreEqual(gmap.Hotspot.Element.Foreground, ZoneDefaultBrush);
                    }

                    foreach (var mask in gmap.Masks)
                    {
                        Assert.AreEqual(mask.Element.Tag, string.Empty);
                        Assert.AreEqual(((Path)mask.Element).Stroke, ZoneDefaultBrush);
                    }
                }
            }));
        }

        private void SetControlSettings(CanvasViewModel canvasViewModel)
        {
            canvasViewModel.DeviceControlSettings.SelectedZoneBrush = SelectedZoneBrush;
            canvasViewModel.DeviceControlSettings.ZoneDefaultBrush = ZoneDefaultBrush;
            canvasViewModel.DeviceControlSettings.ControlBackground = ControlBackgroundBrush;
            canvasViewModel.DeviceControlSettings.ControlOpacity = BackgroundOpacity;
            canvasViewModel.DeviceControlSettings.ControlBorder = ControlBorderBrush;
            canvasViewModel.DeviceControlSettings.ControlBorderThickness = BorderThickness;
        }

        private void HotspotClickHandler(object sender, PointerRoutedEventArgs e)
        {
            
        }

        private void MaskClickHandler(object sender, PointerRoutedEventArgs e)
        {
            
        }

        private async Task RunInUIThread(DispatchedHandler callback)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, callback);
        }
    }
}
