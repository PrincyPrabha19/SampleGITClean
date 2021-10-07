using System.Collections.Generic;
using Dominator.Domain.Enums;

namespace Dominator.Domain
{
    public interface IProfileProcessor
    {
        ITuningManager TuningManager { set; }
        bool Apply(List<IDataComponent> dataComponents, bool forceRestart, bool isNewProfile, out RebootMask rebootMask);
        bool ApplyMemoryProfile(List<IDataComponent> dataComponents, out RebootMask rebootRequired);
        bool ApplyCPUProfile(List<IDataComponent> dataComponents, out RebootMask rebootRequired);
    }
}