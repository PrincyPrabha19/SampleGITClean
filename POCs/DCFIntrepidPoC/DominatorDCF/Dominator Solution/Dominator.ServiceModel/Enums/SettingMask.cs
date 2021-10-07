using System;

namespace Dominator.Domain.Enums
{
    [Flags]
    public enum SettingMask : byte
    {
        Read   = 1, 
        Write  = 2,
        Reboot = 4,
        Save   = 8
    }
}