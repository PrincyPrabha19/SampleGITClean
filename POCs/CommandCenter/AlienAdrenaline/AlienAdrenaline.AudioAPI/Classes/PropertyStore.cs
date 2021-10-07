using System;
using System.Runtime.InteropServices;
using AlienLabs.AlienAdrenaline.AudioAPI.Structs;

namespace AlienLabs.AlienAdrenaline.AudioAPI.Classes
{
    public class PropertyStore
    {
        private IPropertyStore realPropertyStore;

        public int Count
        {
            get
            {
                int result;
                Marshal.ThrowExceptionForHR(realPropertyStore.GetCount(out result));
                return result;
            }
        }

        public PropertyStoreProperty this[int index]
        {
            get
            {
                PROPVARIANT result;
                PROPERTYKEY key = Get(index);
                Marshal.ThrowExceptionForHR(realPropertyStore.GetValue(ref key, out result));
                return new PropertyStoreProperty(key, result);
            }
        }

        public bool Contains(Guid guid)
        {
            for (int i = 0; i < Count; i++)
            {
                PROPERTYKEY key = Get(i);
                if (key.fmtid == guid)
                    return true;
            }
            return false;
        }

        public PropertyStoreProperty this[Guid guid]
        {
            get
            {
                PROPVARIANT result;
                for (int i = 0; i < Count; i++)
                {
                    PROPERTYKEY key = Get(i);
                    if (key.fmtid == guid)
                    {
                        Marshal.ThrowExceptionForHR(realPropertyStore.GetValue(ref key, out result));
                        return new PropertyStoreProperty(key, result);
                    }
                }
                return null;
            }
        }

        public PROPERTYKEY Get(int index)
        {
            PROPERTYKEY key;
            Marshal.ThrowExceptionForHR(realPropertyStore.GetAt(index, out key));
            return key;
        }

        public PROPVARIANT GetValue(int index)
        {
            PROPVARIANT result;
            PROPERTYKEY key = Get(index);
            Marshal.ThrowExceptionForHR(realPropertyStore.GetValue(ref key, out result));
            return result;
        }

        internal PropertyStore(IPropertyStore store)
        {
            realPropertyStore = store;
        }
    }
}
