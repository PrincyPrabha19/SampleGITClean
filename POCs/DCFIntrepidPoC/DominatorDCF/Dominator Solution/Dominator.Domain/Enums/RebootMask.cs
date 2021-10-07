using System;

namespace Dominator.Domain.Enums
{
    [Flags]
    public enum RebootMask : byte
    {
        NoRebootRequired        =0,
        CPUOCRebootRequired     =1,
        MemoryOCRebootRequired    =2
    }
}
