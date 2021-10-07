using System;
using System.Runtime.InteropServices;
using AlienLabs.AlienAdrenaline.AudioAPI.Enums;

namespace AlienLabs.AlienAdrenaline.AudioAPI.Classes
{
    [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    internal class _MMDeviceEnumerator
    {
    }

    /// <summary>
    /// IMMDeviceEnumerator Wrapper Class
    /// </summary>
    public class MMDeviceEnumerator 
    {
        private IMMDeviceEnumerator realDeviceEnumerator = new _MMDeviceEnumerator() as IMMDeviceEnumerator;

        public MMDeviceCollection EnumerateAudioEndPoints(EDataFlow dataFlow, EDeviceState dwStateMask)
        {
            //IMMDeviceCollection result;
            //Marshal.ThrowExceptionForHR(realDeviceEnumerator.EnumAudioEndpoints(dataFlow,dwStateMask,out result));
            //return new MMDeviceCollection(result);

            IMMDeviceCollection deviceCollection;
            var result = realDeviceEnumerator.EnumAudioEndpoints(dataFlow, dwStateMask, out deviceCollection);
            if (result == 0)
                return new MMDeviceCollection(deviceCollection);
            return null;
        }

        public MMDevice GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role)
        {
            //IMMDevice device = null;
            //Marshal.ThrowExceptionForHR(((IMMDeviceEnumerator)realDeviceEnumerator).GetDefaultAudioEndpoint(dataFlow, role, out device));
            //return new MMDevice(device);

            IMMDevice device = null;
            var result = ((IMMDeviceEnumerator)realDeviceEnumerator).GetDefaultAudioEndpoint(dataFlow, role, out device);
            if (result == 0)
                return new MMDevice(device);
            return null;
        }

        public MMDevice GetDevice(string ID)
        {
            //IMMDevice device = null;
            //Marshal.ThrowExceptionForHR(((IMMDeviceEnumerator)realDeviceEnumerator).GetDevice(ID, out device));
            //return new MMDevice(device);

            IMMDevice device = null;
            var result = ((IMMDeviceEnumerator)realDeviceEnumerator).GetDevice(ID, out device);
            if (result == 0)
                return new MMDevice(device);
            return null;
        }

        public MMDeviceEnumerator()
        {
            if (System.Environment.OSVersion.Version.Major < 6)
            {
                throw new NotSupportedException("This functionality is only supported on Windows Vista or newer.");
            }
        }
    }
}
