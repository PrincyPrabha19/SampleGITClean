using System;
using System.Runtime.InteropServices;

namespace AlienLabs.AlienAdrenaline.AudioAPI.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct PROPERTYKEY
    {
        public Guid fmtid;
        public uint pid;
    }        
}
