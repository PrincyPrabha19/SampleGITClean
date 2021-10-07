using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace LoadingExternalPackage.Domain
{
    public interface IDeviceControlSettings
    {
        SolidColorBrush SelectedZoneBrush { get; set; }
        SolidColorBrush ZoneDefaultBrush { get; set; }
        SolidColorBrush ControlBackground { get; set; }
        double ControlOpacity { get; set; }
        SolidColorBrush ControlBorder { get; set; }
        Thickness ControlBorderThickness { get; set; }
        List<string> SelectedZones { get; set; }
        List<GroupMapping> GroupMappings { get; }

        void SelectZone(string hotspotName);
        void SelectAllZones();
        void ResetZone(string hotspotName);
        void ResetAllZones();
    }
}