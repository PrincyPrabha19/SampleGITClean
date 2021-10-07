using System;
using System.Collections.Generic;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes.Helpers
{
    public class MemoryTuning : ITuning
    {
        public IXTUService XTUService { get; set; }
        public bool TuneControls(List<ControlValue> memorySettings, out bool isRestartRequired)
        {
            isRestartRequired = false;
            foreach (var setting in memorySettings)
                setting.Id = SettingsIDRepository.ControlIDs[setting.Id];
            return XTUService?.TuneListOfControls(memorySettings, out isRestartRequired) ?? false;
        }

        public bool TuneControl(uint controlID, decimal controlValue, out bool rebootNeeded)
        {
            rebootNeeded = false;
            bool reboot;
            var result = XTUService?.TuneControl(SettingsIDRepository.ControlIDs[controlID], controlValue, out reboot) ?? false;
            var control = GetHWControl(controlID);
            rebootNeeded = control.RequiresReboot && control.BootValue != controlValue;
            return result;
        }

        public bool ProposeControls(out List<ControlValue> proposalResult, out bool isRestartRequired)
        {
            isRestartRequired = false;
            proposalResult = null;
            //var result = XTUService?.ProposeChanges(memoryProposals, out proposalResult, out isRestartRequired) ?? false;
            //if (proposalResult == null) return result;
            //foreach (var setting in proposalResult)
            //    setting.Id = SettingsIDRepository.ControlIDs.FirstOrDefault(x => x.Value == setting.Id).Key;
            return true;
        }

        public bool ApplyControlChanges()
        {
            return XTUService?.ApplyChanges(false) ?? false;
        }

        public bool ApplyDefaultProfile(out bool rebootRequired)
        {
            rebootRequired = false;
            return XTUService?.TuneControl((uint) XTUID.ExtremeMemoryProfile, 0, out rebootRequired) ?? false;
        }

        public bool ApplyDefaultValues(IMemoryDataComponent memoryComponent, out bool rebootRequired)
        {
            var idGenerator = new SettingIDGenerator();
            rebootRequired=false;
            if(SystemInfoRepository.Instance.XMPInfoData.IsXMPSupported)
            TuneControl(idGenerator.GetID(HWComponentType.Memory, SettingType.XMP, 0), 0, out rebootRequired);
            else
            {
                var controlID = idGenerator.GetID(HWComponentType.Memory, SettingType.ActualFrequency, 0);
                TuneControl(controlID, XTUService.GetDefaultValue(controlID), out rebootRequired);
            }
            return true;
        }

        public void RestartSystem()
        {
            XTUService?.RestartSystem();
        }

        public decimal GetDefaultValue(uint controlID)
        {
            throw new NotImplementedException();
        }

        public IControlData GetHWControl(uint controlID)
        {
            IControlData control = new ControlData();
            var controlidy = SettingsIDRepository.ControlIDs[controlID];
            control.BootValue = XTUService.ControlBootValue(controlidy);
            control.RequiresReboot = XTUService.ControlRequiresReboot(controlidy);
            return control;
        }
    }
}
