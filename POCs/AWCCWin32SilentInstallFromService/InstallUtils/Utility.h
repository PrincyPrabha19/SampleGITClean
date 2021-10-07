#pragma once

using namespace ATL;

//static BOOL LaunchElevatedProcessAsCurrentUser(const CString& processPath, const CString& statrupPath, DWORD sessionId, DWORD priorityClass, const CString& desktopName, BOOL createSuspended, DWORD& processId);

__declspec(dllexport) BOOL __cdecl LaunchElevatedProcessAsCurrentUserExternal(const char* processPath, const char* statrupPath, DWORD sessionId, DWORD priorityClass, const char* desktopName, BOOL createSuspended, DWORD& processId);
BOOL LaunchElevatedProcessAsCurrentUser(const CString& processPath, const CString& statrupPath, DWORD sessionId, DWORD priorityClass, const CString& desktopName, BOOL createSuspended, DWORD& processId);

