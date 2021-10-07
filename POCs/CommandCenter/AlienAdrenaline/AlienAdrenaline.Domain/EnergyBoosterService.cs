using System;
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface EnergyBoosterService
    {
        event Action<EnergyBoosterError> InitTUDEnded;
        event Action<EnergyBoosterError> UpdateTUDEnded;
        event Action<EnergyBoosterError> DoStopEnded;
        event Action<EnergyBoosterError> DoRestartEnded;
        event Action<int, int> DoStopProgressed;
        event Action<int, int> DoRestartProgressed;

        void InitTUD();
        void UpdateTUD();
        void DoStop();
        void DoRestart();
    }
}
