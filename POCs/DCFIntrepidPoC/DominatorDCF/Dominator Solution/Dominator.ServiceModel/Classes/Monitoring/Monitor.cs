using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;

namespace Dominator.ServiceModel.Classes.Monitoring
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed),
 ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public abstract class Monitor : IMonitor<decimal>
    {
        #region Properties
        private Timer timer;
        private int pollingInterval = 1;
        private bool busy;

        private readonly Dictionary<uint, decimal> elementValues;
        private readonly Dictionary<uint, int> elementRefCounter;
        private readonly List<Guid> clientList;

        protected IDataService dataService;
        private readonly ILogger logger;
        #endregion

        #region Constructor
        protected Monitor()
        {
            elementValues = new Dictionary<uint, decimal>();
            elementRefCounter = new Dictionary<uint, int>();
            clientList = new List<Guid>();
            logger = LoggerFactory.LoggerInstance;
        }
        #endregion

        #region Methods
        public string Ping()
        {
            return DateTime.Now.ToString("u");
        }

        public virtual void Start(Guid clientID)
        {
            if (clientList.Contains(clientID))
                return;

            clientList.Add(clientID);
            if (clientList.Count > 1)
                return;

            InitDataProviders();
            wakeupIfNeeded();

            logger?.WriteLine("Monitor service: STARTED");
        }

        public virtual void Stop(Guid clientID)
        {
            if (!clientList.Contains(clientID))
                return;

            clientList.Remove(clientID);
            if (clientList.Count != 0)
                return;

            CleanDataProviders();
            disposeTimer();

            elementValues.Clear();
            elementRefCounter.Clear();

            logger?.WriteLine("Monitor service: STOPPED");
        }

        public void AddElement(uint elementID)
        {
            if (!elementValues.ContainsKey(elementID))
            {
                elementRefCounter.Add(elementID, 0);
                startMonitoringElement(elementID);
            }

            elementRefCounter[elementID]++;
        }

        public void AddElements(uint[] elementIDs)
        {
            foreach (var element in elementIDs)
                AddElement(element);
        }

        public decimal GetElementValue(uint elementID)
        {
            return elementValues.ContainsKey(elementID) ? elementValues[elementID] : 0;
        }

        public decimal[] GetAllElementValues(uint[] elementIDs)
        {
            if (elementIDs == null || elementIDs.Length < 0)
                return null;
            var res = new decimal[elementIDs.Length];
            for (int i = 0; res != null && i < elementIDs.Length; i++)
                res[i] = elementValues.ContainsKey(elementIDs[i]) ? elementValues[elementIDs[i]] : 0;
            return res;
        }

        public void RemoveElement(uint elementID)
        {
            if (!elementValues.ContainsKey(elementID)) return;

            System.Threading.Monitor.Enter(elementValues);
            try
            {
                if (--elementRefCounter[elementID] == 0)
                    elementValues.Remove(elementID);
            }
            catch
            {
            }

            System.Threading.Monitor.Exit(elementValues);
            sleepIfNeeded();
        }

        protected virtual void InitDataProviders()
        {
        }

        protected virtual void CleanDataProviders()
        {
        } 
        private void createTimer()
        {
            timer = new Timer(timerCallBack, null, 0, pollingInterval * 1000);
        }

        private void disposeTimer()
        {
            if (timer == null) return;

            timer.Dispose();
            timer = null;
        }

        private void wakeupIfNeeded()
        {
            if (timer == null && elementValues.Count > 0)
                createTimer();
        }

        private void sleepIfNeeded()
        {
            if (elementValues.Count == 0)
                disposeTimer();
        }

        private void startMonitoringElement(uint elementID)
        {
            System.Threading.Monitor.Enter(elementValues);
            try
            {
                elementValues.Add(elementID, 0);
            }
            catch
            { }
            System.Threading.Monitor.Exit(elementValues);

            wakeupIfNeeded();
        }

        private void polling()
        {
            if (busy) return;

            busy = true;
            updateValueList();
            busy = false;
        }

        private void updateValueList()
        {
            if (elementValues == null || elementValues.Count == 0)
                return;

            if (!System.Threading.Monitor.TryEnter(elementValues, 500))
                return;

            try
            {
                var keys = elementValues.Keys.ToArray();
                var res = dataService.GetAllControlValue(keys);
                foreach (KeyValuePair<uint, decimal> item in res)
                {
                    elementValues[item.Key] = item.Value;
                }
            }
            catch { }

            System.Threading.Monitor.Exit(elementValues);
        }

        private decimal readElement(uint elementID)
        {
            return dataService.GetControlValue(elementID);
        }

        #endregion

        #region Event Handlers
        private void timerCallBack(object stateObject)
        {
            polling();
        }
        #endregion
    }
}
