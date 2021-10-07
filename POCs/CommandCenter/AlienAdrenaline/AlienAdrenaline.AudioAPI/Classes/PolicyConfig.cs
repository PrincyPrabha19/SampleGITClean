using System;
using System.Runtime.InteropServices;
using AlienLabs.AlienAdrenaline.AudioAPI.Enums;

namespace AlienLabs.AlienAdrenaline.AudioAPI.Classes
{
    [ComImport, Guid("870AF99C-171D-4F9E-AF0D-E63DF40C2BC9")]
    internal class _PolicyConfig
    {        
    }

    /// <summary>
    /// IPolicyConfig Wrapper Class
    /// </summary>
    public class PolicyConfig
    {
        #region Private Properties
        private readonly IPolicyConfig realPolicyConfig = new _PolicyConfig() as IPolicyConfig;
        #endregion

        #region Constructors
        public PolicyConfig()
        {
            if (System.Environment.OSVersion.Version.Major < 6)
                throw new NotSupportedException("This functionality is only supported on Windows Vista or newer.");
        }
        #endregion

        #region Public Members
        public int SetDefaultEndPoint(string deviceId, ERole eRole)
        {
            int result = 0;
            result = realPolicyConfig.SetDefaultEndpoint(deviceId, eRole);
            return result;

            //IMMDevice _Device = null;
            //Marshal.ThrowExceptionForHR(((IMMDeviceEnumerator)_realEnumerator).GetDefaultAudioEndpoint(dataFlow, role, out _Device));
            //return new MMDevice(_Device);            
        }
        #endregion
    }
}
