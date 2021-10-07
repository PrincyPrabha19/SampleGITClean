using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Classes.SystemInfo;
using Intel.Overclocking.SDK.Tuning;

namespace Dominator.ServiceModel
{
    [ServiceContract]
    public interface IXTUService
    {
        [OperationContract]
        bool Initialize();

        [OperationContract]
        string Ping();

        [OperationContract]
        bool ApplyXTUProfile(string name, out bool rebootRequired, bool forceRestart);

        [OperationContract]
        bool LoadXTUProfile(string path);

        [OperationContract]
        bool LoadXTUProfileValues(string name, out List<ProfileSetting> profileSettings);

        [OperationContract]
        decimal GetProfileSettings(string name, uint controlID);

        [OperationContract]
        string GetProcessorBrand();

        [OperationContract]
        bool ApplyDefaultProfile(out bool rebootRequired);

        [OperationContract]
        bool IsSystemOverclocked();

        [OperationContract]
        bool IsOverclockingSupported();

        [OperationContract]
        decimal GetValueOfControl(uint controlID);

        [OperationContract]
        decimal[] GetAllControlValue(uint[] controlIDs);

        [OperationContract]
        bool TuneControl(uint controlID, decimal controlValue, out bool isRestartRequired);

        [OperationContract]
        bool TuneListOfControls(List<ControlValue> proposals, out bool isRestartRequired);

        [OperationContract]
        decimal GetControlValue(uint controlID);

        [OperationContract]
        bool ProposeChanges(List<ControlValue> proposals, out List<ControlValue> proposalResult, out bool requiresReboot);

        [OperationContract]
        bool ApplyChanges(bool forceRestart);

        [OperationContract]
        List<ClientTuningProposalResult> DiscardChanges();

        [OperationContract]
        decimal GetMaxControlValue(uint controlID);

        [OperationContract]
        decimal GetMinControlValue(uint controlID);

        [OperationContract]
        CPUInfoData GetCPUInfoData();

        [OperationContract]
        WatchdogTimerInfo GetWatchdogTimerInfo();

        [OperationContract]
        decimal SettingsStepValue(uint controlID);

        [OperationContract]
        XMPInfoData GetXMPInfoData();

        [OperationContract]
        bool IsMemoryOCSupported();

        [OperationContract]
        decimal GetDefaultValue(uint controlID);

        [OperationContract]
        void RestartSystem();

        [OperationContract]
        bool GetHWControl(uint id, out IControlData controlData);

        [OperationContract]
        bool ControlRequiresReboot(uint id);

        [OperationContract]
        decimal ControlBootValue(uint id);

        [OperationContract]
        bool SetCurrentAuto();

        [OperationContract]
        void StartMonitor();

        [OperationContract]
        void StopMonitor();
    }

    [DataContract]
    public class ControlData : IControlData
    {
        [DataMember]
        public decimal ActiveValue { get; set; }
        [DataMember]
        public decimal BootValue { get; set; }
        [DataMember]
        public bool RequiresReboot { get; set; }
    }

    public interface IControlData
    {
        decimal ActiveValue { get; set; }
        decimal BootValue { get; set; }  
        bool RequiresReboot { get; set; }
    }

    [DataContract]
    public class ProfileSetting : IProfileSetting
    {
        [DataMember]
        public uint Id { get; set; }

        [DataMember]
        public decimal Value { get; set; }

        [DataMember]
        public string Unit { get; set; }
    }

    public interface IProfileSetting
    {
        uint Id { get; set; }

        decimal Value { get; set; }

        string Unit { get; set; }
    }
}