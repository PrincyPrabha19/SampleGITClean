using System;
using System.Runtime.InteropServices;
using AlienLabs.AlienAdrenaline.AudioAPI.Enums;
using AlienLabs.AlienAdrenaline.AudioAPI.Structs;

namespace AlienLabs.AlienAdrenaline.AudioAPI
{
    /// <summary>
    /// class CPolicyConfigVistaClient
    /// {294935CE-F637-4E7C-A41B-AB255460B862}
    ///  
    /// interface IPolicyConfigVista
    /// {568b9108-44bf-40b4-9006-86afe5b5a620}
    ///
    /// Query interface:
    /// CComPtr<IPolicyConfigVista> PolicyConfig;
    /// PolicyConfig.CoCreateInstance(__uuidof(CPolicyConfigVistaClient));
    /// 
    /// @compatible: Windows Vista and Later
    /// </summary>

    [Guid("568B9108-44BF-40B4-9006-86AFE5B5A620"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPolicyConfigVista
    {
        //virtual HRESULT GetMixFormat(PCWSTR, WAVEFORMATEX **); // not available on Windows 7, use method from IPolicyConfig
        int GetMixFormat(string _PCWSTR, ref WAVEFORMATEX _WAVEFORMATEX); 

        //virtual HRESULT STDMETHODCALLTYPE GetDeviceFormat(PCWSTR, INT, WAVEFORMATEX **);
        int GetDeviceFormat(string _PCWSTR, int _INT, ref WAVEFORMATEX _WAVEFORMATEX);

        //virtual HRESULT STDMETHODCALLTYPE SetDeviceFormat(PCWSTR, WAVEFORMATEX *, WAVEFORMATEX *);
        int SetDeviceFormat(string _PCWSTR, WAVEFORMATEX _WAVEFORMATEX_1, WAVEFORMATEX _WAVEFORMATEX_2);

        //virtual HRESULT STDMETHODCALLTYPE GetProcessingPeriod(PCWSTR, INT, PINT64, PINT64);  // not available on Windows 7, use method from IPolicyConfig
        int GetProcessingPeriod(string _PCWSTR, int _INT, ref long _PINT64_1, ref long _PINT64_2);

        //virtual HRESULT STDMETHODCALLTYPE SetProcessingPeriod(PCWSTR, PINT64); // not available on Windows 7, use method from IPolicyConfig
        int SetDeviceFormat(string _PCWSTR, ref long _PINT64);

        //virtual HRESULT STDMETHODCALLTYPE GetShareMode(PCWSTR,struct DeviceShareMode *); // not available on Windows 7, use method from IPolicyConfig
        int GetShareMode(string _PCWSTR, ref IntPtr _DeviceShareMode);

        //virtual HRESULT STDMETHODCALLTYPE SetShareMode(PCWSTR,struct DeviceShareMode *); // not available on Windows 7, use method from IPolicyConfig
        int SetShareMode(string _PCWSTR, ref IntPtr _DeviceShareMode);

        //virtual HRESULT STDMETHODCALLTYPE GetPropertyValue(PCWSTR,const PROPERTYKEY &,PROPVARIANT *);
        int GetPropertyValue(string _PCWSTR, PROPERTYKEY _PROPERTYKEY, ref PROPVARIANT _PROPVARIANT);

        //virtual HRESULT STDMETHODCALLTYPE SetPropertyValue(PCWSTR,const PROPERTYKEY &,PROPVARIANT *);
        int SetPropertyValue(string _PCWSTR, PROPERTYKEY _PROPERTYKEY, ref PROPVARIANT _PROPVARIANT);

        //virtual HRESULT STDMETHODCALLTYPE SetDefaultEndpoint(__in PCWSTR wszDeviceId,__in ERole eRole );
        int SetDefaultEndpoint(string _PCWSTR, ERole _ERole);

        //virtual HRESULT STDMETHODCALLTYPE SetEndpointVisibility(PCWSTR,INT); // not available on Windows 7, use method from IPolicyConfig
        int SetEndpointVisibility(string _PCWSTR, int _INT);
    }
}
