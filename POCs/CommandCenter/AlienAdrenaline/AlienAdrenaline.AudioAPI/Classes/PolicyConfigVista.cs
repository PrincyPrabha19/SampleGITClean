using System;
using System.Runtime.InteropServices;
using AlienLabs.AlienAdrenaline.AudioAPI.Enums;

namespace AlienLabs.AlienAdrenaline.AudioAPI.Classes
{
    [ComImport, Guid("294935CE-F637-4E7C-A41B-AB255460B862")]
    internal class _PolicyConfigVista
    {        
    }

    /// <summary>
    /// IPolicyConfigVista Wrapper Class
    /// </summary>
    public class PolicyConfigVista
    {
        #region Private Properties
        private readonly IPolicyConfigVista realPolicyConfigVista = new _PolicyConfigVista() as IPolicyConfigVista;
        #endregion

        #region Constructors
        public PolicyConfigVista()
        {
            if (System.Environment.OSVersion.Version.Major < 6)
                throw new NotSupportedException("This functionality is only supported on Windows Vista or newer.");
        }
        #endregion

        #region Public Members
        public int SetDefaultEndPoint(string deviceId, ERole eRole)
        {
            int result = 0;
            result = realPolicyConfigVista.SetDefaultEndpoint(deviceId, eRole);
            return result;        
        }
        #endregion
    }
}
