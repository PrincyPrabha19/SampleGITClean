using System.Collections.Generic;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Helpers;

namespace Dominator.Domain
{
    public interface ITuning
    {
        IXTUService XTUService { set; }
        bool TuneControls(List<ControlValue> settings, out bool isRestartRequired);
        bool TuneControl(uint controlID, decimal controlValue, out bool rebootNeeded);
        bool ProposeControls(out List<ControlValue> proposalResult, out bool isRestartRequired);
        bool ApplyControlChanges();
        bool ApplyDefaultProfile(out bool rebootRequired);
        bool ApplyDefaultValues(IMemoryDataComponent memoryComponent, out bool rebootRequired);
        void RestartSystem();
        decimal GetDefaultValue(uint controlID);
        IControlData GetHWControl(uint controlID);
    }
}
