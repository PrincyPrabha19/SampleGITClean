using System.Globalization;
using Dominator.Domain.Enums;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.ServiceModel.Classes.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Dominator.ServiceModel.Classes.Monitoring
{
    public class DataService : IDataService
    {
        private IDataProvider<decimal> xtuDataProvider;
        private IDataProvider<decimal> windowsDataProvider;
        private IXTUService xtuService;
        public void Initialize()
        {
            xtuService = ServiceRepository.XTUServiceInstance;
            //Uncomment this to start the xtu monitoring manually
            //  xtuService.StartMonitor();
            xtuDataProvider = new XTUDataProvider { XTUService = xtuService };
            windowsDataProvider = new WindowsDataProvider();
        }

        public decimal GetProfileSettings(string name, uint id)
        {
            return xtuService?.GetProfileSettings(name, SettingsIDRepository.ControlIDs[id]) ?? -1;
        }

        public Dictionary<uint, decimal> GetAllControlValue(uint[] elementIDs)
        {
            var res = new Dictionary<uint, decimal>();
            var xtuIDs = new Dictionary<uint, uint>();

            for (int i = 0; i < elementIDs.Length; i++)
            {
                if (!SettingsIDRepository.ControlIDs.ContainsKey(elementIDs[i]))
                {
                    res.Add(elementIDs[i], 0);
                    continue;
                }

                var convertedID = SettingsIDRepository.ControlIDs[elementIDs[i]];
                if (!isXTURequired(convertedID))
                {
                    res.Add(elementIDs[i], windowsDataProvider.GetControlValue(convertedID));
                    continue;
                }

                xtuIDs.Add(convertedID, elementIDs[i]);
            }

            if (xtuIDs.Count == 0)
                return res;

            var ary = xtuIDs.Keys.ToArray();
            var xtuValues = xtuDataProvider.GetAllControl(ary);
            for (int i = 0; i < ary.Length; i++)
            {
                res.Add(xtuIDs[ary[i]], xtuValues[i]);
            }

            return res;
        }

        public decimal GetControlValue(uint elementID)
        {
            if (!SettingsIDRepository.ControlIDs.ContainsKey(elementID))
                return 0;

            var convertedID = SettingsIDRepository.ControlIDs[elementID];
            return isXTURequired(convertedID) ? 
                xtuDataProvider.GetControlValue(convertedID) : 
                windowsDataProvider.GetControlValue(convertedID);
        }

        public decimal GetControlValue(uint elementID, SettingMask settingMask)
        {
            if((settingMask & SettingMask.Save) != 0)   //use tuning lib if its a read and write setting else use the monitoring lib
               return xtuService?.GetControlValue(SettingsIDRepository.ControlIDs[elementID]) ?? -1;
            return GetControlValue(elementID);
        }

        public ControlInfo ReadSettingInfo(uint elementID)
        {
            return new ControlInfo
            {
                MaxValue = getMaxControlValue(elementID),
                MinValue = getMinControlValue(elementID),
                NoOfDecimals = getNumOfDecimals(elementID)
            };
        }

        
        public void Release()
        {
            xtuDataProvider = null;
            //Uncomment this to the stop the xtu monitoring manually
           // xtuService.StopMonitor();
            windowsDataProvider = null;
        }

        private decimal getMaxControlValue(uint controlID)
        {
            return xtuService?.GetMaxControlValue(SettingsIDRepository.ControlIDs[controlID]) ?? -1;
        }
        private decimal getMinControlValue(uint controlID)
        {
            return xtuService?.GetMinControlValue(SettingsIDRepository.ControlIDs[controlID]) ?? -1;
        }

        private int getNumOfDecimals(uint controlID)
        {
            var stepValue = xtuService?.SettingsStepValue(SettingsIDRepository.ControlIDs[controlID]) ?? -1;
            var numOfDecimals = stepValue.ToString(CultureInfo.InvariantCulture).Split('.');
            if (numOfDecimals.Length > 1)
                return numOfDecimals[1].Length;
            return 0;
        }
        private bool isXTURequired(uint elementID)
        {
            return elementID < SettingsIDRepository.WINDOWS_BASEID;
        }

    }
}
