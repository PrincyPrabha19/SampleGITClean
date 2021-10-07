// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H

#ifndef STRICT
#define STRICT
#endif

// add headers that you want to pre-compile here
#include "framework.h"

#define _ATL_NO_AUTOMATIC_NAMESPACE

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS	// some CString constructors will be explicit

#define ATL_NO_ASSERT_ON_DESTROY_NONEXISTENT_WINDOW

#include <Windows.h>
#include <TlHelp32.h>
#include <UserEnv.h>
#include <Pathcch.h>
#include <Wtsapi32.h>
#include <Dbt.h>
#include <Setupapi.h>
#include <usbiodef.h>
#include <atlbase.h>
#include <atlcom.h>
#include <atlctl.h>
#include <atlcoll.h>
#include <atlstr.h>
#include <atlsafe.h>
#include <Strsafe.h>
#include <string>
#include <Ntsecapi.h>
namespace WindowsInternal
{
#include <Winternl.h>
}

#define DBG_LEVEL_TRACE		0x01
#define DBG_LEVEL_INFO		0x02
#define DBG_LEVEL_DEBUG		0x04
#define DBG_LEVEL_WARN		0x08
#define DBG_LEVEL_ERROR		0x10
#define DBG_LEVEL_FATAL		0x20
#define DBG_LEVEL_ALL		0xFF

#ifndef DBG_LEVEL_ENABLED
#define DBG_LEVEL_ENABLED   ( DBG_LEVEL_DEBUG | DBG_LEVEL_INFO | DBG_LEVEL_WARN | DBG_LEVEL_ERROR | DBG_LEVEL_FATAL )
#endif

#if _DEBUG


#define HiveMindServiceTrace(LEVEL,...)																	\
		{																										\
		if((DBG_LEVEL_ENABLED & LEVEL) == LEVEL)															\
				{																									\
			TCHAR __DEBUG_BUF[2048];																					\
			if (LEVEL == DBG_LEVEL_TRACE) _stprintf_s(__DEBUG_BUF,_T("HiveMindService!%S (TRACE): "), __FUNCTION__);				\
						else if(LEVEL == DBG_LEVEL_INFO) _stprintf_s(__DEBUG_BUF,_T("HiveMindService!%S (INFO): "),__FUNCTION__);			\
						else if(LEVEL == DBG_LEVEL_DEBUG) _stprintf_s(__DEBUG_BUF,_T("HiveMindService!%S (DEBUG): "),__FUNCTION__);		\
						else if(LEVEL == DBG_LEVEL_WARN) _stprintf_s(__DEBUG_BUF,_T("HiveMindService!%S (WARN): "),__FUNCTION__);			\
						else if(LEVEL == DBG_LEVEL_ERROR) _stprintf_s(__DEBUG_BUF,_T("HiveMindService!%S (ERROR): "),__FUNCTION__);		\
						else if(LEVEL == DBG_LEVEL_FATAL) _stprintf_s(__DEBUG_BUF,_T("HiveMindService!%S (FATAL): "),__FUNCTION__);		\
						else _stprintf_s(__DEBUG_BUF,_T("HiveMindService!%S (0x%x): "),__FUNCTION__,LEVEL);								\
			OutputDebugString(__DEBUG_BUF);																			\
			_stprintf_s(__DEBUG_BUF,__VA_ARGS__); 																			\
			OutputDebugString(__DEBUG_BUF);																			\
				}																									\
		}											  

#else

#define HiveMindServiceTrace(LEVEL,...) {} \

#endif

#pragma comment(lib, "Pathcch")
#pragma comment(lib, "Userenv")
#pragma comment(lib, "Wtsapi32")
#pragma comment(lib, "Setupapi")
#pragma comment(lib, "Secur32")
#pragma comment(lib, "ntdll")


#endif //PCH_H
