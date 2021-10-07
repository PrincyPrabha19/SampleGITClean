using System.Collections.Generic;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes.Helpers
{
    public class CPUTuning : ITuning
    {
        public IXTUService XTUService { private get; set; }
        //TODO: do something about the restart required information
        public bool TuneControls(List<ControlValue> cpuSettings, out bool isRestartRequired)
        {
            isRestartRequired = false;
            foreach (var setting in cpuSettings)
            {
                setting.Id = SettingsIDRepository.ControlIDs[setting.Id];
            }
            return XTUService?.TuneListOfControls(cpuSettings, out isRestartRequired) ?? false;
        }
        
        public bool TuneControl(uint controlID, decimal controlValue, out bool rebootNeeded)
        {
            rebootNeeded = false;
            bool reboot;
            var result = XTUService?.TuneControl(SettingsIDRepository.ControlIDs[controlID], controlValue, out reboot) ?? false;
            var control = GetHWControl(controlID);
            if (control.RequiresReboot && control.BootValue != controlValue)
                rebootNeeded = true;
            return result;
        }
        public bool ProposeControls(out List<ControlValue> proposalResult, out bool isRestartRequired)
        {
            isRestartRequired = false;
            proposalResult = null;
            //var result = XTUService?.ProposeChanges(cpuProposals, out proposalResult, out isRestartRequired) ?? false;
            //if (proposalResult == null) return result;
            //foreach (var setting in proposalResult)
            //{
            //  setting.Id = SettingsIDRepository.ControlIDs.FirstOrDefault(x => x.Value == setting.Id).Key;
            //}
            return true;
        }
        public bool ApplyControlChanges()
        {
            return XTUService?.ApplyChanges(false) ?? false;
        }

        public bool ApplyDefaultValues(IMemoryDataComponent memoryComponent, out bool rebootRequired)
        {
            var idGenerator = new SettingIDGenerator();
            rebootRequired = false;
            return ApplyDefaultProfile(out rebootRequired);
        }

        public void RestartSystem()
        {
            XTUService.RestartSystem();
        }

        public decimal GetDefaultValue(uint controlID)
        {
            return XTUService.GetDefaultValue(SettingsIDRepository.ControlIDs[controlID]);
        }

        public IControlData GetHWControl(uint controlID)
        {
            IControlData control = new ControlData();
            var controlidy = SettingsIDRepository.ControlIDs[controlID];
            control.BootValue = XTUService.ControlBootValue(controlidy);
            control.RequiresReboot = XTUService.ControlRequiresReboot(controlidy);
            return control;
        }

        public bool ApplyDefaultProfile(out bool rebootRequired)
        {
            rebootRequired = false;
            return XTUService?.ApplyDefaultProfile(out rebootRequired) ?? false;
        }
    }
}
