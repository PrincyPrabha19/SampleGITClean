using System;
using Dominator.Domain.Classes.Factories;
using Dominator.ServiceModel;

namespace Dominator.Domain.Classes
{
    public class MonitorManager : IMonitorManager
    {
        #region Properties
        private readonly Guid monitorID;
        private IMonitor<decimal> monitorInstance;
        #endregion

        #region Constructor
        public MonitorManager()
        {
            monitorID = Guid.NewGuid();
        }
        #endregion

        #region Methods
        public void Start()
        {
            initOrRefreshMonitorStatus();
            monitorInstance?.Start(monitorID);
        }

        public void Stop()
        {
            initOrRefreshMonitorStatus();
            monitorInstance?.Stop(monitorID);
        }

        public void AddElement(uint elementID)
        {
            initOrRefreshMonitorStatus();
            monitorInstance?.AddElement(elementID);
        }

        public void AddElements(uint[] elementIDs)
        {
            initOrRefreshMonitorStatus();
            monitorInstance?.AddElements(elementIDs);
        }

        public decimal GetElementValue(uint elementID)
        {
            initOrRefreshMonitorStatus();
            return monitorInstance?.GetElementValue(elementID) ?? 0;
        }

        public decimal[] GetAllElementValues(uint[] elementIDs)
        {
            initOrRefreshMonitorStatus();
            return monitorInstance?.GetAllElementValues(elementIDs) ?? null;
        }

        public void RemoveElement(uint elementID)
        {
            initOrRefreshMonitorStatus();
            monitorInstance?.RemoveElement(elementID);
        }

        private void initOrRefreshMonitorStatus()
        {
            if (monitorInstance == null)
                monitorInstance = MonitorFactory.NewCPUMonitor();
            else
            {
                var initializedNeeded = false;

                try
                {
                    monitorInstance.Ping();
                }
                catch
                {
                    initializedNeeded = true;
                }

                if (initializedNeeded)
                    monitorInstance = MonitorFactory.NewCPUMonitor();
            }
        }
        #endregion
    }
}