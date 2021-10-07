

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.01.0620 */
/* at Mon Jan 18 22:14:07 2038
 */
/* Compiler settings for C:\Users\A1\AppData\Local\Temp\Server.idl-381bcb7c:
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

#ifndef ____FIIterator_1_Server__CAlienFXDeviceSetupInfo_FWD_DEFINED__
#define ____FIIterator_1_Server__CAlienFXDeviceSetupInfo_FWD_DEFINED__
typedef interface __FIIterator_1_Server__CAlienFXDeviceSetupInfo __FIIterator_1_Server__CAlienFXDeviceSetupInfo;

#endif 	/* ____FIIterator_1_Server__CAlienFXDeviceSetupInfo_FWD_DEFINED__ */


#ifndef ____FIIterable_1_Server__CAlienFXDeviceSetupInfo_FWD_DEFINED__
#define ____FIIterable_1_Server__CAlienFXDeviceSetupInfo_FWD_DEFINED__
typedef interface __FIIterable_1_Server__CAlienFXDeviceSetupInfo __FIIterable_1_Server__CAlienFXDeviceSetupInfo;

#endif 	/* ____FIIterable_1_Server__CAlienFXDeviceSetupInfo_FWD_DEFINED__ */


#ifndef ____x_Server_CDeviceSetupInfoReader_FWD_DEFINED__
#define ____x_Server_CDeviceSetupInfoReader_FWD_DEFINED__
typedef interface __x_Server_CDeviceSetupInfoReader __x_Server_CDeviceSetupInfoReader;

#ifdef __cplusplus
namespace Server {
    interface DeviceSetupInfoReader;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CDeviceSetupInfoReader_FWD_DEFINED__ */


#ifndef ____x_Server_CDeviceSetupInfoWriter_FWD_DEFINED__
#define ____x_Server_CDeviceSetupInfoWriter_FWD_DEFINED__
typedef interface __x_Server_CDeviceSetupInfoWriter __x_Server_CDeviceSetupInfoWriter;

#ifdef __cplusplus
namespace Server {
    interface DeviceSetupInfoWriter;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CDeviceSetupInfoWriter_FWD_DEFINED__ */


#ifndef ____x_Server_CIAFXSetupInfoHelperClass_FWD_DEFINED__
#define ____x_Server_CIAFXSetupInfoHelperClass_FWD_DEFINED__
typedef interface __x_Server_CIAFXSetupInfoHelperClass __x_Server_CIAFXSetupInfoHelperClass;

#ifdef __cplusplus
namespace Server {
    interface IAFXSetupInfoHelperClass;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIAFXSetupInfoHelperClass_FWD_DEFINED__ */


#ifndef ____x_Server_CIAFXSetupInfoHelperStatic_FWD_DEFINED__
#define ____x_Server_CIAFXSetupInfoHelperStatic_FWD_DEFINED__
typedef interface __x_Server_CIAFXSetupInfoHelperStatic __x_Server_CIAFXSetupInfoHelperStatic;

#ifdef __cplusplus
namespace Server {
    interface IAFXSetupInfoHelperStatic;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIAFXSetupInfoHelperStatic_FWD_DEFINED__ */


#ifndef ____x_Server_CIAlienFXComponentClass_FWD_DEFINED__
#define ____x_Server_CIAlienFXComponentClass_FWD_DEFINED__
typedef interface __x_Server_CIAlienFXComponentClass __x_Server_CIAlienFXComponentClass;

#ifdef __cplusplus
namespace Server {
    interface IAlienFXComponentClass;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIAlienFXComponentClass_FWD_DEFINED__ */


#ifndef ____x_Server_CIAlienFXDeviceDiscoveryService_FWD_DEFINED__
#define ____x_Server_CIAlienFXDeviceDiscoveryService_FWD_DEFINED__
typedef interface __x_Server_CIAlienFXDeviceDiscoveryService __x_Server_CIAlienFXDeviceDiscoveryService;

#ifdef __cplusplus
namespace Server {
    interface IAlienFXDeviceDiscoveryService;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIAlienFXDeviceDiscoveryService_FWD_DEFINED__ */


#ifndef ____x_Server_CIAlienFXDeviceSetupInfoFactoryClass_FWD_DEFINED__
#define ____x_Server_CIAlienFXDeviceSetupInfoFactoryClass_FWD_DEFINED__
typedef interface __x_Server_CIAlienFXDeviceSetupInfoFactoryClass __x_Server_CIAlienFXDeviceSetupInfoFactoryClass;

#ifdef __cplusplus
namespace Server {
    interface IAlienFXDeviceSetupInfoFactoryClass;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIAlienFXDeviceSetupInfoFactoryClass_FWD_DEFINED__ */


#ifndef ____x_Server_CIAlienFXDeviceSetupInfoFactoryStatic_FWD_DEFINED__
#define ____x_Server_CIAlienFXDeviceSetupInfoFactoryStatic_FWD_DEFINED__
typedef interface __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic;

#ifdef __cplusplus
namespace Server {
    interface IAlienFXDeviceSetupInfoFactoryStatic;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIAlienFXDeviceSetupInfoFactoryStatic_FWD_DEFINED__ */


#ifndef ____x_Server_CIHostProcessManagerClass_FWD_DEFINED__
#define ____x_Server_CIHostProcessManagerClass_FWD_DEFINED__
typedef interface __x_Server_CIHostProcessManagerClass __x_Server_CIHostProcessManagerClass;

#ifdef __cplusplus
namespace Server {
    interface IHostProcessManagerClass;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIHostProcessManagerClass_FWD_DEFINED__ */


#ifndef ____x_Server_CIModelProviderClassClass_FWD_DEFINED__
#define ____x_Server_CIModelProviderClassClass_FWD_DEFINED__
typedef interface __x_Server_CIModelProviderClassClass __x_Server_CIModelProviderClassClass;

#ifdef __cplusplus
namespace Server {
    interface IModelProviderClassClass;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIModelProviderClassClass_FWD_DEFINED__ */


#ifndef ____x_Server_CIObjectFactoryClass_FWD_DEFINED__
#define ____x_Server_CIObjectFactoryClass_FWD_DEFINED__
typedef interface __x_Server_CIObjectFactoryClass __x_Server_CIObjectFactoryClass;

#ifdef __cplusplus
namespace Server {
    interface IObjectFactoryClass;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIObjectFactoryClass_FWD_DEFINED__ */


#ifndef ____x_Server_CIRegistryDeviceSetupInfoReaderClass_FWD_DEFINED__
#define ____x_Server_CIRegistryDeviceSetupInfoReaderClass_FWD_DEFINED__
typedef interface __x_Server_CIRegistryDeviceSetupInfoReaderClass __x_Server_CIRegistryDeviceSetupInfoReaderClass;

#ifdef __cplusplus
namespace Server {
    interface IRegistryDeviceSetupInfoReaderClass;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIRegistryDeviceSetupInfoReaderClass_FWD_DEFINED__ */


#ifndef ____x_Server_CIRegistryDeviceSetupInfoWriterClass_FWD_DEFINED__
#define ____x_Server_CIRegistryDeviceSetupInfoWriterClass_FWD_DEFINED__
typedef interface __x_Server_CIRegistryDeviceSetupInfoWriterClass __x_Server_CIRegistryDeviceSetupInfoWriterClass;

#ifdef __cplusplus
namespace Server {
    interface IRegistryDeviceSetupInfoWriterClass;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIRegistryDeviceSetupInfoWriterClass_FWD_DEFINED__ */


#ifndef ____x_Server_CIRegistryServiceClassClass_FWD_DEFINED__
#define ____x_Server_CIRegistryServiceClassClass_FWD_DEFINED__
typedef interface __x_Server_CIRegistryServiceClassClass __x_Server_CIRegistryServiceClassClass;

#ifdef __cplusplus
namespace Server {
    interface IRegistryServiceClassClass;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIRegistryServiceClassClass_FWD_DEFINED__ */


#ifndef ____x_Server_CIRegistryServiceClassFactory_FWD_DEFINED__
#define ____x_Server_CIRegistryServiceClassFactory_FWD_DEFINED__
typedef interface __x_Server_CIRegistryServiceClassFactory __x_Server_CIRegistryServiceClassFactory;

#ifdef __cplusplus
namespace Server {
    interface IRegistryServiceClassFactory;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CIRegistryServiceClassFactory_FWD_DEFINED__ */


#ifndef ____x_Server_CModelProvider_FWD_DEFINED__
#define ____x_Server_CModelProvider_FWD_DEFINED__
typedef interface __x_Server_CModelProvider __x_Server_CModelProvider;

#ifdef __cplusplus
namespace Server {
    interface ModelProvider;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CModelProvider_FWD_DEFINED__ */


#ifndef ____x_Server_CModelReader_FWD_DEFINED__
#define ____x_Server_CModelReader_FWD_DEFINED__
typedef interface __x_Server_CModelReader __x_Server_CModelReader;

#ifdef __cplusplus
namespace Server {
    interface ModelReader;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CModelReader_FWD_DEFINED__ */


#ifndef ____x_Server_CRegistryService_FWD_DEFINED__
#define ____x_Server_CRegistryService_FWD_DEFINED__
typedef interface __x_Server_CRegistryService __x_Server_CRegistryService;

#ifdef __cplusplus
namespace Server {
    interface RegistryService;
} /* end namespace */

#endif /* __cplusplus */

#endif 	/* ____x_Server_CRegistryService_FWD_DEFINED__ */


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

#ifdef __cplusplus
} /*extern "C"*/ 
#endif
#include <windows.foundation.collections.h>
#ifdef __cplusplus
extern "C" {
#endif

#ifdef __cplusplus
namespace Server {
struct AlienFXDeviceSetupInfo;
} /*Server*/
#endif


/* interface __MIDL_itf_Server_0000_0000 */
/* [local] */ 




extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0000_v0_0_s_ifspec;

/* interface __MIDL_itf_Server2Eidl_0000_0325 */




/* interface __MIDL_itf_Server2Eidl_0000_0325 */




extern RPC_IF_HANDLE __MIDL_itf_Server2Eidl_0000_0325_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server2Eidl_0000_0325_v0_0_s_ifspec;

/* interface __MIDL_itf_Server_0000_0001 */
/* [local] */ 

#ifndef DEF___FIIterator_1_Server__CAlienFXDeviceSetupInfo_USE
#define DEF___FIIterator_1_Server__CAlienFXDeviceSetupInfo_USE
#if defined(__cplusplus) && !defined(RO_NO_TEMPLATE_NAME)
} /*extern "C"*/ 
namespace Windows { namespace Foundation { namespace Collections {
template <>
struct __declspec(uuid("bb7c3122-ed8d-5c12-975e-641b6111b980"))
IIterator<struct Server::AlienFXDeviceSetupInfo> : IIterator_impl<struct Server::AlienFXDeviceSetupInfo> {
static const wchar_t* z_get_rc_name_impl() {
return L"Windows.Foundation.Collections.IIterator`1<Server.AlienFXDeviceSetupInfo>"; }
};
typedef IIterator<struct Server::AlienFXDeviceSetupInfo> __FIIterator_1_Server__CAlienFXDeviceSetupInfo_t;
#define ____FIIterator_1_Server__CAlienFXDeviceSetupInfo_FWD_DEFINED__
#define __FIIterator_1_Server__CAlienFXDeviceSetupInfo Windows::Foundation::Collections::__FIIterator_1_Server__CAlienFXDeviceSetupInfo_t

/* Windows */ } /* Foundation */ } /* Collections */ }
extern "C" {
#endif //__cplusplus
#endif /* DEF___FIIterator_1_Server__CAlienFXDeviceSetupInfo_USE */


/* interface __MIDL_itf_Server_0000_0001 */
/* [local] */ 




extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0001_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0001_v0_0_s_ifspec;

/* interface __MIDL_itf_Server2Eidl_0000_0326 */




/* interface __MIDL_itf_Server2Eidl_0000_0326 */




extern RPC_IF_HANDLE __MIDL_itf_Server2Eidl_0000_0326_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server2Eidl_0000_0326_v0_0_s_ifspec;

/* interface __MIDL_itf_Server_0000_0002 */
/* [local] */ 

#ifndef DEF___FIIterable_1_Server__CAlienFXDeviceSetupInfo_USE
#define DEF___FIIterable_1_Server__CAlienFXDeviceSetupInfo_USE
#if defined(__cplusplus) && !defined(RO_NO_TEMPLATE_NAME)
} /*extern "C"*/ 
namespace Windows { namespace Foundation { namespace Collections {
template <>
struct __declspec(uuid("da389464-48ba-5337-8ae4-fc256d5e24f0"))
IIterable<struct Server::AlienFXDeviceSetupInfo> : IIterable_impl<struct Server::AlienFXDeviceSetupInfo> {
static const wchar_t* z_get_rc_name_impl() {
return L"Windows.Foundation.Collections.IIterable`1<Server.AlienFXDeviceSetupInfo>"; }
};
typedef IIterable<struct Server::AlienFXDeviceSetupInfo> __FIIterable_1_Server__CAlienFXDeviceSetupInfo_t;
#define ____FIIterable_1_Server__CAlienFXDeviceSetupInfo_FWD_DEFINED__
#define __FIIterable_1_Server__CAlienFXDeviceSetupInfo Windows::Foundation::Collections::__FIIterable_1_Server__CAlienFXDeviceSetupInfo_t

/* Windows */ } /* Foundation */ } /* Collections */ }
extern "C" {
#endif //__cplusplus
#endif /* DEF___FIIterable_1_Server__CAlienFXDeviceSetupInfo_USE */
#if defined(__cplusplus)
}
#endif // defined(__cplusplus)
#include <Windows.Foundation.h>
#if defined(__cplusplus)
extern "C" {
#endif // defined(__cplusplus)

#if !defined(__cplusplus)
typedef struct __x_Server_CAlienFXDeviceSetupInfo __x_Server_CAlienFXDeviceSetupInfo;

#endif


















#ifdef __cplusplus
namespace Server {
class AFXSetupInfoHelper;
} /*Server*/
#endif
#ifdef __cplusplus
namespace Server {
class AlienFXComponent;
} /*Server*/
#endif
#ifdef __cplusplus
namespace Server {
class AlienFXDeviceDiscoveryServiceClass;
} /*Server*/
#endif
#ifdef __cplusplus
namespace Server {
class AlienFXDeviceSetupInfoFactory;
} /*Server*/
#endif
#ifdef __cplusplus
namespace Server {
class HostProcessManager;
} /*Server*/
#endif
#ifdef __cplusplus
namespace Server {
class ModelProviderClass;
} /*Server*/
#endif
#ifdef __cplusplus
namespace Server {
class ObjectFactory;
} /*Server*/
#endif
#ifdef __cplusplus
namespace Server {
class RegistryDeviceSetupInfoReader;
} /*Server*/
#endif
#ifdef __cplusplus
namespace Server {
class RegistryDeviceSetupInfoWriter;
} /*Server*/
#endif
#ifdef __cplusplus
namespace Server {
class RegistryModelReader;
} /*Server*/
#endif
#ifdef __cplusplus
namespace Server {
class RegistryServiceClass;
} /*Server*/
#endif


/* interface __MIDL_itf_Server_0000_0002 */
/* [local] */ 



#ifdef __cplusplus

} /* end extern "C" */
namespace Server {
    
    typedef struct AlienFXDeviceSetupInfo AlienFXDeviceSetupInfo;
    
} /* end namespace */

extern "C" { 
#endif





















extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0002_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0002_v0_0_s_ifspec;

/* interface __MIDL_itf_Server2Eidl_0000_0327 */




/* interface __MIDL_itf_Server2Eidl_0000_0327 */




extern RPC_IF_HANDLE __MIDL_itf_Server2Eidl_0000_0327_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server2Eidl_0000_0327_v0_0_s_ifspec;

/* interface __MIDL_itf_Server_0000_0003 */
/* [local] */ 

#ifndef DEF___FIIterator_1_Server__CAlienFXDeviceSetupInfo
#define DEF___FIIterator_1_Server__CAlienFXDeviceSetupInfo
#if !defined(__cplusplus) || defined(RO_NO_TEMPLATE_NAME)


/* interface __MIDL_itf_Server_0000_0003 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0003_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0003_v0_0_s_ifspec;

#ifndef ____FIIterator_1_Server__CAlienFXDeviceSetupInfo_INTERFACE_DEFINED__
#define ____FIIterator_1_Server__CAlienFXDeviceSetupInfo_INTERFACE_DEFINED__

/* interface __FIIterator_1_Server__CAlienFXDeviceSetupInfo */
/* [unique][uuid][object] */ 



/* interface __FIIterator_1_Server__CAlienFXDeviceSetupInfo */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID___FIIterator_1_Server__CAlienFXDeviceSetupInfo;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("bb7c3122-ed8d-5c12-975e-641b6111b980")
    __FIIterator_1_Server__CAlienFXDeviceSetupInfo : public IInspectable
    {
    public:
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_Current( 
            /* [retval][out] */ struct Server::AlienFXDeviceSetupInfo *current) = 0;
        
        virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_HasCurrent( 
            /* [retval][out] */ boolean *hasCurrent) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE MoveNext( 
            /* [retval][out] */ boolean *hasCurrent) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetMany( 
            /* [in] */ unsigned int capacity,
            /* [size_is][length_is][out] */ struct Server::AlienFXDeviceSetupInfo *items,
            /* [retval][out] */ unsigned int *actual) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __FIIterator_1_Server__CAlienFXDeviceSetupInfoVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __FIIterator_1_Server__CAlienFXDeviceSetupInfo * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __FIIterator_1_Server__CAlienFXDeviceSetupInfo * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __FIIterator_1_Server__CAlienFXDeviceSetupInfo * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __FIIterator_1_Server__CAlienFXDeviceSetupInfo * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __FIIterator_1_Server__CAlienFXDeviceSetupInfo * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __FIIterator_1_Server__CAlienFXDeviceSetupInfo * This,
            /* [out] */ TrustLevel *trustLevel);
        
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_Current )( 
            __FIIterator_1_Server__CAlienFXDeviceSetupInfo * This,
            /* [retval][out] */ struct __x_Server_CAlienFXDeviceSetupInfo *current);
        
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_HasCurrent )( 
            __FIIterator_1_Server__CAlienFXDeviceSetupInfo * This,
            /* [retval][out] */ boolean *hasCurrent);
        
        HRESULT ( STDMETHODCALLTYPE *MoveNext )( 
            __FIIterator_1_Server__CAlienFXDeviceSetupInfo * This,
            /* [retval][out] */ boolean *hasCurrent);
        
        HRESULT ( STDMETHODCALLTYPE *GetMany )( 
            __FIIterator_1_Server__CAlienFXDeviceSetupInfo * This,
            /* [in] */ unsigned int capacity,
            /* [size_is][length_is][out] */ struct __x_Server_CAlienFXDeviceSetupInfo *items,
            /* [retval][out] */ unsigned int *actual);
        
        END_INTERFACE
    } __FIIterator_1_Server__CAlienFXDeviceSetupInfoVtbl;

    interface __FIIterator_1_Server__CAlienFXDeviceSetupInfo
    {
        CONST_VTBL struct __FIIterator_1_Server__CAlienFXDeviceSetupInfoVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __FIIterator_1_Server__CAlienFXDeviceSetupInfo_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __FIIterator_1_Server__CAlienFXDeviceSetupInfo_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __FIIterator_1_Server__CAlienFXDeviceSetupInfo_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __FIIterator_1_Server__CAlienFXDeviceSetupInfo_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __FIIterator_1_Server__CAlienFXDeviceSetupInfo_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __FIIterator_1_Server__CAlienFXDeviceSetupInfo_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __FIIterator_1_Server__CAlienFXDeviceSetupInfo_get_Current(This,current)	\
    ( (This)->lpVtbl -> get_Current(This,current) ) 

#define __FIIterator_1_Server__CAlienFXDeviceSetupInfo_get_HasCurrent(This,hasCurrent)	\
    ( (This)->lpVtbl -> get_HasCurrent(This,hasCurrent) ) 

#define __FIIterator_1_Server__CAlienFXDeviceSetupInfo_MoveNext(This,hasCurrent)	\
    ( (This)->lpVtbl -> MoveNext(This,hasCurrent) ) 

#define __FIIterator_1_Server__CAlienFXDeviceSetupInfo_GetMany(This,capacity,items,actual)	\
    ( (This)->lpVtbl -> GetMany(This,capacity,items,actual) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____FIIterator_1_Server__CAlienFXDeviceSetupInfo_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0004 */
/* [local] */ 

#endif /* pinterface */
#endif /* DEF___FIIterator_1_Server__CAlienFXDeviceSetupInfo */


/* interface __MIDL_itf_Server_0000_0004 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0004_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0004_v0_0_s_ifspec;

/* interface __MIDL_itf_Server2Eidl_0000_0328 */




/* interface __MIDL_itf_Server2Eidl_0000_0328 */




extern RPC_IF_HANDLE __MIDL_itf_Server2Eidl_0000_0328_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server2Eidl_0000_0328_v0_0_s_ifspec;

/* interface __MIDL_itf_Server_0000_0005 */
/* [local] */ 

#ifndef DEF___FIIterable_1_Server__CAlienFXDeviceSetupInfo
#define DEF___FIIterable_1_Server__CAlienFXDeviceSetupInfo
#if !defined(__cplusplus) || defined(RO_NO_TEMPLATE_NAME)


/* interface __MIDL_itf_Server_0000_0005 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0005_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0005_v0_0_s_ifspec;

#ifndef ____FIIterable_1_Server__CAlienFXDeviceSetupInfo_INTERFACE_DEFINED__
#define ____FIIterable_1_Server__CAlienFXDeviceSetupInfo_INTERFACE_DEFINED__

/* interface __FIIterable_1_Server__CAlienFXDeviceSetupInfo */
/* [unique][uuid][object] */ 



/* interface __FIIterable_1_Server__CAlienFXDeviceSetupInfo */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID___FIIterable_1_Server__CAlienFXDeviceSetupInfo;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("da389464-48ba-5337-8ae4-fc256d5e24f0")
    __FIIterable_1_Server__CAlienFXDeviceSetupInfo : public IInspectable
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE First( 
            /* [retval][out] */ __FIIterator_1_Server__CAlienFXDeviceSetupInfo **first) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct __FIIterable_1_Server__CAlienFXDeviceSetupInfoVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __FIIterable_1_Server__CAlienFXDeviceSetupInfo * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __FIIterable_1_Server__CAlienFXDeviceSetupInfo * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __FIIterable_1_Server__CAlienFXDeviceSetupInfo * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __FIIterable_1_Server__CAlienFXDeviceSetupInfo * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __FIIterable_1_Server__CAlienFXDeviceSetupInfo * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __FIIterable_1_Server__CAlienFXDeviceSetupInfo * This,
            /* [out] */ TrustLevel *trustLevel);
        
        HRESULT ( STDMETHODCALLTYPE *First )( 
            __FIIterable_1_Server__CAlienFXDeviceSetupInfo * This,
            /* [retval][out] */ __FIIterator_1_Server__CAlienFXDeviceSetupInfo **first);
        
        END_INTERFACE
    } __FIIterable_1_Server__CAlienFXDeviceSetupInfoVtbl;

    interface __FIIterable_1_Server__CAlienFXDeviceSetupInfo
    {
        CONST_VTBL struct __FIIterable_1_Server__CAlienFXDeviceSetupInfoVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __FIIterable_1_Server__CAlienFXDeviceSetupInfo_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __FIIterable_1_Server__CAlienFXDeviceSetupInfo_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __FIIterable_1_Server__CAlienFXDeviceSetupInfo_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __FIIterable_1_Server__CAlienFXDeviceSetupInfo_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __FIIterable_1_Server__CAlienFXDeviceSetupInfo_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __FIIterable_1_Server__CAlienFXDeviceSetupInfo_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __FIIterable_1_Server__CAlienFXDeviceSetupInfo_First(This,first)	\
    ( (This)->lpVtbl -> First(This,first) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____FIIterable_1_Server__CAlienFXDeviceSetupInfo_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0006 */
/* [local] */ 

#endif /* pinterface */
#endif /* DEF___FIIterable_1_Server__CAlienFXDeviceSetupInfo */
#if !defined(__cplusplus)
struct __x_Server_CAlienFXDeviceSetupInfo
    {
    HSTRING VendorId;
    HSTRING ProductId;
    boolean IsPresent;
    boolean IsInstalled;
    } ;
#endif
#if !defined(____x_Server_CDeviceSetupInfoReader_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_DeviceSetupInfoReader[] = L"Server.DeviceSetupInfoReader";
#endif /* !defined(____x_Server_CDeviceSetupInfoReader_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0006 */
/* [local] */ 

#ifdef __cplusplus
} /* end extern "C" */
namespace Server {
    
    struct AlienFXDeviceSetupInfo
        {
        HSTRING VendorId;
        HSTRING ProductId;
        boolean IsPresent;
        boolean IsInstalled;
        } ;
} /* end namespace */

extern "C" { 
#endif



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0006_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0006_v0_0_s_ifspec;

#ifndef ____x_Server_CDeviceSetupInfoReader_INTERFACE_DEFINED__
#define ____x_Server_CDeviceSetupInfoReader_INTERFACE_DEFINED__

/* interface __x_Server_CDeviceSetupInfoReader */
/* [uuid][object] */ 



/* interface Server::DeviceSetupInfoReader */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CDeviceSetupInfoReader;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("5621CEE1-BEF6-564A-6D54-A9395C9B5E14")
        DeviceSetupInfoReader : public IInspectable
        {
        public:
            virtual HRESULT STDMETHODCALLTYPE Find( 
                /* [out][retval] */ __FIIterable_1_Server__CAlienFXDeviceSetupInfo **value) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_DeviceSetupInfoReader = __uuidof(DeviceSetupInfoReader);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CDeviceSetupInfoReaderVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CDeviceSetupInfoReader * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CDeviceSetupInfoReader * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CDeviceSetupInfoReader * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CDeviceSetupInfoReader * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CDeviceSetupInfoReader * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CDeviceSetupInfoReader * This,
            /* [out] */ TrustLevel *trustLevel);
        
        HRESULT ( STDMETHODCALLTYPE *Find )( 
            __x_Server_CDeviceSetupInfoReader * This,
            /* [out][retval] */ __FIIterable_1_Server__CAlienFXDeviceSetupInfo **value);
        
        END_INTERFACE
    } __x_Server_CDeviceSetupInfoReaderVtbl;

    interface __x_Server_CDeviceSetupInfoReader
    {
        CONST_VTBL struct __x_Server_CDeviceSetupInfoReaderVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CDeviceSetupInfoReader_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CDeviceSetupInfoReader_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CDeviceSetupInfoReader_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CDeviceSetupInfoReader_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CDeviceSetupInfoReader_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CDeviceSetupInfoReader_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CDeviceSetupInfoReader_Find(This,value)	\
    ( (This)->lpVtbl -> Find(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CDeviceSetupInfoReader_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0007 */
/* [local] */ 

#if !defined(____x_Server_CDeviceSetupInfoWriter_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_DeviceSetupInfoWriter[] = L"Server.DeviceSetupInfoWriter";
#endif /* !defined(____x_Server_CDeviceSetupInfoWriter_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0007 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0007_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0007_v0_0_s_ifspec;

#ifndef ____x_Server_CDeviceSetupInfoWriter_INTERFACE_DEFINED__
#define ____x_Server_CDeviceSetupInfoWriter_INTERFACE_DEFINED__

/* interface __x_Server_CDeviceSetupInfoWriter */
/* [uuid][object] */ 



/* interface Server::DeviceSetupInfoWriter */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CDeviceSetupInfoWriter;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("AA547604-E14D-51B8-5615-9605178E411D")
        DeviceSetupInfoWriter : public IInspectable
        {
        public:
            virtual HRESULT STDMETHODCALLTYPE Write( 
                /* [in] */ HSTRING vendorId,
                /* [in] */ HSTRING productId) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_DeviceSetupInfoWriter = __uuidof(DeviceSetupInfoWriter);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CDeviceSetupInfoWriterVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CDeviceSetupInfoWriter * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CDeviceSetupInfoWriter * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CDeviceSetupInfoWriter * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CDeviceSetupInfoWriter * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CDeviceSetupInfoWriter * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CDeviceSetupInfoWriter * This,
            /* [out] */ TrustLevel *trustLevel);
        
        HRESULT ( STDMETHODCALLTYPE *Write )( 
            __x_Server_CDeviceSetupInfoWriter * This,
            /* [in] */ HSTRING vendorId,
            /* [in] */ HSTRING productId);
        
        END_INTERFACE
    } __x_Server_CDeviceSetupInfoWriterVtbl;

    interface __x_Server_CDeviceSetupInfoWriter
    {
        CONST_VTBL struct __x_Server_CDeviceSetupInfoWriterVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CDeviceSetupInfoWriter_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CDeviceSetupInfoWriter_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CDeviceSetupInfoWriter_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CDeviceSetupInfoWriter_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CDeviceSetupInfoWriter_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CDeviceSetupInfoWriter_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CDeviceSetupInfoWriter_Write(This,vendorId,productId)	\
    ( (This)->lpVtbl -> Write(This,vendorId,productId) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CDeviceSetupInfoWriter_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0008 */
/* [local] */ 

#if !defined(____x_Server_CIAFXSetupInfoHelperClass_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IAFXSetupInfoHelperClass[] = L"Server.IAFXSetupInfoHelperClass";
#endif /* !defined(____x_Server_CIAFXSetupInfoHelperClass_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0008 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0008_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0008_v0_0_s_ifspec;

#ifndef ____x_Server_CIAFXSetupInfoHelperClass_INTERFACE_DEFINED__
#define ____x_Server_CIAFXSetupInfoHelperClass_INTERFACE_DEFINED__

/* interface __x_Server_CIAFXSetupInfoHelperClass */
/* [uuid][object] */ 



/* interface Server::IAFXSetupInfoHelperClass */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIAFXSetupInfoHelperClass;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("EC22E168-FD0C-52E2-7746-90C9259C9887")
        IAFXSetupInfoHelperClass : public IInspectable
        {
        public:
        };

        extern const __declspec(selectany) IID & IID_IAFXSetupInfoHelperClass = __uuidof(IAFXSetupInfoHelperClass);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIAFXSetupInfoHelperClassVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIAFXSetupInfoHelperClass * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIAFXSetupInfoHelperClass * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIAFXSetupInfoHelperClass * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIAFXSetupInfoHelperClass * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIAFXSetupInfoHelperClass * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIAFXSetupInfoHelperClass * This,
            /* [out] */ TrustLevel *trustLevel);
        
        END_INTERFACE
    } __x_Server_CIAFXSetupInfoHelperClassVtbl;

    interface __x_Server_CIAFXSetupInfoHelperClass
    {
        CONST_VTBL struct __x_Server_CIAFXSetupInfoHelperClassVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIAFXSetupInfoHelperClass_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIAFXSetupInfoHelperClass_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIAFXSetupInfoHelperClass_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIAFXSetupInfoHelperClass_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIAFXSetupInfoHelperClass_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIAFXSetupInfoHelperClass_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIAFXSetupInfoHelperClass_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0009 */
/* [local] */ 

#if !defined(____x_Server_CIAFXSetupInfoHelperStatic_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IAFXSetupInfoHelperStatic[] = L"Server.IAFXSetupInfoHelperStatic";
#endif /* !defined(____x_Server_CIAFXSetupInfoHelperStatic_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0009 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0009_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0009_v0_0_s_ifspec;

#ifndef ____x_Server_CIAFXSetupInfoHelperStatic_INTERFACE_DEFINED__
#define ____x_Server_CIAFXSetupInfoHelperStatic_INTERFACE_DEFINED__

/* interface __x_Server_CIAFXSetupInfoHelperStatic */
/* [uuid][object] */ 



/* interface Server::IAFXSetupInfoHelperStatic */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIAFXSetupInfoHelperStatic;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("AEAFE193-06F6-5217-7E9F-6374F2D11961")
        IAFXSetupInfoHelperStatic : public IInspectable
        {
        public:
            virtual HRESULT STDMETHODCALLTYPE AreEqual( 
                /* [in] */ Server::AlienFXDeviceSetupInfo device1,
                /* [in] */ Server::AlienFXDeviceSetupInfo device2,
                /* [out][retval] */ boolean *value) = 0;
            
            virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_Empty( 
                /* [out][retval] */ Server::AlienFXDeviceSetupInfo *value) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_IAFXSetupInfoHelperStatic = __uuidof(IAFXSetupInfoHelperStatic);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIAFXSetupInfoHelperStaticVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIAFXSetupInfoHelperStatic * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIAFXSetupInfoHelperStatic * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIAFXSetupInfoHelperStatic * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIAFXSetupInfoHelperStatic * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIAFXSetupInfoHelperStatic * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIAFXSetupInfoHelperStatic * This,
            /* [out] */ TrustLevel *trustLevel);
        
        HRESULT ( STDMETHODCALLTYPE *AreEqual )( 
            __x_Server_CIAFXSetupInfoHelperStatic * This,
            /* [in] */ __x_Server_CAlienFXDeviceSetupInfo device1,
            /* [in] */ __x_Server_CAlienFXDeviceSetupInfo device2,
            /* [out][retval] */ boolean *value);
        
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_Empty )( 
            __x_Server_CIAFXSetupInfoHelperStatic * This,
            /* [out][retval] */ __x_Server_CAlienFXDeviceSetupInfo *value);
        
        END_INTERFACE
    } __x_Server_CIAFXSetupInfoHelperStaticVtbl;

    interface __x_Server_CIAFXSetupInfoHelperStatic
    {
        CONST_VTBL struct __x_Server_CIAFXSetupInfoHelperStaticVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIAFXSetupInfoHelperStatic_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIAFXSetupInfoHelperStatic_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIAFXSetupInfoHelperStatic_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIAFXSetupInfoHelperStatic_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIAFXSetupInfoHelperStatic_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIAFXSetupInfoHelperStatic_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CIAFXSetupInfoHelperStatic_AreEqual(This,device1,device2,value)	\
    ( (This)->lpVtbl -> AreEqual(This,device1,device2,value) ) 

#define __x_Server_CIAFXSetupInfoHelperStatic_get_Empty(This,value)	\
    ( (This)->lpVtbl -> get_Empty(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIAFXSetupInfoHelperStatic_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0010 */
/* [local] */ 

#if !defined(____x_Server_CIAlienFXComponentClass_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IAlienFXComponentClass[] = L"Server.IAlienFXComponentClass";
#endif /* !defined(____x_Server_CIAlienFXComponentClass_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0010 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0010_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0010_v0_0_s_ifspec;

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
            virtual HRESULT STDMETHODCALLTYPE DiscoveDevices( 
                /* [out][retval] */ Server::AlienFXDeviceSetupInfo *value) = 0;
            
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
        
        HRESULT ( STDMETHODCALLTYPE *DiscoveDevices )( 
            __x_Server_CIAlienFXComponentClass * This,
            /* [out][retval] */ __x_Server_CAlienFXDeviceSetupInfo *value);
        
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


#define __x_Server_CIAlienFXComponentClass_DiscoveDevices(This,value)	\
    ( (This)->lpVtbl -> DiscoveDevices(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIAlienFXComponentClass_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0011 */
/* [local] */ 

#if !defined(____x_Server_CIAlienFXDeviceDiscoveryService_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IAlienFXDeviceDiscoveryService[] = L"Server.IAlienFXDeviceDiscoveryService";
#endif /* !defined(____x_Server_CIAlienFXDeviceDiscoveryService_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0011 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0011_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0011_v0_0_s_ifspec;

#ifndef ____x_Server_CIAlienFXDeviceDiscoveryService_INTERFACE_DEFINED__
#define ____x_Server_CIAlienFXDeviceDiscoveryService_INTERFACE_DEFINED__

/* interface __x_Server_CIAlienFXDeviceDiscoveryService */
/* [uuid][object] */ 



/* interface Server::IAlienFXDeviceDiscoveryService */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIAlienFXDeviceDiscoveryService;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("032F70B3-26CF-5DDE-55A5-8AD8E6BF9B3B")
        IAlienFXDeviceDiscoveryService : public IInspectable
        {
        public:
            virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_RegistryDeviceSetupInfoReader( 
                /* [out][retval] */ Server::DeviceSetupInfoReader **value) = 0;
            
            virtual /* [propput] */ HRESULT STDMETHODCALLTYPE put_RegistryDeviceSetupInfoReader( 
                /* [in] */ Server::DeviceSetupInfoReader *value) = 0;
            
            virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_RegistryDeviceSetupInfoWriter( 
                /* [out][retval] */ Server::DeviceSetupInfoWriter **value) = 0;
            
            virtual /* [propput] */ HRESULT STDMETHODCALLTYPE put_RegistryDeviceSetupInfoWriter( 
                /* [in] */ Server::DeviceSetupInfoWriter *value) = 0;
            
            virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_ModelProvider( 
                /* [out][retval] */ Server::ModelProvider **value) = 0;
            
            virtual /* [propput] */ HRESULT STDMETHODCALLTYPE put_ModelProvider( 
                /* [in] */ Server::ModelProvider *value) = 0;
            
            virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_AllDevices( 
                /* [out][retval] */ __FIIterable_1_Server__CAlienFXDeviceSetupInfo **value) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE DiscoverDevices( 
                /* [out][retval] */ __FIIterable_1_Server__CAlienFXDeviceSetupInfo **value) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_IAlienFXDeviceDiscoveryService = __uuidof(IAlienFXDeviceDiscoveryService);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIAlienFXDeviceDiscoveryServiceVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIAlienFXDeviceDiscoveryService * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIAlienFXDeviceDiscoveryService * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIAlienFXDeviceDiscoveryService * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIAlienFXDeviceDiscoveryService * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIAlienFXDeviceDiscoveryService * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIAlienFXDeviceDiscoveryService * This,
            /* [out] */ TrustLevel *trustLevel);
        
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_RegistryDeviceSetupInfoReader )( 
            __x_Server_CIAlienFXDeviceDiscoveryService * This,
            /* [out][retval] */ __x_Server_CDeviceSetupInfoReader **value);
        
        /* [propput] */ HRESULT ( STDMETHODCALLTYPE *put_RegistryDeviceSetupInfoReader )( 
            __x_Server_CIAlienFXDeviceDiscoveryService * This,
            /* [in] */ __x_Server_CDeviceSetupInfoReader *value);
        
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_RegistryDeviceSetupInfoWriter )( 
            __x_Server_CIAlienFXDeviceDiscoveryService * This,
            /* [out][retval] */ __x_Server_CDeviceSetupInfoWriter **value);
        
        /* [propput] */ HRESULT ( STDMETHODCALLTYPE *put_RegistryDeviceSetupInfoWriter )( 
            __x_Server_CIAlienFXDeviceDiscoveryService * This,
            /* [in] */ __x_Server_CDeviceSetupInfoWriter *value);
        
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_ModelProvider )( 
            __x_Server_CIAlienFXDeviceDiscoveryService * This,
            /* [out][retval] */ __x_Server_CModelProvider **value);
        
        /* [propput] */ HRESULT ( STDMETHODCALLTYPE *put_ModelProvider )( 
            __x_Server_CIAlienFXDeviceDiscoveryService * This,
            /* [in] */ __x_Server_CModelProvider *value);
        
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_AllDevices )( 
            __x_Server_CIAlienFXDeviceDiscoveryService * This,
            /* [out][retval] */ __FIIterable_1_Server__CAlienFXDeviceSetupInfo **value);
        
        HRESULT ( STDMETHODCALLTYPE *DiscoverDevices )( 
            __x_Server_CIAlienFXDeviceDiscoveryService * This,
            /* [out][retval] */ __FIIterable_1_Server__CAlienFXDeviceSetupInfo **value);
        
        END_INTERFACE
    } __x_Server_CIAlienFXDeviceDiscoveryServiceVtbl;

    interface __x_Server_CIAlienFXDeviceDiscoveryService
    {
        CONST_VTBL struct __x_Server_CIAlienFXDeviceDiscoveryServiceVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIAlienFXDeviceDiscoveryService_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIAlienFXDeviceDiscoveryService_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIAlienFXDeviceDiscoveryService_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIAlienFXDeviceDiscoveryService_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIAlienFXDeviceDiscoveryService_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIAlienFXDeviceDiscoveryService_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CIAlienFXDeviceDiscoveryService_get_RegistryDeviceSetupInfoReader(This,value)	\
    ( (This)->lpVtbl -> get_RegistryDeviceSetupInfoReader(This,value) ) 

#define __x_Server_CIAlienFXDeviceDiscoveryService_put_RegistryDeviceSetupInfoReader(This,value)	\
    ( (This)->lpVtbl -> put_RegistryDeviceSetupInfoReader(This,value) ) 

#define __x_Server_CIAlienFXDeviceDiscoveryService_get_RegistryDeviceSetupInfoWriter(This,value)	\
    ( (This)->lpVtbl -> get_RegistryDeviceSetupInfoWriter(This,value) ) 

#define __x_Server_CIAlienFXDeviceDiscoveryService_put_RegistryDeviceSetupInfoWriter(This,value)	\
    ( (This)->lpVtbl -> put_RegistryDeviceSetupInfoWriter(This,value) ) 

#define __x_Server_CIAlienFXDeviceDiscoveryService_get_ModelProvider(This,value)	\
    ( (This)->lpVtbl -> get_ModelProvider(This,value) ) 

#define __x_Server_CIAlienFXDeviceDiscoveryService_put_ModelProvider(This,value)	\
    ( (This)->lpVtbl -> put_ModelProvider(This,value) ) 

#define __x_Server_CIAlienFXDeviceDiscoveryService_get_AllDevices(This,value)	\
    ( (This)->lpVtbl -> get_AllDevices(This,value) ) 

#define __x_Server_CIAlienFXDeviceDiscoveryService_DiscoverDevices(This,value)	\
    ( (This)->lpVtbl -> DiscoverDevices(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIAlienFXDeviceDiscoveryService_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0012 */
/* [local] */ 

#if !defined(____x_Server_CIAlienFXDeviceSetupInfoFactoryClass_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IAlienFXDeviceSetupInfoFactoryClass[] = L"Server.IAlienFXDeviceSetupInfoFactoryClass";
#endif /* !defined(____x_Server_CIAlienFXDeviceSetupInfoFactoryClass_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0012 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0012_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0012_v0_0_s_ifspec;

#ifndef ____x_Server_CIAlienFXDeviceSetupInfoFactoryClass_INTERFACE_DEFINED__
#define ____x_Server_CIAlienFXDeviceSetupInfoFactoryClass_INTERFACE_DEFINED__

/* interface __x_Server_CIAlienFXDeviceSetupInfoFactoryClass */
/* [uuid][object] */ 



/* interface Server::IAlienFXDeviceSetupInfoFactoryClass */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIAlienFXDeviceSetupInfoFactoryClass;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("A336E405-3C46-50A0-7178-E3B7701C6603")
        IAlienFXDeviceSetupInfoFactoryClass : public IInspectable
        {
        public:
        };

        extern const __declspec(selectany) IID & IID_IAlienFXDeviceSetupInfoFactoryClass = __uuidof(IAlienFXDeviceSetupInfoFactoryClass);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIAlienFXDeviceSetupInfoFactoryClassVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIAlienFXDeviceSetupInfoFactoryClass * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIAlienFXDeviceSetupInfoFactoryClass * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIAlienFXDeviceSetupInfoFactoryClass * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIAlienFXDeviceSetupInfoFactoryClass * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIAlienFXDeviceSetupInfoFactoryClass * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIAlienFXDeviceSetupInfoFactoryClass * This,
            /* [out] */ TrustLevel *trustLevel);
        
        END_INTERFACE
    } __x_Server_CIAlienFXDeviceSetupInfoFactoryClassVtbl;

    interface __x_Server_CIAlienFXDeviceSetupInfoFactoryClass
    {
        CONST_VTBL struct __x_Server_CIAlienFXDeviceSetupInfoFactoryClassVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIAlienFXDeviceSetupInfoFactoryClass_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIAlienFXDeviceSetupInfoFactoryClass_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIAlienFXDeviceSetupInfoFactoryClass_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIAlienFXDeviceSetupInfoFactoryClass_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIAlienFXDeviceSetupInfoFactoryClass_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIAlienFXDeviceSetupInfoFactoryClass_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIAlienFXDeviceSetupInfoFactoryClass_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0013 */
/* [local] */ 

#if !defined(____x_Server_CIAlienFXDeviceSetupInfoFactoryStatic_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IAlienFXDeviceSetupInfoFactoryStatic[] = L"Server.IAlienFXDeviceSetupInfoFactoryStatic";
#endif /* !defined(____x_Server_CIAlienFXDeviceSetupInfoFactoryStatic_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0013 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0013_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0013_v0_0_s_ifspec;

#ifndef ____x_Server_CIAlienFXDeviceSetupInfoFactoryStatic_INTERFACE_DEFINED__
#define ____x_Server_CIAlienFXDeviceSetupInfoFactoryStatic_INTERFACE_DEFINED__

/* interface __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic */
/* [uuid][object] */ 



/* interface Server::IAlienFXDeviceSetupInfoFactoryStatic */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIAlienFXDeviceSetupInfoFactoryStatic;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("BAD53A97-CEF1-5028-60BB-B99AE8C94CD5")
        IAlienFXDeviceSetupInfoFactoryStatic : public IInspectable
        {
        public:
            virtual HRESULT STDMETHODCALLTYPE NewAlienFXDeviceSetupInfo( 
                /* [in] */ HSTRING vId,
                /* [in] */ HSTRING pID,
                /* [out][retval] */ Server::AlienFXDeviceSetupInfo *value) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_IAlienFXDeviceSetupInfoFactoryStatic = __uuidof(IAlienFXDeviceSetupInfoFactoryStatic);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIAlienFXDeviceSetupInfoFactoryStaticVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic * This,
            /* [out] */ TrustLevel *trustLevel);
        
        HRESULT ( STDMETHODCALLTYPE *NewAlienFXDeviceSetupInfo )( 
            __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic * This,
            /* [in] */ HSTRING vId,
            /* [in] */ HSTRING pID,
            /* [out][retval] */ __x_Server_CAlienFXDeviceSetupInfo *value);
        
        END_INTERFACE
    } __x_Server_CIAlienFXDeviceSetupInfoFactoryStaticVtbl;

    interface __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic
    {
        CONST_VTBL struct __x_Server_CIAlienFXDeviceSetupInfoFactoryStaticVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CIAlienFXDeviceSetupInfoFactoryStatic_NewAlienFXDeviceSetupInfo(This,vId,pID,value)	\
    ( (This)->lpVtbl -> NewAlienFXDeviceSetupInfo(This,vId,pID,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIAlienFXDeviceSetupInfoFactoryStatic_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0014 */
/* [local] */ 

#if !defined(____x_Server_CIHostProcessManagerClass_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IHostProcessManagerClass[] = L"Server.IHostProcessManagerClass";
#endif /* !defined(____x_Server_CIHostProcessManagerClass_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0014 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0014_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0014_v0_0_s_ifspec;

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


/* interface __MIDL_itf_Server_0000_0015 */
/* [local] */ 

#if !defined(____x_Server_CIModelProviderClassClass_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IModelProviderClassClass[] = L"Server.IModelProviderClassClass";
#endif /* !defined(____x_Server_CIModelProviderClassClass_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0015 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0015_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0015_v0_0_s_ifspec;

#ifndef ____x_Server_CIModelProviderClassClass_INTERFACE_DEFINED__
#define ____x_Server_CIModelProviderClassClass_INTERFACE_DEFINED__

/* interface __x_Server_CIModelProviderClassClass */
/* [uuid][object] */ 



/* interface Server::IModelProviderClassClass */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIModelProviderClassClass;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("38BC5DC3-16B5-51AC-6977-0F01D3F88160")
        IModelProviderClassClass : public IInspectable
        {
        public:
            virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_RegistryModelReader( 
                /* [out][retval] */ Server::ModelReader **value) = 0;
            
            virtual /* [propput] */ HRESULT STDMETHODCALLTYPE put_RegistryModelReader( 
                /* [in] */ Server::ModelReader *value) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_IModelProviderClassClass = __uuidof(IModelProviderClassClass);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIModelProviderClassClassVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIModelProviderClassClass * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIModelProviderClassClass * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIModelProviderClassClass * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIModelProviderClassClass * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIModelProviderClassClass * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIModelProviderClassClass * This,
            /* [out] */ TrustLevel *trustLevel);
        
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_RegistryModelReader )( 
            __x_Server_CIModelProviderClassClass * This,
            /* [out][retval] */ __x_Server_CModelReader **value);
        
        /* [propput] */ HRESULT ( STDMETHODCALLTYPE *put_RegistryModelReader )( 
            __x_Server_CIModelProviderClassClass * This,
            /* [in] */ __x_Server_CModelReader *value);
        
        END_INTERFACE
    } __x_Server_CIModelProviderClassClassVtbl;

    interface __x_Server_CIModelProviderClassClass
    {
        CONST_VTBL struct __x_Server_CIModelProviderClassClassVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIModelProviderClassClass_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIModelProviderClassClass_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIModelProviderClassClass_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIModelProviderClassClass_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIModelProviderClassClass_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIModelProviderClassClass_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CIModelProviderClassClass_get_RegistryModelReader(This,value)	\
    ( (This)->lpVtbl -> get_RegistryModelReader(This,value) ) 

#define __x_Server_CIModelProviderClassClass_put_RegistryModelReader(This,value)	\
    ( (This)->lpVtbl -> put_RegistryModelReader(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIModelProviderClassClass_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0016 */
/* [local] */ 

#if !defined(____x_Server_CIObjectFactoryClass_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IObjectFactoryClass[] = L"Server.IObjectFactoryClass";
#endif /* !defined(____x_Server_CIObjectFactoryClass_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0016 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0016_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0016_v0_0_s_ifspec;

#ifndef ____x_Server_CIObjectFactoryClass_INTERFACE_DEFINED__
#define ____x_Server_CIObjectFactoryClass_INTERFACE_DEFINED__

/* interface __x_Server_CIObjectFactoryClass */
/* [uuid][object] */ 



/* interface Server::IObjectFactoryClass */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIObjectFactoryClass;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("5842DEE6-157C-590D-79D9-F29BB439ECD0")
        IObjectFactoryClass : public IInspectable
        {
        public:
            virtual HRESULT STDMETHODCALLTYPE NewAlienFXDeviceDiscovery( 
                /* [out][retval] */ Server::IAlienFXDeviceDiscoveryService **value) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_IObjectFactoryClass = __uuidof(IObjectFactoryClass);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIObjectFactoryClassVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIObjectFactoryClass * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIObjectFactoryClass * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIObjectFactoryClass * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIObjectFactoryClass * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIObjectFactoryClass * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIObjectFactoryClass * This,
            /* [out] */ TrustLevel *trustLevel);
        
        HRESULT ( STDMETHODCALLTYPE *NewAlienFXDeviceDiscovery )( 
            __x_Server_CIObjectFactoryClass * This,
            /* [out][retval] */ __x_Server_CIAlienFXDeviceDiscoveryService **value);
        
        END_INTERFACE
    } __x_Server_CIObjectFactoryClassVtbl;

    interface __x_Server_CIObjectFactoryClass
    {
        CONST_VTBL struct __x_Server_CIObjectFactoryClassVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIObjectFactoryClass_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIObjectFactoryClass_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIObjectFactoryClass_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIObjectFactoryClass_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIObjectFactoryClass_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIObjectFactoryClass_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CIObjectFactoryClass_NewAlienFXDeviceDiscovery(This,value)	\
    ( (This)->lpVtbl -> NewAlienFXDeviceDiscovery(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIObjectFactoryClass_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0017 */
/* [local] */ 

#if !defined(____x_Server_CIRegistryDeviceSetupInfoReaderClass_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IRegistryDeviceSetupInfoReaderClass[] = L"Server.IRegistryDeviceSetupInfoReaderClass";
#endif /* !defined(____x_Server_CIRegistryDeviceSetupInfoReaderClass_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0017 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0017_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0017_v0_0_s_ifspec;

#ifndef ____x_Server_CIRegistryDeviceSetupInfoReaderClass_INTERFACE_DEFINED__
#define ____x_Server_CIRegistryDeviceSetupInfoReaderClass_INTERFACE_DEFINED__

/* interface __x_Server_CIRegistryDeviceSetupInfoReaderClass */
/* [uuid][object] */ 



/* interface Server::IRegistryDeviceSetupInfoReaderClass */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIRegistryDeviceSetupInfoReaderClass;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("C550ECB9-903B-5EF4-5EE6-607596B081F0")
        IRegistryDeviceSetupInfoReaderClass : public IInspectable
        {
        public:
            virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_RegistryAPI( 
                /* [out][retval] */ Server::RegistryService **value) = 0;
            
            virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_RegistryEntries( 
                /* [out] */ UINT32 *__valueSize,
                /* [out][retval][size_is][size_is] */ HSTRING **value) = 0;
            
            virtual /* [propput] */ HRESULT STDMETHODCALLTYPE put_RegistryAPI( 
                /* [in] */ Server::RegistryService *value) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_IRegistryDeviceSetupInfoReaderClass = __uuidof(IRegistryDeviceSetupInfoReaderClass);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIRegistryDeviceSetupInfoReaderClassVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIRegistryDeviceSetupInfoReaderClass * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIRegistryDeviceSetupInfoReaderClass * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIRegistryDeviceSetupInfoReaderClass * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIRegistryDeviceSetupInfoReaderClass * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIRegistryDeviceSetupInfoReaderClass * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIRegistryDeviceSetupInfoReaderClass * This,
            /* [out] */ TrustLevel *trustLevel);
        
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_RegistryAPI )( 
            __x_Server_CIRegistryDeviceSetupInfoReaderClass * This,
            /* [out][retval] */ __x_Server_CRegistryService **value);
        
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_RegistryEntries )( 
            __x_Server_CIRegistryDeviceSetupInfoReaderClass * This,
            /* [out] */ UINT32 *__valueSize,
            /* [out][retval][size_is][size_is] */ HSTRING **value);
        
        /* [propput] */ HRESULT ( STDMETHODCALLTYPE *put_RegistryAPI )( 
            __x_Server_CIRegistryDeviceSetupInfoReaderClass * This,
            /* [in] */ __x_Server_CRegistryService *value);
        
        END_INTERFACE
    } __x_Server_CIRegistryDeviceSetupInfoReaderClassVtbl;

    interface __x_Server_CIRegistryDeviceSetupInfoReaderClass
    {
        CONST_VTBL struct __x_Server_CIRegistryDeviceSetupInfoReaderClassVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIRegistryDeviceSetupInfoReaderClass_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIRegistryDeviceSetupInfoReaderClass_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIRegistryDeviceSetupInfoReaderClass_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIRegistryDeviceSetupInfoReaderClass_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIRegistryDeviceSetupInfoReaderClass_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIRegistryDeviceSetupInfoReaderClass_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CIRegistryDeviceSetupInfoReaderClass_get_RegistryAPI(This,value)	\
    ( (This)->lpVtbl -> get_RegistryAPI(This,value) ) 

#define __x_Server_CIRegistryDeviceSetupInfoReaderClass_get_RegistryEntries(This,__valueSize,value)	\
    ( (This)->lpVtbl -> get_RegistryEntries(This,__valueSize,value) ) 

#define __x_Server_CIRegistryDeviceSetupInfoReaderClass_put_RegistryAPI(This,value)	\
    ( (This)->lpVtbl -> put_RegistryAPI(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIRegistryDeviceSetupInfoReaderClass_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0018 */
/* [local] */ 

#if !defined(____x_Server_CIRegistryDeviceSetupInfoWriterClass_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IRegistryDeviceSetupInfoWriterClass[] = L"Server.IRegistryDeviceSetupInfoWriterClass";
#endif /* !defined(____x_Server_CIRegistryDeviceSetupInfoWriterClass_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0018 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0018_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0018_v0_0_s_ifspec;

#ifndef ____x_Server_CIRegistryDeviceSetupInfoWriterClass_INTERFACE_DEFINED__
#define ____x_Server_CIRegistryDeviceSetupInfoWriterClass_INTERFACE_DEFINED__

/* interface __x_Server_CIRegistryDeviceSetupInfoWriterClass */
/* [uuid][object] */ 



/* interface Server::IRegistryDeviceSetupInfoWriterClass */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIRegistryDeviceSetupInfoWriterClass;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("738EFEC5-CAD7-58A4-4346-1188176C78FF")
        IRegistryDeviceSetupInfoWriterClass : public IInspectable
        {
        public:
            virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_RegistryAPI( 
                /* [out][retval] */ Server::RegistryService **value) = 0;
            
            virtual /* [propput] */ HRESULT STDMETHODCALLTYPE put_RegistryAPI( 
                /* [in] */ Server::RegistryService *value) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_IRegistryDeviceSetupInfoWriterClass = __uuidof(IRegistryDeviceSetupInfoWriterClass);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIRegistryDeviceSetupInfoWriterClassVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIRegistryDeviceSetupInfoWriterClass * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIRegistryDeviceSetupInfoWriterClass * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIRegistryDeviceSetupInfoWriterClass * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIRegistryDeviceSetupInfoWriterClass * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIRegistryDeviceSetupInfoWriterClass * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIRegistryDeviceSetupInfoWriterClass * This,
            /* [out] */ TrustLevel *trustLevel);
        
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_RegistryAPI )( 
            __x_Server_CIRegistryDeviceSetupInfoWriterClass * This,
            /* [out][retval] */ __x_Server_CRegistryService **value);
        
        /* [propput] */ HRESULT ( STDMETHODCALLTYPE *put_RegistryAPI )( 
            __x_Server_CIRegistryDeviceSetupInfoWriterClass * This,
            /* [in] */ __x_Server_CRegistryService *value);
        
        END_INTERFACE
    } __x_Server_CIRegistryDeviceSetupInfoWriterClassVtbl;

    interface __x_Server_CIRegistryDeviceSetupInfoWriterClass
    {
        CONST_VTBL struct __x_Server_CIRegistryDeviceSetupInfoWriterClassVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIRegistryDeviceSetupInfoWriterClass_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIRegistryDeviceSetupInfoWriterClass_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIRegistryDeviceSetupInfoWriterClass_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIRegistryDeviceSetupInfoWriterClass_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIRegistryDeviceSetupInfoWriterClass_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIRegistryDeviceSetupInfoWriterClass_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CIRegistryDeviceSetupInfoWriterClass_get_RegistryAPI(This,value)	\
    ( (This)->lpVtbl -> get_RegistryAPI(This,value) ) 

#define __x_Server_CIRegistryDeviceSetupInfoWriterClass_put_RegistryAPI(This,value)	\
    ( (This)->lpVtbl -> put_RegistryAPI(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIRegistryDeviceSetupInfoWriterClass_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0019 */
/* [local] */ 

#if !defined(____x_Server_CIRegistryServiceClassClass_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IRegistryServiceClassClass[] = L"Server.IRegistryServiceClassClass";
#endif /* !defined(____x_Server_CIRegistryServiceClassClass_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0019 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0019_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0019_v0_0_s_ifspec;

#ifndef ____x_Server_CIRegistryServiceClassClass_INTERFACE_DEFINED__
#define ____x_Server_CIRegistryServiceClassClass_INTERFACE_DEFINED__

/* interface __x_Server_CIRegistryServiceClassClass */
/* [uuid][object] */ 



/* interface Server::IRegistryServiceClassClass */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIRegistryServiceClassClass;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("A425C2F6-3F9D-5DC5-7685-91C898C76263")
        IRegistryServiceClassClass : public IInspectable
        {
        public:
            virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_FullPath( 
                /* [out][retval] */ HSTRING *value) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_IRegistryServiceClassClass = __uuidof(IRegistryServiceClassClass);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIRegistryServiceClassClassVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIRegistryServiceClassClass * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIRegistryServiceClassClass * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIRegistryServiceClassClass * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIRegistryServiceClassClass * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIRegistryServiceClassClass * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIRegistryServiceClassClass * This,
            /* [out] */ TrustLevel *trustLevel);
        
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_FullPath )( 
            __x_Server_CIRegistryServiceClassClass * This,
            /* [out][retval] */ HSTRING *value);
        
        END_INTERFACE
    } __x_Server_CIRegistryServiceClassClassVtbl;

    interface __x_Server_CIRegistryServiceClassClass
    {
        CONST_VTBL struct __x_Server_CIRegistryServiceClassClassVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIRegistryServiceClassClass_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIRegistryServiceClassClass_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIRegistryServiceClassClass_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIRegistryServiceClassClass_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIRegistryServiceClassClass_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIRegistryServiceClassClass_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CIRegistryServiceClassClass_get_FullPath(This,value)	\
    ( (This)->lpVtbl -> get_FullPath(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIRegistryServiceClassClass_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0020 */
/* [local] */ 

#if !defined(____x_Server_CIRegistryServiceClassFactory_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_IRegistryServiceClassFactory[] = L"Server.IRegistryServiceClassFactory";
#endif /* !defined(____x_Server_CIRegistryServiceClassFactory_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0020 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0020_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0020_v0_0_s_ifspec;

#ifndef ____x_Server_CIRegistryServiceClassFactory_INTERFACE_DEFINED__
#define ____x_Server_CIRegistryServiceClassFactory_INTERFACE_DEFINED__

/* interface __x_Server_CIRegistryServiceClassFactory */
/* [uuid][object] */ 



/* interface Server::IRegistryServiceClassFactory */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CIRegistryServiceClassFactory;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("0D3EBC6A-99BA-57A6-638C-23D51EAC68B1")
        IRegistryServiceClassFactory : public IInspectable
        {
        public:
            virtual HRESULT STDMETHODCALLTYPE CreateRegistryServiceClass( 
                /* [in] */ boolean force,
                /* [in] */ HSTRING rootPath,
                /* [in] */ HSTRING keyPath,
                /* [out][retval] */ Server::IRegistryServiceClassClass **value) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_IRegistryServiceClassFactory = __uuidof(IRegistryServiceClassFactory);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CIRegistryServiceClassFactoryVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CIRegistryServiceClassFactory * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CIRegistryServiceClassFactory * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CIRegistryServiceClassFactory * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CIRegistryServiceClassFactory * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CIRegistryServiceClassFactory * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CIRegistryServiceClassFactory * This,
            /* [out] */ TrustLevel *trustLevel);
        
        HRESULT ( STDMETHODCALLTYPE *CreateRegistryServiceClass )( 
            __x_Server_CIRegistryServiceClassFactory * This,
            /* [in] */ boolean force,
            /* [in] */ HSTRING rootPath,
            /* [in] */ HSTRING keyPath,
            /* [out][retval] */ __x_Server_CIRegistryServiceClassClass **value);
        
        END_INTERFACE
    } __x_Server_CIRegistryServiceClassFactoryVtbl;

    interface __x_Server_CIRegistryServiceClassFactory
    {
        CONST_VTBL struct __x_Server_CIRegistryServiceClassFactoryVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CIRegistryServiceClassFactory_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CIRegistryServiceClassFactory_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CIRegistryServiceClassFactory_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CIRegistryServiceClassFactory_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CIRegistryServiceClassFactory_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CIRegistryServiceClassFactory_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CIRegistryServiceClassFactory_CreateRegistryServiceClass(This,force,rootPath,keyPath,value)	\
    ( (This)->lpVtbl -> CreateRegistryServiceClass(This,force,rootPath,keyPath,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CIRegistryServiceClassFactory_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0021 */
/* [local] */ 

#if !defined(____x_Server_CModelProvider_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_ModelProvider[] = L"Server.ModelProvider";
#endif /* !defined(____x_Server_CModelProvider_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0021 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0021_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0021_v0_0_s_ifspec;

#ifndef ____x_Server_CModelProvider_INTERFACE_DEFINED__
#define ____x_Server_CModelProvider_INTERFACE_DEFINED__

/* interface __x_Server_CModelProvider */
/* [uuid][object] */ 



/* interface Server::ModelProvider */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CModelProvider;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("9A9C27B0-20CF-51CC-6ABC-9E33D663B0CE")
        ModelProvider : public IInspectable
        {
        public:
            virtual /* [propget] */ HRESULT STDMETHODCALLTYPE get_FromRegistry( 
                /* [out][retval] */ HSTRING *value) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_ModelProvider = __uuidof(ModelProvider);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CModelProviderVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CModelProvider * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CModelProvider * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CModelProvider * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CModelProvider * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CModelProvider * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CModelProvider * This,
            /* [out] */ TrustLevel *trustLevel);
        
        /* [propget] */ HRESULT ( STDMETHODCALLTYPE *get_FromRegistry )( 
            __x_Server_CModelProvider * This,
            /* [out][retval] */ HSTRING *value);
        
        END_INTERFACE
    } __x_Server_CModelProviderVtbl;

    interface __x_Server_CModelProvider
    {
        CONST_VTBL struct __x_Server_CModelProviderVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CModelProvider_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CModelProvider_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CModelProvider_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CModelProvider_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CModelProvider_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CModelProvider_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CModelProvider_get_FromRegistry(This,value)	\
    ( (This)->lpVtbl -> get_FromRegistry(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CModelProvider_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0022 */
/* [local] */ 

#if !defined(____x_Server_CModelReader_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_ModelReader[] = L"Server.ModelReader";
#endif /* !defined(____x_Server_CModelReader_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0022 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0022_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0022_v0_0_s_ifspec;

#ifndef ____x_Server_CModelReader_INTERFACE_DEFINED__
#define ____x_Server_CModelReader_INTERFACE_DEFINED__

/* interface __x_Server_CModelReader */
/* [uuid][object] */ 



/* interface Server::ModelReader */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CModelReader;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("A952C877-8CC1-5917-6434-D42EFA599080")
        ModelReader : public IInspectable
        {
        public:
            virtual HRESULT STDMETHODCALLTYPE Read( 
                /* [out][retval] */ HSTRING *value) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_ModelReader = __uuidof(ModelReader);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CModelReaderVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CModelReader * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CModelReader * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CModelReader * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CModelReader * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CModelReader * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CModelReader * This,
            /* [out] */ TrustLevel *trustLevel);
        
        HRESULT ( STDMETHODCALLTYPE *Read )( 
            __x_Server_CModelReader * This,
            /* [out][retval] */ HSTRING *value);
        
        END_INTERFACE
    } __x_Server_CModelReaderVtbl;

    interface __x_Server_CModelReader
    {
        CONST_VTBL struct __x_Server_CModelReaderVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CModelReader_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CModelReader_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CModelReader_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CModelReader_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CModelReader_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CModelReader_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CModelReader_Read(This,value)	\
    ( (This)->lpVtbl -> Read(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CModelReader_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0023 */
/* [local] */ 

#if !defined(____x_Server_CRegistryService_INTERFACE_DEFINED__)
extern const __declspec(selectany) _Null_terminated_ WCHAR InterfaceName_Server_RegistryService[] = L"Server.RegistryService";
#endif /* !defined(____x_Server_CRegistryService_INTERFACE_DEFINED__) */


/* interface __MIDL_itf_Server_0000_0023 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0023_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0023_v0_0_s_ifspec;

#ifndef ____x_Server_CRegistryService_INTERFACE_DEFINED__
#define ____x_Server_CRegistryService_INTERFACE_DEFINED__

/* interface __x_Server_CRegistryService */
/* [uuid][object] */ 



/* interface Server::RegistryService */
/* [uuid][object] */ 


EXTERN_C const IID IID___x_Server_CRegistryService;

#if defined(__cplusplus) && !defined(CINTERFACE)
    } /* end extern "C" */
    namespace Server {
        
        MIDL_INTERFACE("710EB651-E7D7-51F0-631F-D6FDE4EC354D")
        RegistryService : public IInspectable
        {
        public:
            virtual HRESULT STDMETHODCALLTYPE CreateRegistryValue( 
                /* [in] */ HSTRING valueName,
                /* [in] */ IInspectable *defaultValue,
                /* [out][retval] */ boolean *value) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE GetRegistryValue1( 
                /* [in] */ HSTRING keyName,
                /* [out][retval] */ IInspectable **value) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE GetRegistryValue2( 
                /* [in] */ HSTRING keyName,
                /* [in] */ IInspectable *defaultValue,
                /* [out][retval] */ IInspectable **value) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE SetRegistryValue( 
                /* [in] */ HSTRING valueName,
                /* [in] */ IInspectable *currentValue,
                /* [out][retval] */ boolean *value) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE GetSubKeyNames( 
                /* [out] */ UINT32 *__valueSize,
                /* [out][retval][size_is][size_is] */ HSTRING **value) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE DeleteSubKey( 
                /* [in] */ HSTRING subkey,
                /* [in] */ boolean throwOnMissingSubKey) = 0;
            
            virtual HRESULT STDMETHODCALLTYPE CreateSubKey( 
                /* [in] */ HSTRING subkey,
                /* [out][retval] */ boolean *value) = 0;
            
        };

        extern const __declspec(selectany) IID & IID_RegistryService = __uuidof(RegistryService);

        
    }  /* end namespace */
    extern "C" { 
    
#else 	/* C style interface */

    typedef struct __x_Server_CRegistryServiceVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            __x_Server_CRegistryService * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            __x_Server_CRegistryService * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            __x_Server_CRegistryService * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetIids )( 
            __x_Server_CRegistryService * This,
            /* [out] */ ULONG *iidCount,
            /* [size_is][size_is][out] */ IID **iids);
        
        HRESULT ( STDMETHODCALLTYPE *GetRuntimeClassName )( 
            __x_Server_CRegistryService * This,
            /* [out] */ HSTRING *className);
        
        HRESULT ( STDMETHODCALLTYPE *GetTrustLevel )( 
            __x_Server_CRegistryService * This,
            /* [out] */ TrustLevel *trustLevel);
        
        HRESULT ( STDMETHODCALLTYPE *CreateRegistryValue )( 
            __x_Server_CRegistryService * This,
            /* [in] */ HSTRING valueName,
            /* [in] */ IInspectable *defaultValue,
            /* [out][retval] */ boolean *value);
        
        HRESULT ( STDMETHODCALLTYPE *GetRegistryValue1 )( 
            __x_Server_CRegistryService * This,
            /* [in] */ HSTRING keyName,
            /* [out][retval] */ IInspectable **value);
        
        HRESULT ( STDMETHODCALLTYPE *GetRegistryValue2 )( 
            __x_Server_CRegistryService * This,
            /* [in] */ HSTRING keyName,
            /* [in] */ IInspectable *defaultValue,
            /* [out][retval] */ IInspectable **value);
        
        HRESULT ( STDMETHODCALLTYPE *SetRegistryValue )( 
            __x_Server_CRegistryService * This,
            /* [in] */ HSTRING valueName,
            /* [in] */ IInspectable *currentValue,
            /* [out][retval] */ boolean *value);
        
        HRESULT ( STDMETHODCALLTYPE *GetSubKeyNames )( 
            __x_Server_CRegistryService * This,
            /* [out] */ UINT32 *__valueSize,
            /* [out][retval][size_is][size_is] */ HSTRING **value);
        
        HRESULT ( STDMETHODCALLTYPE *DeleteSubKey )( 
            __x_Server_CRegistryService * This,
            /* [in] */ HSTRING subkey,
            /* [in] */ boolean throwOnMissingSubKey);
        
        HRESULT ( STDMETHODCALLTYPE *CreateSubKey )( 
            __x_Server_CRegistryService * This,
            /* [in] */ HSTRING subkey,
            /* [out][retval] */ boolean *value);
        
        END_INTERFACE
    } __x_Server_CRegistryServiceVtbl;

    interface __x_Server_CRegistryService
    {
        CONST_VTBL struct __x_Server_CRegistryServiceVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define __x_Server_CRegistryService_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define __x_Server_CRegistryService_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define __x_Server_CRegistryService_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define __x_Server_CRegistryService_GetIids(This,iidCount,iids)	\
    ( (This)->lpVtbl -> GetIids(This,iidCount,iids) ) 

#define __x_Server_CRegistryService_GetRuntimeClassName(This,className)	\
    ( (This)->lpVtbl -> GetRuntimeClassName(This,className) ) 

#define __x_Server_CRegistryService_GetTrustLevel(This,trustLevel)	\
    ( (This)->lpVtbl -> GetTrustLevel(This,trustLevel) ) 


#define __x_Server_CRegistryService_CreateRegistryValue(This,valueName,defaultValue,value)	\
    ( (This)->lpVtbl -> CreateRegistryValue(This,valueName,defaultValue,value) ) 

#define __x_Server_CRegistryService_GetRegistryValue1(This,keyName,value)	\
    ( (This)->lpVtbl -> GetRegistryValue1(This,keyName,value) ) 

#define __x_Server_CRegistryService_GetRegistryValue2(This,keyName,defaultValue,value)	\
    ( (This)->lpVtbl -> GetRegistryValue2(This,keyName,defaultValue,value) ) 

#define __x_Server_CRegistryService_SetRegistryValue(This,valueName,currentValue,value)	\
    ( (This)->lpVtbl -> SetRegistryValue(This,valueName,currentValue,value) ) 

#define __x_Server_CRegistryService_GetSubKeyNames(This,__valueSize,value)	\
    ( (This)->lpVtbl -> GetSubKeyNames(This,__valueSize,value) ) 

#define __x_Server_CRegistryService_DeleteSubKey(This,subkey,throwOnMissingSubKey)	\
    ( (This)->lpVtbl -> DeleteSubKey(This,subkey,throwOnMissingSubKey) ) 

#define __x_Server_CRegistryService_CreateSubKey(This,subkey,value)	\
    ( (This)->lpVtbl -> CreateSubKey(This,subkey,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ____x_Server_CRegistryService_INTERFACE_DEFINED__ */


/* interface __MIDL_itf_Server_0000_0024 */
/* [local] */ 

#ifndef RUNTIMECLASS_Server_AFXSetupInfoHelper_DEFINED
#define RUNTIMECLASS_Server_AFXSetupInfoHelper_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_AFXSetupInfoHelper[] = L"Server.AFXSetupInfoHelper";
#endif
#ifndef RUNTIMECLASS_Server_AlienFXComponent_DEFINED
#define RUNTIMECLASS_Server_AlienFXComponent_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_AlienFXComponent[] = L"Server.AlienFXComponent";
#endif
#ifndef RUNTIMECLASS_Server_AlienFXDeviceDiscoveryServiceClass_DEFINED
#define RUNTIMECLASS_Server_AlienFXDeviceDiscoveryServiceClass_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_AlienFXDeviceDiscoveryServiceClass[] = L"Server.AlienFXDeviceDiscoveryServiceClass";
#endif
#ifndef RUNTIMECLASS_Server_AlienFXDeviceSetupInfoFactory_DEFINED
#define RUNTIMECLASS_Server_AlienFXDeviceSetupInfoFactory_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_AlienFXDeviceSetupInfoFactory[] = L"Server.AlienFXDeviceSetupInfoFactory";
#endif
#ifndef RUNTIMECLASS_Server_HostProcessManager_DEFINED
#define RUNTIMECLASS_Server_HostProcessManager_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_HostProcessManager[] = L"Server.HostProcessManager";
#endif
#ifndef RUNTIMECLASS_Server_ModelProviderClass_DEFINED
#define RUNTIMECLASS_Server_ModelProviderClass_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_ModelProviderClass[] = L"Server.ModelProviderClass";
#endif
#ifndef RUNTIMECLASS_Server_ObjectFactory_DEFINED
#define RUNTIMECLASS_Server_ObjectFactory_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_ObjectFactory[] = L"Server.ObjectFactory";
#endif
#ifndef RUNTIMECLASS_Server_RegistryDeviceSetupInfoReader_DEFINED
#define RUNTIMECLASS_Server_RegistryDeviceSetupInfoReader_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_RegistryDeviceSetupInfoReader[] = L"Server.RegistryDeviceSetupInfoReader";
#endif
#ifndef RUNTIMECLASS_Server_RegistryDeviceSetupInfoWriter_DEFINED
#define RUNTIMECLASS_Server_RegistryDeviceSetupInfoWriter_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_RegistryDeviceSetupInfoWriter[] = L"Server.RegistryDeviceSetupInfoWriter";
#endif
#ifndef RUNTIMECLASS_Server_RegistryModelReader_DEFINED
#define RUNTIMECLASS_Server_RegistryModelReader_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_RegistryModelReader[] = L"Server.RegistryModelReader";
#endif
#ifndef RUNTIMECLASS_Server_RegistryServiceClass_DEFINED
#define RUNTIMECLASS_Server_RegistryServiceClass_DEFINED
extern const __declspec(selectany) _Null_terminated_ WCHAR RuntimeClass_Server_RegistryServiceClass[] = L"Server.RegistryServiceClass";
#endif


/* interface __MIDL_itf_Server_0000_0024 */
/* [local] */ 



extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0024_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Server_0000_0024_v0_0_s_ifspec;

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


