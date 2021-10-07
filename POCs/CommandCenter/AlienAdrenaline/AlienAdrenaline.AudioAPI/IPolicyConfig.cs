using System;
using System.Runtime.InteropServices;
using AlienLabs.AlienAdrenaline.AudioAPI.Enums;
using AlienLabs.AlienAdrenaline.AudioAPI.Structs;

namespace AlienLabs.AlienAdrenaline.AudioAPI
{
    /// <summary>
    /// class CPolicyConfigClient
    /// {870af99c-171d-4f9e-af0d-e63df40c2bc9}
    ///  
    /// interface IPolicyConfig
    /// {f8679f50-850a-41cf-9c72-430f290290c8}
    ///
    /// Query interface:
    /// CComPtr<IPolicyConfig> PolicyConfig;
    /// PolicyConfig.CoCreateInstance(__uuidof(CPolicyConfigClient));
    /// 
    /// @compatible: Windows 7 and Later    
    /// </summary>
    
    [Guid("F8679F50-850A-41CF-9C72-430F290290C8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPolicyConfig
    {
        //virtual HRESULT GetMixFormat(PCWSTR, WAVEFORMATEX **);
        int GetMixFormat(string _PCWSTR, ref WAVEFORMATEX _WAVEFORMATEX);

        //virtual HRESULT STDMETHODCALLTYPE GetDeviceFormat(PCWSTR, INT, WAVEFORMATEX **);
        int GetDeviceFormat(string _PCWSTR, int _INT, ref WAVEFORMATEX _WAVEFORMATEX);

        //virtual HRESULT STDMETHODCALLTYPE ResetDeviceFormat(PCWSTR);
        int ResetDeviceFormat(string _PCWSTR);

        //virtual HRESULT STDMETHODCALLTYPE SetDeviceFormat(PCWSTR, WAVEFORMATEX *, WAVEFORMATEX *);
        int SetDeviceFormat(string _PCWSTR, WAVEFORMATEX _WAVEFORMATEX_1, WAVEFORMATEX _WAVEFORMATEX_2);

        //virtual HRESULT STDMETHODCALLTYPE GetProcessingPeriod(PCWSTR, INT, PINT64, PINT64);
        int GetProcessingPeriod(string _PCWSTR, int _INT, ref long _PINT64_1, ref long _PINT64_2);

        //virtual HRESULT STDMETHODCALLTYPE SetProcessingPeriod(PCWSTR, PINT64);
        int SetDeviceFormat(string _PCWSTR, ref long _PINT64);

        //virtual HRESULT STDMETHODCALLTYPE GetShareMode(PCWSTR,struct DeviceShareMode *);
        int GetShareMode(string _PCWSTR, ref IntPtr _DeviceShareMode);

        //virtual HRESULT STDMETHODCALLTYPE SetShareMode(PCWSTR,struct DeviceShareMode *);
        int SetShareMode(string _PCWSTR, ref IntPtr _DeviceShareMode);

        //virtual HRESULT STDMETHODCALLTYPE GetPropertyValue(PCWSTR,const PROPERTYKEY &,PROPVARIANT *);
        int GetPropertyValue(string _PCWSTR, PROPERTYKEY _PROPERTYKEY, ref PROPVARIANT _PROPVARIANT);

        //virtual HRESULT STDMETHODCALLTYPE SetPropertyValue(PCWSTR,const PROPERTYKEY &,PROPVARIANT *);
        int SetPropertyValue(string _PCWSTR, PROPERTYKEY _PROPERTYKEY, ref PROPVARIANT _PROPVARIANT);

        //virtual HRESULT STDMETHODCALLTYPE SetDefaultEndpoint(__in PCWSTR wszDeviceId,__in ERole eRole );
        int SetDefaultEndpoint(string _PCWSTR, ERole _ERole);

        //virtual HRESULT STDMETHODCALLTYPE SetEndpointVisibility(PCWSTR,INT);
        int SetEndpointVisibility(string _PCWSTR, int _INT);
    }
}
