#include "stdafx.h"
#include "DomOCBiosSupportAPI.h"

#include <wbemidl.h>

#pragma comment(lib, "wbemuuid.lib")

static const auto awccClassName = L"AWCCWmiMethodFunction";
static const auto awccInstanceName = L"AWCCWmiMethodFunction.InstanceName=\"ACPI\\\\PNP0C14\\\\AWCC_0\"";
static auto coInitialized = false;
static CComPtr<IWbemLocator> pLoc = nullptr;
static CComPtr<IWbemServices> pSvc = nullptr;

DOMOCBIOSSUPPORTAPI_API int Initialize()
{
	if(coInitialized)
		return DOMOCBIOSSUPPORTAPI_SUCCESS;

	static const DWORD coInit[] = { COINIT_MULTITHREADED, COINIT_APARTMENTTHREADED, 0xffffffff };
	for(const auto dwCoInit : coInit)
	{
		const auto result = CoInitializeEx(0, dwCoInit);
		if(result == S_OK || result == S_FALSE)
			break;
		else if(result == RPC_E_CHANGED_MODE)
			continue;
		else
			return DOMOCBIOSSUPPORTAPI_FAILED;
	}
	coInitialized = true;

	if(FAILED(CoCreateInstance(CLSID_WbemLocator, 0, CLSCTX_INPROC_SERVER, IID_IWbemLocator, (LPVOID*)&pLoc)) ||
	   FAILED(pLoc->ConnectServer(CComBSTR(L"ROOT\\WMI"), nullptr, nullptr, 0, 0, 0, 0, &pSvc)) ||
	   FAILED(CoSetProxyBlanket(pSvc, RPC_C_AUTHN_WINNT, RPC_C_AUTHZ_NONE, nullptr, RPC_C_AUTHN_LEVEL_CALL, RPC_C_IMP_LEVEL_IMPERSONATE, nullptr, EOAC_NONE)))
	{
		Release();
		return DOMOCBIOSSUPPORTAPI_FAILED;
	}

	CComPtr<IWbemClassObject> pClassInstance = nullptr;
	if(FAILED(pSvc->GetObject(CComBSTR(awccInstanceName), 0, nullptr, &pClassInstance, nullptr)))
	{
		Release();
		return DOMOCBIOSSUPPORTAPI_NOT_SUPPORTED;
	}

	return DOMOCBIOSSUPPORTAPI_SUCCESS;
}

DOMOCBIOSSUPPORTAPI_API int ReturnOverclockingReport()
{
	static const auto methodName = L"Return_OverclockingReport";

	if(!coInitialized)
		return DOMOCBIOSSUPPORTAPI_NOT_INITIALIZED;

	CComPtr<IWbemClassObject> pOutParams = nullptr;
	CComVariant returnValue;

	if(FAILED(pSvc->ExecMethod(CComBSTR(awccInstanceName), CComBSTR(methodName), 0, nullptr, nullptr, &pOutParams, nullptr)) ||
	   FAILED(pOutParams->Get(L"argr", 0, &returnValue, nullptr, nullptr)) ||
	   (returnValue.vt != VT_I4 && returnValue.vt != VT_UI4))
	{
		return DOMOCBIOSSUPPORTAPI_NOT_INITIALIZED;
	}

	return returnValue.intVal;
}

DOMOCBIOSSUPPORTAPI_API int SetOCUIBIOSControl(bool enabled)
{
	static const auto methodName = L"Set_OCUIBIOSControl";

	if(!coInitialized)
		return DOMOCBIOSSUPPORTAPI_NOT_INITIALIZED;

	CComPtr<IWbemClassObject> pClass = nullptr;
	CComPtr<IWbemClassObject> pInParamsDefinition = nullptr;
	CComPtr<IWbemClassObject> pClassInstance = nullptr;
	CComPtr<IWbemClassObject> pOutParams = nullptr;
	CComVariant returnValue;

	if(FAILED(pSvc->GetObject(CComBSTR(awccClassName), 0, nullptr, &pClass, nullptr)) ||
	   FAILED(pClass->GetMethod(methodName, 0, &pInParamsDefinition, nullptr)) ||
	   FAILED(pInParamsDefinition->SpawnInstance(0, &pClassInstance)) ||
	   FAILED(pClassInstance->Put(L"arg2", 0, &CComVariant(enabled ? 1 : 0), 0)) ||
	   FAILED(pSvc->ExecMethod(CComBSTR(awccInstanceName), CComBSTR(methodName), 0, nullptr, pClassInstance, &pOutParams, nullptr)) ||
	   FAILED(pOutParams->Get(L"argr", 0, &returnValue, nullptr, nullptr)) ||
	   (returnValue.vt != VT_I4 && returnValue.vt != VT_UI4))
	{
		return DOMOCBIOSSUPPORTAPI_FAILED;
	}

	return returnValue.intVal;
}

DOMOCBIOSSUPPORTAPI_API int ClearOCFailSafeFlag()
{
	static const auto methodName = L"Clear_OCFailSafeFlag";

	if(!coInitialized)
		return DOMOCBIOSSUPPORTAPI_NOT_INITIALIZED;

	CComPtr<IWbemClassObject> pOutParams = nullptr;
	CComVariant returnValue;

	if(FAILED(pSvc->ExecMethod(CComBSTR(awccInstanceName), CComBSTR(methodName), 0, nullptr, nullptr, &pOutParams, nullptr)) ||
	   FAILED(pOutParams->Get(L"argr", 0, &returnValue, nullptr, nullptr)) ||
	   (returnValue.vt != VT_I4 && returnValue.vt != VT_UI4))
	{
		return DOMOCBIOSSUPPORTAPI_FAILED;
	}

	return returnValue.intVal;
}

DOMOCBIOSSUPPORTAPI_API int Release()
{
	if(!coInitialized)
		return DOMOCBIOSSUPPORTAPI_NOT_INITIALIZED;

	pSvc = nullptr;
	pLoc = nullptr;
	CoUninitialize();

	coInitialized = false;
	return DOMOCBIOSSUPPORTAPI_SUCCESS;
}
