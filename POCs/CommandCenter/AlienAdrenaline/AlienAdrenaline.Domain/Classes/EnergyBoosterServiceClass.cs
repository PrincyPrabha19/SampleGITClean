
using System;
using System.Runtime.InteropServices;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using ioloEnergyBooster;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class EnergyBoosterServiceClass : EnergyBoosterService, IDisposable
    {
        public EnergyBooster energyBooster;

        public event Action<EnergyBoosterError> InitTUDEnded;
        public event Action<EnergyBoosterError> UpdateTUDEnded;
        public event Action<EnergyBoosterError> DoStopEnded;
        public event Action<EnergyBoosterError> DoRestartEnded;
        public event Action<int, int> DoStopProgressed;
        public event Action<int, int> DoRestartProgressed;

        public EnergyBoosterServiceClass()
        {
            energyBooster = new EnergyBooster();
            //energyBooster.ActionEnded += energyBooster_ActionEnded;
            //energyBooster.ActionProgressed += energyBooster_ActionProgressed;
        }

        ~EnergyBoosterServiceClass()
        {
            Dispose();
        }

        public void Dispose()
        {
            Marshal.ReleaseComObject(energyBooster);
        }

        //void energyBooster_ActionProgressed(ioloEnergyBooster.Action action, int completed, int total)
        //{
        //    switch (action)
        //    {
        //        case System.Action.Stop:
        //            if (StopProgressed != null) StopProgressed(completed, total);
        //            break;
        //        case System.Action.Start:
        //            if (StartProgressed != null) StartProgressed(completed, total);
        //            break;
        //    }
        //}

        //void energyBooster_ActionEnded(ioloEnergyBooster.Action action, Error error)
        //{
        //    EnergyBoosterError _error;
        //    Enum.TryParse(error.ToString(), out _error);

        //    switch (action)
        //    {
        //        case System.Action.Parse:
        //            if (InitEnded != null) InitEnded(_error);
        //            break;
        //        case System.Action.Update:
        //            if (UpdateEnded != null) UpdateEnded(_error);
        //            break;
        //        case System.Action.Stop:
        //            if (StopEnded != null) StopEnded(_error);
        //            break;
        //        case System.Action.Start:
        //            if (StartEnded != null) StartEnded(_error);
        //            break;
        //    }
        //}

        public void InitTUD()
        {
            var error = energyBooster.InitTUD(null);

            EnergyBoosterError _error;
            Enum.TryParse(error.ToString(), out _error);
            if (InitTUDEnded != null) 
                InitTUDEnded(_error);

            //BackgroundWorker worker = new BackgroundWorker();
            //worker.DoWork += energyBooster.DoInitTUD;
            //worker.RunWorkerAsync();            
        }

        public void UpdateTUD()
        {
            var error = energyBooster.UpdateTUD(null);

            EnergyBoosterError _error;
            Enum.TryParse(error.ToString(), out _error);
            if (UpdateTUDEnded != null)
                UpdateTUDEnded(_error);

            //BackgroundWorker worker = new BackgroundWorker();
            //worker.DoWork += energyBooster.DoUpdateTUD;
            //worker.RunWorkerAsync();
        }

        public void DoStop()
        {
            var error = energyBooster.DoStop(LogLevel.None, true, null);

            EnergyBoosterError _error;
            Enum.TryParse(error.ToString(), out _error);
            if (DoStopEnded != null)
                DoStopEnded(_error);

            //BackgroundWorker worker = new BackgroundWorker();
            //worker.DoWork += energyBooster.DoStop;
            //worker.RunWorkerAsync();
        }

        public void DoRestart()
        {
            var error = energyBooster.DoRestart(LogLevel.None, true, null);

            EnergyBoosterError _error;
            Enum.TryParse(error.ToString(), out _error);
            if (DoRestartEnded != null)
                DoRestartEnded(_error);

            //BackgroundWorker worker = new BackgroundWorker();
            //worker.DoWork += energyBooster.DoRestart;
            //worker.RunWorkerAsync();
        }
    }
}