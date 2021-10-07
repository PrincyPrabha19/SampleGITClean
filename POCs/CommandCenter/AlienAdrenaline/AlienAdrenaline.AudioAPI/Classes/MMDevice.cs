using System.Runtime.InteropServices;
using AlienLabs.AlienAdrenaline.AudioAPI.Enums;

namespace AlienLabs.AlienAdrenaline.AudioAPI.Classes
{
    /// <summary>
    /// IMMDevice Wrapper Class
    /// </summary>
    public class MMDevice
    {
        #region Private Properties
        private IMMDevice realDevice;
        private PropertyStore propertyStore;
        //private AudioMeterInformation _AudioMeterInformation;
        //private AudioEndpointVolume _AudioEndpointVolume;
        //private AudioSessionManager _AudioSessionManager;
        #endregion

        #region Guids
        //private static Guid IID_IAudioMeterInformation = typeof(IAudioMeterInformation).GUID; 
        //private static Guid IID_IAudioEndpointVolume = typeof(IAudioEndpointVolume).GUID;  
        //private static Guid IID_IAudioSessionManager = typeof(IAudioSessionManager2).GUID;
        #endregion

        #region Properties
        //public AudioSessionManager AudioSessionManager
        //{
        //    get
        //    {
        //        if (_AudioSessionManager == null)
        //            GetAudioSessionManager();

        //        return _AudioSessionManager;
        //    }
        //}

        //public AudioMeterInformation AudioMeterInformation
        //{
        //    get
        //    {
        //        if (_AudioMeterInformation == null)
        //            GetAudioMeterInformation();

        //        return _AudioMeterInformation;
        //    }
        //}

        //public AudioEndpointVolume AudioEndpointVolume
        //{
        //    get
        //    {
        //        if (_AudioEndpointVolume == null)
        //            GetAudioEndpointVolume();

        //        return _AudioEndpointVolume;
        //    }
        //}

        public PropertyStore Properties
        {
            get
            {
                if (propertyStore == null)
                    GetPropertyInformation();
                return propertyStore;
            }
        }

        public string FriendlyName
        {
            get
            {
                if (propertyStore == null)
                    GetPropertyInformation();
                if (propertyStore.Contains(PKEY.PKEY_DeviceInterface_FriendlyName))
                {
                    return (string)propertyStore[PKEY.PKEY_DeviceInterface_FriendlyName].Value;
                }
                else
                    return "Unknown";
            }
        }

        public string IconPath
        {
            get
            {
                if (propertyStore == null)
                    GetPropertyInformation();
                if (propertyStore.Contains(PKEY.PKEY_DeviceInterface_IconPath))
                {
                    return (string)propertyStore[PKEY.PKEY_DeviceInterface_IconPath].Value;
                }
                else
                    return "Unknown";
            }
        }

        public string ID
        {
            get
            {
                string Result;
                Marshal.ThrowExceptionForHR(realDevice.GetId(out Result));
                return Result;
            }
        }

        public EDataFlow DataFlow
        {
            get
            {
                EDataFlow result = EDataFlow.eAll;
                var ep = realDevice as IMMEndpoint;
            	if (ep != null) 
					ep.GetDataFlow(out result);
            	return result;
            }
        }

        public EDeviceState State
        {
            get
            {
                EDeviceState Result;
                Marshal.ThrowExceptionForHR(realDevice.GetState(out Result));
                return Result;

            }
        }
        #endregion

        #region Constructors
        internal MMDevice(IMMDevice realDevice)
        {
            this.realDevice = realDevice;
        }
        #endregion

        #region Private Members
        private void GetPropertyInformation()
        {
            IPropertyStore propstore;
            Marshal.ThrowExceptionForHR(realDevice.OpenPropertyStore(EStgmAccess.STGM_READ, out propstore));
            propertyStore = new PropertyStore(propstore);
        }

        private void GetAudioSessionManager()
        {
            //object result;
            //Marshal.ThrowExceptionForHR(_RealDevice.Activate(ref IID_IAudioSessionManager, CLSCTX.ALL, IntPtr.Zero, out result));
            //_AudioSessionManager = new AudioSessionManager(result as IAudioSessionManager2);
        }

        private void GetAudioMeterInformation()
        {
            //object result;
            //Marshal.ThrowExceptionForHR( _RealDevice.Activate(ref IID_IAudioMeterInformation, CLSCTX.ALL, IntPtr.Zero, out result));
            //_AudioMeterInformation = new AudioMeterInformation( result as IAudioMeterInformation);
        }

        private void GetAudioEndpointVolume()
        {
            //object result;
            //Marshal.ThrowExceptionForHR(_RealDevice.Activate(ref IID_IAudioEndpointVolume, CLSCTX.ALL, IntPtr.Zero, out result));
            //_AudioEndpointVolume = new AudioEndpointVolume(result as IAudioEndpointVolume);
        }
        #endregion
    }
}
