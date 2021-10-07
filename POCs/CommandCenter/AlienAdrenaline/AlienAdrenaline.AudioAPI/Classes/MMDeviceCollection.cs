using System.Runtime.InteropServices;

namespace AlienLabs.AlienAdrenaline.AudioAPI.Classes
{
    /// <summary>
    /// IMMDeviceCollection Wrapper Class
    /// </summary>
    public class MMDeviceCollection
    {
        private IMMDeviceCollection realDeviceCollection;

        public int Count
        {
            get
            {
                uint result;
                Marshal.ThrowExceptionForHR(realDeviceCollection.GetCount(out result));
                return (int)result;
            }
        }

        public MMDevice this[int index]
        {
            get
            {
                IMMDevice result;
                realDeviceCollection.Item((uint)index, out result);
                return new MMDevice(result);
            }
        }

        internal MMDeviceCollection(IMMDeviceCollection parent)
        {
            realDeviceCollection = parent;
        }
    }
}
