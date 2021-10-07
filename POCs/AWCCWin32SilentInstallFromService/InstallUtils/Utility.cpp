#include "pch.h"
#include "Utility.h"

BOOL LaunchElevatedProcessAsCurrentUserExternal (const char* processPath, const char* statrupPath, DWORD sessionId, DWORD priorityClass, const char* desktopName, BOOL createSuspended, DWORD& processId)
{
	CString _processPath = CString(processPath);
	CString _statrupPath = CString(statrupPath);
	CString _desktopName = CString(desktopName);
	
	return LaunchElevatedProcessAsCurrentUser(_processPath, _statrupPath, sessionId, priorityClass, _desktopName, createSuspended, processId);
}

BOOL LaunchElevatedProcessAsCurrentUser(const CString& processPath, const CString& statrupPath, DWORD sessionId, DWORD priorityClass, const CString& desktopName, BOOL createSuspended, DWORD& processId)
{
	BOOL retVal = FALSE;
	HANDLE hUserToken;

	if (WTSQueryUserToken(sessionId, &hUserToken))
	{
		TOKEN_LINKED_TOKEN tokenLinkedToken;
		DWORD tokenLinkedTokenSize = 0;

		if (GetTokenInformation(hUserToken, TokenLinkedToken, &tokenLinkedToken, sizeof(tokenLinkedToken), &tokenLinkedTokenSize))
		{
			HANDLE hUserTokenDup;

			if (DuplicateTokenEx(tokenLinkedToken.LinkedToken, MAXIMUM_ALLOWED, NULL, SecurityImpersonation, TokenPrimary, &hUserTokenDup))
			{
				if (SetTokenInformation(hUserTokenDup, TokenSessionId, (void*)&sessionId, sizeof(DWORD)))
				{
					DWORD dwUIAccess = 1;
					if (SetTokenInformation(hUserTokenDup, TokenUIAccess, (void*)&dwUIAccess, sizeof(DWORD)))
					{
						LPVOID pEnv = NULL;
						DWORD dwCreationFlags = createSuspended ? (priorityClass | CREATE_NEW_CONSOLE | CREATE_SUSPENDED) : priorityClass | CREATE_NEW_CONSOLE;
						STARTUPINFO si;
						PROCESS_INFORMATION pi;

						ZeroMemory(&si, sizeof(STARTUPINFO));
						ZeroMemory(&pi, sizeof(PROCESS_INFORMATION));

						si.cb = sizeof(STARTUPINFO);
						CString fullDesktopName;
						fullDesktopName.Format(_T("%s\\%s"), _T("winsta0"), (LPCTSTR)desktopName);
						si.lpDesktop = (LPTSTR)(LPCTSTR)fullDesktopName;

						if (CreateEnvironmentBlock(&pEnv, hUserTokenDup, TRUE))
						{
							dwCreationFlags |= CREATE_UNICODE_ENVIRONMENT;
						}
						else
						{
							pEnv = NULL;
							HiveMindServiceTrace(DBG_LEVEL_WARN, _T("CreateEnvironmentBlock failed for sessionId = %d,gle = 0x%x\n"), sessionId, GetLastError());
						}

						if (CreateProcessAsUser(hUserTokenDup,            // client's access token
							NULL,              // file to execute
							(LPTSTR)(LPCTSTR)processPath,     // command line
							NULL,              // pointer to process SECURITY_ATTRIBUTES
							NULL,              // pointer to thread SECURITY_ATTRIBUTES
							FALSE,             // handles are not inheritable
							dwCreationFlags,  // creation flags
							pEnv,              // pointer to new environment block 
							statrupPath.IsEmpty() ? NULL : (LPCTSTR)statrupPath,   // name of current directory 
							&si,               // pointer to STARTUPINFO structure
							&pi                // receives information about new process
						))
						{
							processId = pi.dwProcessId;
							CloseHandle(pi.hProcess);
							CloseHandle(pi.hThread);
							retVal = TRUE;
						}
						else
						{
							HiveMindServiceTrace(DBG_LEVEL_ERROR, _T("CreateProcessAsUser failed for sessionId = %d,gle = 0x%x\n"), sessionId, GetLastError());
						}

						if (pEnv != NULL)
						{
							DestroyEnvironmentBlock(pEnv);
						}
					}
					else
					{
						HiveMindServiceTrace(DBG_LEVEL_ERROR, _T("SetTokenInformation(TokenUIAccess) failed for sessionId = %d,gle = 0x%x\n"), sessionId, GetLastError());
					}
				}
				else
				{
					HiveMindServiceTrace(DBG_LEVEL_ERROR, _T("SetTokenInformation(TokenSessionId) failed for sessionId = %d,gle = 0x%x\n"), sessionId, GetLastError());
				}

				CloseHandle(hUserTokenDup);
			}
			else
			{
				HiveMindServiceTrace(DBG_LEVEL_ERROR, _T("DuplicateTokenEx failed for sessionId = %d,gle = 0x%x\n"), sessionId, GetLastError());

			}

			CloseHandle(tokenLinkedToken.LinkedToken);

		}
		else
		{
			HiveMindServiceTrace(DBG_LEVEL_ERROR, _T("GetTokenInformation failed for sessionId = %d,gle = 0x%x\n"), sessionId, GetLastError());
		}


		CloseHandle(hUserToken);
	}
	else
	{
		HiveMindServiceTrace(DBG_LEVEL_ERROR, _T("WTSQueryUserToken failed for sessionId = %d,gle = 0x%x\n"), sessionId, GetLastError());
	}

	return retVal;
}