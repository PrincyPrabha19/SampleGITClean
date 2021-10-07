using System;
using System.Runtime.InteropServices;
using AlienLabs.AlienAdrenaline.AudioAPI.Structs;

namespace AlienLabs.AlienAdrenaline.AudioAPI
{
    [Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyStore
    {
        [PreserveSig]
        int GetCount(out Int32 count);

        [PreserveSig]
        int GetAt(int propertyIndex, out PROPERTYKEY propertyKey);

        [PreserveSig]
        int GetValue(ref PROPERTYKEY propertyKey, out PROPVARIANT pv);

        [PreserveSig]
        int SetValue(ref PROPERTYKEY propertyKey, ref PROPVARIANT propVariant);

        [PreserveSig]
        int Commit();
    };
}