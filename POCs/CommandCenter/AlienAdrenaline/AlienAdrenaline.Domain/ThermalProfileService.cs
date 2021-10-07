

using System;
using System.Collections.ObjectModel;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface ThermalProfileService
    {
        bool IsThermalControlsSupportServiceActive { get; }
        ObservableCollection<ThermalProfile> GetAllThermalProfiles();
        Guid GetActiveThermalProfile();
        void SetThermalProfile(Guid id);
        bool ExistsThermalProfile(Guid id);
        void Dispose();
    }
}
