using Dominator.Domain.Enums;
using Dominator.ServiceModel.Classes.Helpers;
using System.Collections.Generic;

namespace Dominator.ServiceModel
{
    public interface IDataService
    {
        void Initialize();
        decimal GetProfileSettings(string name, uint id);
        Dictionary<uint, decimal> GetAllControlValue(uint[] elementIDs);
        decimal GetControlValue(uint elementID);
        decimal GetControlValue(uint settingID, SettingMask settingMask);
        ControlInfo ReadSettingInfo(uint elementID);
        void Release();
    }
}
