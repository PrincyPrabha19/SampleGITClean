#ifdef DOMOCBIOSSUPPORTAPI_EXPORTS
#define DOMOCBIOSSUPPORTAPI_API __declspec(dllexport)
#else
#define DOMOCBIOSSUPPORTAPI_API __declspec(dllimport)
#endif

#define DOMOCBIOSSUPPORTAPI_SUCCESS 0
#define DOMOCBIOSSUPPORTAPI_FAILED 1
#define DOMOCBIOSSUPPORTAPI_NOT_INITIALIZED -1
#define DOMOCBIOSSUPPORTAPI_NOT_SUPPORTED 2

#ifdef __cplusplus
extern "C" {
#endif

	DOMOCBIOSSUPPORTAPI_API int Initialize();
	DOMOCBIOSSUPPORTAPI_API int ReturnOverclockingReport();
	DOMOCBIOSSUPPORTAPI_API int SetOCUIBIOSControl(bool enabled);
	DOMOCBIOSSUPPORTAPI_API int ClearOCFailSafeFlag();
	DOMOCBIOSSUPPORTAPI_API int Release();

#ifdef __cplusplus
}
#endif
