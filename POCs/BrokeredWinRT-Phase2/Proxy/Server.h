

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.01.0620 */
/* at Mon Jan 18 22:14:07 2038
 */
/* Compiler settings for C:\Users\A1\AppData\Local\Temp\Server.idl-e0b442a4:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 8.01.0620 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif /* __RPCNDR_H_VERSION__ */

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __Server_h__
#define __Server_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ____x_Server_CIAlienFXAPIWrapperClass_FWD_DEFINED__
#define ____x_Server_CIAlienFXAPIWrapperClass_FWD_DEFINED__
typedef interface __x_Server_CIAlienFXAPIWrapperClass __x_Server_CIAlienFXAPIWrapperClass;

#ifdef __cplusplus
namespace Server {
    interface IAlienFXAPIWrapperClass;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIAlienFXAPIWrapperClass_FWD_DEFINED__ */


#ifndef ____x_Server_CIAlienFXAPIWrapperStatic_FWD_DEFINED__
#define ____x_Server_CIAlienFXAPIWrapperStatic_FWD_DEFINED__
typedef interface __x_Server_CIAlienFXAPIWrapperStatic __x_Server_CIAlienFXAPIWrapperStatic;

#ifdef __cplusplus
namespace Server {
    interface IAlienFXAPIWrapperStatic;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIAlienFXAPIWrapperStatic_FWD_DEFINED__ */


#ifndef ____x_Server_CIAlienFXBiosSupportAPI32MapClass_FWD_DEFINED__
#define ____x_Server_CIAlienFXBiosSupportAPI32MapClass_FWD_DEFINED__
typedef interface __x_Server_CIAlienFXBiosSupportAPI32MapClass __x_Server_CIAlienFXBiosSupportAPI32MapClass;

#ifdef __cplusplus
namespace Server {
    interface IAlienFXBiosSupportAPI32MapClass;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIAlienFXBiosSupportAPI32MapClass_FWD_DEFINED__ */


#ifndef ____x_Server_CIAlienFXBiosSupportAPI64MapClass_FWD_DEFINED__
#define ____x_Server_CIAlienFXBiosSupportAPI64MapClass_FWD_DEFINED__
typedef interface __x_Server_CIAlienFXBiosSupportAPI64MapClass __x_Server_CIAlienFXBiosSupportAPI64MapClass;

#ifdef __cplusplus
namespace Server {
    interface IAlienFXBiosSupportAPI64MapClass;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIAlienFXBiosSupportAPI64MapClass_FWD_DEFINED__ */


#ifndef ____x_Server_CIAlienFXComponentClass_FWD_DEFINED__
#define ____x_Server_CIAlienFXComponentClass_FWD_DEFINED__
typedef interface __x_Server_CIAlienFXComponentClass __x_Server_CIAlienFXComponentClass;

#ifdef __cplusplus
namespace Server {
    interface IAlienFXComponentClass;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIAlienFXComponentClass_FWD_DEFINED__ */


#ifndef ____x_Server_CIBIOSSupport_FWD_DEFINED__
#define ____x_Server_CIBIOSSupport_FWD_DEFINED__
typedef interface __x_Server_CIBIOSSupport __x_Server_CIBIOSSupport;

#ifdef __cplusplus
namespace Server {
    interface IBIOSSupport;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIBIOSSupport_FWD_DEFINED__ */


#ifndef ____x_Server_CIHostProcessManagerClass_FWD_DEFINED__
#define ____x_Server_CIHostProcessManagerClass_FWD_DEFINED__
typedef interface __x_Server_CIHostProcessManagerClass __x_Server_CIHostProcessManagerClass;

#ifdef __cplusplus
namespace Server {
    interface IHostProcessManagerClass;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIHostProcessManagerClass_FWD_DEFINED__ */


/* header files for imported files */
#include "inspectable.h"
#include "AsyncInfo.h"
#include "EventToken.h"
#include "Windows.Foundation.h"

#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_Server_0000_0000 */
/* [local] */ 

#if defined(__cplusplus)
}
#endif // defined(__cplusplus)
#include <Windows.Foundation.h>
#if defined(__cplusplus)
extern "C" {
#endif // defined(__cplusplus)








#ifdef __cplusplus
namespace Server {
class AlienFXAPIWrapper;
} /*Server*/
#endif
#ifdef __cplusplus
namespace Server {
class AlienFXBiosSupportAPI32Map;
} /*Server*/
#endif
#ifdef __cplusplus
namespace Server {
class AlienFXBiosSupportAPI64Map;
} /*Server*/
#endif
#ifdef __cplusplus
namespace Server {
class AlienFXComponent;
} /*Server*/
#endif
#ifdef __cplusplus
namespace Server {
class BIOSSupport;
} /*Server*/
#endif
#ifdef __cplusplus
namespace Server {
class HostProcessManager;
} /*Server*/
#endif
#if !defined(____x_Server_CIAlienFXAPIWrapperClass_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IAlienFXAPIWrapperClass[] = L"Server.IAlienFXAPIWrapperClass";
#endif /* !defined(____x_Server_CIAlienFXAPIWrapperClass_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0000 */
/* [local] */ 











extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0000_v0_0_s_ifspec;

#ifndef ____x_Server_CIAlienFXAPIWrapperClass_INTERFACE_DEFINED__
#define ____x_Server_CIAlienFXAPIWrapperClass_INTERFACE_DEFINED__

/* interface __x_Server_CIAlienFXAPIWrapperClass */
/* [uuid][object] */ 



/* interface Server::IAlienFXAPIWrapperClass */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIAlienFXAPIWrapperClass;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("09984274-161B-587B-5CB2-0C6807E0BF4C")
        IAlienFXAPIWrapperClass : public IInspectable
        {
        public:
        };

        extern const __declspec(selectany) IID & IID_IAlienFXAPIWrapperClass = __uuidof(IAlienFXAPIWrapperClass);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIAlienFXAPIWrapperClassVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIAlienFXAPIWrapperClass * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIAlienFXAPIWrapperClass * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIAlienFXAPIWrapperClass * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIAlienFXAPIWrapperClass * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIAlienFXAPIWrapperClass * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIAlienFXAPIWrapperClass * This,
            /* [out] */ TrustLevel *trustLevel);
        
        END_INTERFACE
    } __x_Server_CIAlienFXAPIWrapperClassVtbl;

    interface __x_Server_CIAlienFXAPIWrapperClass
    {
        CONST_VTBL struct __x_Server_CIAlienFXAPIWrapperClassVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIAlienFXAPIWrapperClass_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIAlienFXAPIWrapperClass_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIAlienFXAPIWrapperClass_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIAlienFXAPIWrapperClass_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIAlienFXAPIWrapperClass_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIAlienFXAPIWrapperClass_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIAlienFXAPIWrapperClass_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0001 */
/* [local] */ 

#if !defined(____x_Server_CIAlienFXAPIWrapperStatic_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IAlienFXAPIWrapperStatic[] = L"Server.IAlienFXAPIWrapperStatic";
#endif /* !defined(____x_Server_CIAlienFXAPIWrapperStatic_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0001 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0001_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0001_v0_0_s_ifspec;

#ifndef ____x_Server_CIAlienFXAPIWrapperStatic_INTERFACE_DEFINED__
#define ____x_Server_CIAlienFXAPIWrapperStatic_INTERFACE_DEFINED__

/* interface __x_Server_CIAlienFXAPIWrapperStatic */
/* [uuid][object] */ 



/* interface Server::IAlienFXAPIWrapperStatic */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIAlienFXAPIWrapperStatic;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("B0B063D7-079E-5E70-5650-6D34B8A0FEC2")
        IAlienFXAPIWrapperStatic : public IInspectable
        {
        public:
            virtual HRESULT STDMETHODCALLTYPE Initialize( 
                /* [out][retval] */ INT32 *value) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE log( 
                /* [in] */ HSTRING msg) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE Release( 
                /* [out][retval] */ INT32 *value) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE SetDimligths( 
                /* [in] */ UINT32 leds,
                /* [in] */ INT32 brightness,
                /* [out][retval] */ INT32 *value) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE SetLightColorDataForPState( 
                /* [in] */ UINT32 leds,
                /* [in] */ UINT32 color,
                /* [in] */ INT32 state,
                /* [out][retval] */ INT32 *value) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE SetLightColor( 
                /* [in] */ UINT32 leds,
                /* [in] */ UINT32 color,
                /* [out][retval] */ INT32 *value) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_IAlienFXAPIWrapperStatic = __uuidof(IAlienFXAPIWrapperStatic);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIAlienFXAPIWrapperStaticVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIAlienFXAPIWrapperStatic * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIAlienFXAPIWrapperStatic * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIAlienFXAPIWrapperStatic * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIAlienFXAPIWrapperStatic * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIAlienFXAPIWrapperStatic * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIAlienFXAPIWrapperStatic * This,
            /* [out] */ TrustLevel *trustLevel);
        
        HRESULT ( STDMETHODCALLTYPE *Initialize )( 
            __x_Server_CIAlienFXAPIWrapperStatic * This,
            /* [out][retval] */ INT32 *value);
        
        HRESULT ( STDMETHODCALLTYPE *log )( 
            __x_Server_CIAlienFXAPIWrapperStatic * This,
            /* [in] */ HSTRING msg);
        
        HRESULT ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIAlienFXAPIWrapperStatic * This,
            /* [out][retval] */ INT32 *value);
        
        HRESULT ( STDMETHODCALLTYPE *SetDimligths )( 
            __x_Server_CIAlienFXAPIWrapperStatic * This,
            /* [in] */ UINT32 leds,
            /* [in] */ INT32 brightness,
            /* [out][retval] */ INT32 *value);
        
        HRESULT ( STDMETHODCALLTYPE *SetLightColorDataForPState )( 
            __x_Server_CIAlienFXAPIWrapperStatic * This,
            /* [in] */ UINT32 leds,
            /* [in] */ UINT32 color,
            /* [in] */ INT32 state,
            /* [out][retval] */ INT32 *value);
        
        HRESULT ( STDMETHODCALLTYPE *SetLightColor )( 
            __x_Server_CIAlienFXAPIWrapperStatic * This,
            /* [in] */ UINT32 leds,
            /* [in] */ UINT32 color,
            /* [out][retval] */ INT32 *value);
        
        END_INTERFACE
    } __x_Server_CIAlienFXAPIWrapperStaticVtbl;

    interface __x_Server_CIAlienFXAPIWrapperStatic
    {
        CONST_VTBL struct __x_Server_CIAlienFXAPIWrapperStaticVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIAlienFXAPIWrapperStatic_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIAlienFXAPIWrapperStatic_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIAlienFXAPIWrapperStatic_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIAlienFXAPIWrapperStatic_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIAlienFXAPIWrapperStatic_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIAlienFXAPIWrapperStatic_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CIAlienFXAPIWrapperStatic_Initialize(This,value)	\
    ( (This)->lpVtbl -> Initialize(This,value) ) 

#define __x_Server_CIAlienFXAPIWrapperStatic_log(This,msg)	\
    ( (This)->lpVtbl -> log(This,msg) ) 

#define __x_Server_CIAlienFXAPIWrapperStatic_Release(This,value)	\
    ( (This)->lpVtbl -> Release(This,value) ) 

#define __x_Server_CIAlienFXAPIWrapperStatic_SetDimligths(This,leds,brightness,value)	\
    ( (This)->lpVtbl -> SetDimligths(This,leds,brightness,value) ) 

#define __x_Server_CIAlienFXAPIWrapperStatic_SetLightColorDataForPState(This,leds,color,state,value)	\
    ( (This)->lpVtbl -> SetLightColorDataForPState(This,leds,color,state,value) ) 

#define __x_Server_CIAlienFXAPIWrapperStatic_SetLightColor(This,leds,color,value)	\
    ( (This)->lpVtbl -> SetLightColor(This,leds,color,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIAlienFXAPIWrapperStatic_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0002 */
/* [local] */ 

#if !defined(____x_Server_CIAlienFXBiosSupportAPI32MapClass_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IAlienFXBiosSupportAPI32MapClass[] = L"Server.IAlienFXBiosSupportAPI32MapClass";
#endif /* !defined(____x_Server_CIAlienFXBiosSupportAPI32MapClass_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0002 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0002_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0002_v0_0_s_ifspec;

#ifndef ____x_Server_CIAlienFXBiosSupportAPI32MapClass_INTERFACE_DEFINED__
#define ____x_Server_CIAlienFXBiosSupportAPI32MapClass_INTERFACE_DEFINED__

/* interface __x_Server_CIAlienFXBiosSupportAPI32MapClass */
/* [uuid][object] */ 



/* interface Server::IAlienFXBiosSupportAPI32MapClass */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIAlienFXBiosSupportAPI32MapClass;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("C12232EF-E8A1-5982-662D-F997C348B676")
        IAlienFXBiosSupportAPI32MapClass : public IInspectable
        {
        public:
        };

        extern const __declspec(selectany) IID & IID_IAlienFXBiosSupportAPI32MapClass = __uuidof(IAlienFXBiosSupportAPI32MapClass);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIAlienFXBiosSupportAPI32MapClassVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIAlienFXBiosSupportAPI32MapClass * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIAlienFXBiosSupportAPI32MapClass * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIAlienFXBiosSupportAPI32MapClass * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIAlienFXBiosSupportAPI32MapClass * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIAlienFXBiosSupportAPI32MapClass * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIAlienFXBiosSupportAPI32MapClass * This,
            /* [out] */ TrustLevel *trustLevel);
        
        END_INTERFACE
    } __x_Server_CIAlienFXBiosSupportAPI32MapClassVtbl;

    interface __x_Server_CIAlienFXBiosSupportAPI32MapClass
    {
        CONST_VTBL struct __x_Server_CIAlienFXBiosSupportAPI32MapClassVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIAlienFXBiosSupportAPI32MapClass_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIAlienFXBiosSupportAPI32MapClass_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIAlienFXBiosSupportAPI32MapClass_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIAlienFXBiosSupportAPI32MapClass_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIAlienFXBiosSupportAPI32MapClass_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIAlienFXBiosSupportAPI32MapClass_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIAlienFXBiosSupportAPI32MapClass_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0003 */
/* [local] */ 

#if !defined(____x_Server_CIAlienFXBiosSupportAPI64MapClass_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IAlienFXBiosSupportAPI64MapClass[] = L"Server.IAlienFXBiosSupportAPI64MapClass";
#endif /* !defined(____x_Server_CIAlienFXBiosSupportAPI64MapClass_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0003 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0003_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0003_v0_0_s_ifspec;

#ifndef ____x_Server_CIAlienFXBiosSupportAPI64MapClass_INTERFACE_DEFINED__
#define ____x_Server_CIAlienFXBiosSupportAPI64MapClass_INTERFACE_DEFINED__

/* interface __x_Server_CIAlienFXBiosSupportAPI64MapClass */
/* [uuid][object] */ 



/* interface Server::IAlienFXBiosSupportAPI64MapClass */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIAlienFXBiosSupportAPI64MapClass;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("078000CA-944B-53ED-58CE-F8119D207750")
        IAlienFXBiosSupportAPI64MapClass : public IInspectable
        {
        public:
        };

        extern const __declspec(selectany) IID & IID_IAlienFXBiosSupportAPI64MapClass = __uuidof(IAlienFXBiosSupportAPI64MapClass);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIAlienFXBiosSupportAPI64MapClassVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIAlienFXBiosSupportAPI64MapClass * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIAlienFXBiosSupportAPI64MapClass * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIAlienFXBiosSupportAPI64MapClass * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIAlienFXBiosSupportAPI64MapClass * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIAlienFXBiosSupportAPI64MapClass * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIAlienFXBiosSupportAPI64MapClass * This,
            /* [out] */ TrustLevel *trustLevel);
        
        END_INTERFACE
    } __x_Server_CIAlienFXBiosSupportAPI64MapClassVtbl;

    interface __x_Server_CIAlienFXBiosSupportAPI64MapClass
    {
        CONST_VTBL struct __x_Server_CIAlienFXBiosSupportAPI64MapClassVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIAlienFXBiosSupportAPI64MapClass_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIAlienFXBiosSupportAPI64MapClass_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIAlienFXBiosSupportAPI64MapClass_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIAlienFXBiosSupportAPI64MapClass_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIAlienFXBiosSupportAPI64MapClass_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIAlienFXBiosSupportAPI64MapClass_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIAlienFXBiosSupportAPI64MapClass_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0004 */
/* [local] */ 

#if !defined(____x_Server_CIAlienFXComponentClass_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IAlienFXComponentClass[] = L"Server.IAlienFXComponentClass";
#endif /* !defined(____x_Server_CIAlienFXComponentClass_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0004 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0004_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0004_v0_0_s_ifspec;

#ifndef ____x_Server_CIAlienFXComponentClass_INTERFACE_DEFINED__
#define ____x_Server_CIAlienFXComponentClass_INTERFACE_DEFINED__

/* interface __x_Server_CIAlienFXComponentClass */
/* [uuid][object] */ 



/* interface Server::IAlienFXComponentClass */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIAlienFXComponentClass;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("7FF282C0-676C-5793-51F7-C898B33C237C")
        IAlienFXComponentClass : public IInspectable
        {
        public:
            virtual HRESULT STDMETHODCALLTYPE InitializeAPI( 
                /* [out][retval] */ boolean *value) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE ReleaseAPI( 
                /* [out][retval] */ boolean *value) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE SetLightColor( 
                /* [in] */ UINT32 leds,
                /* [in] */ UINT32 color,
                /* [out][retval] */ boolean *value) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_IAlienFXComponentClass = __uuidof(IAlienFXComponentClass);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIAlienFXComponentClassVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIAlienFXComponentClass * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIAlienFXComponentClass * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIAlienFXComponentClass * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIAlienFXComponentClass * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIAlienFXComponentClass * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIAlienFXComponentClass * This,
            /* [out] */ TrustLevel *trustLevel);
        
        HRESULT ( STDMETHODCALLTYPE *InitializeAPI )( 
            __x_Server_CIAlienFXComponentClass * This,
            /* [out][retval] */ boolean *value);
        
        HRESULT ( STDMETHODCALLTYPE *ReleaseAPI )( 
            __x_Server_CIAlienFXComponentClass * This,
            /* [out][retval] */ boolean *value);
        
        HRESULT ( STDMETHODCALLTYPE *SetLightColor )( 
            __x_Server_CIAlienFXComponentClass * This,
            /* [in] */ UINT32 leds,
            /* [in] */ UINT32 color,
            /* [out][retval] */ boolean *value);
        
        END_INTERFACE
    } __x_Server_CIAlienFXComponentClassVtbl;

    interface __x_Server_CIAlienFXComponentClass
    {
        CONST_VTBL struct __x_Server_CIAlienFXComponentClassVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIAlienFXComponentClass_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIAlienFXComponentClass_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIAlienFXComponentClass_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIAlienFXComponentClass_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIAlienFXComponentClass_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIAlienFXComponentClass_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CIAlienFXComponentClass_InitializeAPI(This,value)	\
    ( (This)->lpVtbl -> InitializeAPI(This,value) ) 

#define __x_Server_CIAlienFXComponentClass_ReleaseAPI(This,value)	\
    ( (This)->lpVtbl -> ReleaseAPI(This,value) ) 

#define __x_Server_CIAlienFXComponentClass_SetLightColor(This,leds,color,value)	\
    ( (This)->lpVtbl -> SetLightColor(This,leds,color,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIAlienFXComponentClass_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0005 */
/* [local] */ 

#if !defined(____x_Server_CIBIOSSupport_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IBIOSSupport[] = L"Server.IBIOSSupport";
#endif /* !defined(____x_Server_CIBIOSSupport_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0005 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0005_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0005_v0_0_s_ifspec;

#ifndef ____x_Server_CIBIOSSupport_INTERFACE_DEFINED__
#define ____x_Server_CIBIOSSupport_INTERFACE_DEFINED__

/* interface __x_Server_CIBIOSSupport */
/* [uuid][object] */ 



/* interface Server::IBIOSSupport */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIBIOSSupport;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("A6DD498F-D652-513D-747D-57566515F3DA")
        IBIOSSupport : public IInspectable
        {
        public:
            virtual HRESULT STDMETHODCALLTYPE Ping( 
                /* [out][retval] */ HSTRING *value) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE Initialize( 
                /* [out][retval] */ boolean *value) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE Release( 
                /* [out][retval] */ boolean *value) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE SetLightColor( 
                /* [in] */ UINT32 leds,
                /* [in] */ UINT32 color,
                /* [out][retval] */ boolean *value) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_IBIOSSupport = __uuidof(IBIOSSupport);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIBIOSSupportVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIBIOSSupport * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIBIOSSupport * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIBIOSSupport * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIBIOSSupport * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIBIOSSupport * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIBIOSSupport * This,
            /* [out] */ TrustLevel *trustLevel);
        
        HRESULT ( STDMETHODCALLTYPE *Ping )( 
            __x_Server_CIBIOSSupport * This,
            /* [out][retval] */ HSTRING *value);
        
        HRESULT ( STDMETHODCALLTYPE *Initialize )( 
            __x_Server_CIBIOSSupport * This,
            /* [out][retval] */ boolean *value);
        
        HRESULT ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIBIOSSupport * This,
            /* [out][retval] */ boolean *value);
        
        HRESULT ( STDMETHODCALLTYPE *SetLightColor )( 
            __x_Server_CIBIOSSupport * This,
            /* [in] */ UINT32 leds,
            /* [in] */ UINT32 color,
            /* [out][retval] */ boolean *value);
        
        END_INTERFACE
    } __x_Server_CIBIOSSupportVtbl;

    interface __x_Server_CIBIOSSupport
    {
        CONST_VTBL struct __x_Server_CIBIOSSupportVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIBIOSSupport_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIBIOSSupport_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIBIOSSupport_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIBIOSSupport_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIBIOSSupport_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIBIOSSupport_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CIBIOSSupport_Ping(This,value)	\
    ( (This)->lpVtbl -> Ping(This,value) ) 

#define __x_Server_CIBIOSSupport_Initialize(This,value)	\
    ( (This)->lpVtbl -> Initialize(This,value) ) 

#define __x_Server_CIBIOSSupport_Release(This,value)	\
    ( (This)->lpVtbl -> Release(This,value) ) 

#define __x_Server_CIBIOSSupport_SetLightColor(This,leds,color,value)	\
    ( (This)->lpVtbl -> SetLightColor(This,leds,color,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIBIOSSupport_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0006 */
/* [local] */ 

#if !defined(____x_Server_CIHostProcessManagerClass_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IHostProcessManagerClass[] = L"Server.IHostProcessManagerClass";
#endif /* !defined(____x_Server_CIHostProcessManagerClass_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0006 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0006_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0006_v0_0_s_ifspec;

#ifndef ____x_Server_CIHostProcessManagerClass_INTERFACE_DEFINED__
#define ____x_Server_CIHostProcessManagerClass_INTERFACE_DEFINED__

/* interface __x_Server_CIHostProcessManagerClass */
/* [uuid][object] */ 



/* interface Server::IHostProcessManagerClass */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIHostProcessManagerClass;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("7419718A-B1ED-532A-4166-5C28B8291EAF")
        IHostProcessManagerClass : public IInspectable
        {
        public:
            virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_ProcessId( 
                /* [out][retval] */ INT32 *value) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE KillProcess( void) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_IHostProcessManagerClass = __uuidof(IHostProcessManagerClass);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIHostProcessManagerClassVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIHostProcessManagerClass * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIHostProcessManagerClass * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIHostProcessManagerClass * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIHostProcessManagerClass * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIHostProcessManagerClass * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIHostProcessManagerClass * This,
            /* [out] */ TrustLevel *trustLevel);
        
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_ProcessId )( 
            __x_Server_CIHostProcessManagerClass * This,
            /* [out][retval] */ INT32 *value);
        
        HRESULT ( STDMETHODCALLTYPE *KillProcess )( 
            __x_Server_CIHostProcessManagerClass * This);
        
        END_INTERFACE
    } __x_Server_CIHostProcessManagerClassVtbl;

    interface __x_Server_CIHostProcessManagerClass
    {
        CONST_VTBL struct __x_Server_CIHostProcessManagerClassVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIHostProcessManagerClass_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIHostProcessManagerClass_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIHostProcessManagerClass_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIHostProcessManagerClass_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIHostProcessManagerClass_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIHostProcessManagerClass_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CIHostProcessManagerClass_get_ProcessId(This,value)	\
    ( (This)->lpVtbl -> get_ProcessId(This,value) ) 

#define __x_Server_CIHostProcessManagerClass_KillProcess(This)	\
    ( (This)->lpVtbl -> KillProcess(This) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIHostProcessManagerClass_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0007 */
/* [local] */ 

#ifndef RUNTIMECLASS_Server_AlienFXAPIWrapper_DEFINED
#define RUNTIMECLASS_Server_AlienFXAPIWrapper_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_AlienFXAPIWrapper[] = L"Server.AlienFXAPIWrapper";
#endif
#ifndef RUNTIMECLASS_Server_AlienFXBiosSupportAPI32Map_DEFINED
#define RUNTIMECLASS_Server_AlienFXBiosSupportAPI32Map_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_AlienFXBiosSupportAPI32Map[] = L"Server.AlienFXBiosSupportAPI32Map";
#endif
#ifndef RUNTIMECLASS_Server_AlienFXBiosSupportAPI64Map_DEFINED
#define RUNTIMECLASS_Server_AlienFXBiosSupportAPI64Map_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_AlienFXBiosSupportAPI64Map[] = L"Server.AlienFXBiosSupportAPI64Map";
#endif
#ifndef RUNTIMECLASS_Server_AlienFXComponent_DEFINED
#define RUNTIMECLASS_Server_AlienFXComponent_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_AlienFXComponent[] = L"Server.AlienFXComponent";
#endif
#ifndef RUNTIMECLASS_Server_BIOSSupport_DEFINED
#define RUNTIMECLASS_Server_BIOSSupport_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_BIOSSupport[] = L"Server.BIOSSupport";
#endif
#ifndef RUNTIMECLASS_Server_HostProcessManager_DEFINED
#define RUNTIMECLASS_Server_HostProcessManager_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_HostProcessManager[] = L"Server.HostProcessManager";
#endif


/* interface __MIDL_itf_Server_0000_0007 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0007_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0007_v0_0_s_ifspec;

/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  HSTRING_UserSize(     unsigned long *, unsigned long            , HSTRING * ); 
unsigned char * __RPC_USER  HSTRING_UserMarshal(  unsigned long *, unsigned char *, HSTRING * ); 
unsigned char * __RPC_USER  HSTRING_UserUnmarshal(unsigned long *, unsigned char *, HSTRING * ); 
void                      __RPC_USER  HSTRING_UserFree(     unsigned long *, HSTRING * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


