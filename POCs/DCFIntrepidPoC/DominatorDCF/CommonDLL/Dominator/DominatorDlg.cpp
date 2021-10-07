#include "stdafx.h"
#include "Dominator.h"
#include "DominatorDlg.h"
#include "afxdialogex.h"






namespace DomOCBiosSupportAPI
{
	enum class Status
	{
		Success = 0,
		Failed = 1,
		NotSupported = 2,
		NotInitialized = -1,
	};

	HMODULE module = nullptr;
	int (*Initialize)() = nullptr;
	int (*ReturnOverclockingReport)() = nullptr;
	int (*SetOCUIBIOSControl)(bool enabled) = nullptr;
	int (*ClearOCFailSafeFlag)() = nullptr;
	int (*Release)() = nullptr;

	bool Load()
	{
		return (module = LoadLibrary(L"DomOCBiosSupportAPI.dll")) &&
			(Initialize = reinterpret_cast<decltype(Initialize)>(GetProcAddress(module, "Initialize"))) &&
			(ReturnOverclockingReport = reinterpret_cast<decltype(ReturnOverclockingReport)>(GetProcAddress(module, "ReturnOverclockingReport"))) &&
			(SetOCUIBIOSControl = reinterpret_cast<decltype(SetOCUIBIOSControl)>(GetProcAddress(module, "SetOCUIBIOSControl"))) &&
			(ClearOCFailSafeFlag = reinterpret_cast<decltype(ClearOCFailSafeFlag)>(GetProcAddress(module, "ClearOCFailSafeFlag"))) &&
			(Release = reinterpret_cast<decltype(Release)>(GetProcAddress(module, "Release")));
	}
};











#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CDominatorDlg::CDominatorDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(IDD_DOMINATOR_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CDominatorDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDominatorDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON1, &CDominatorDlg::OnBnClickedButton1)
	ON_BN_CLICKED(IDC_BUTTON2, &CDominatorDlg::OnBnClickedButton2)
	ON_BN_CLICKED(IDC_BUTTON3, &CDominatorDlg::OnBnClickedButton3)
	ON_BN_CLICKED(IDC_BUTTON4, &CDominatorDlg::OnBnClickedButton4)
	ON_BN_CLICKED(IDC_BUTTON5, &CDominatorDlg::OnBnClickedButton5)
	ON_BN_CLICKED(IDC_BUTTON6, &CDominatorDlg::OnBnClickedButton6)
END_MESSAGE_MAP()

BOOL CDominatorDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	//
	DomOCBiosSupportAPI::Load();

	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CDominatorDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

HCURSOR CDominatorDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}





















void CDominatorDlg::OnBnClickedButton1()
{
	CString message = L"Can't load DLL";

	if(DomOCBiosSupportAPI::Initialize)
	{
		const auto result = DomOCBiosSupportAPI::Initialize();

		switch(static_cast<DomOCBiosSupportAPI::Status>(result))
		{
		case DomOCBiosSupportAPI::Status::Success:
			message = L"Success";
			break;
		case DomOCBiosSupportAPI::Status::Failed:
			message = L"Failed";
			break;
		case DomOCBiosSupportAPI::Status::NotSupported:
			message = L"Not Supported";
			break;
		default:
			message.Format(L"Result: %d", result);
		}
	}

	MessageBox(message, L"DomOCBiosSupportAPI::Initialize()");
}

void CDominatorDlg::OnBnClickedButton2()
{
	CString message = L"Can't load DLL";

	if(DomOCBiosSupportAPI::ReturnOverclockingReport)
	{
		const auto result = DomOCBiosSupportAPI::ReturnOverclockingReport();

		switch(static_cast<DomOCBiosSupportAPI::Status>(result))
		{
		case DomOCBiosSupportAPI::Status::Success:
			message = L"Success";
			break;
		case DomOCBiosSupportAPI::Status::Failed:
			message = L"Failed";
			break;
		case DomOCBiosSupportAPI::Status::NotInitialized:
			message = L"Not Initialized";
			break;
		default:
			message.Format(L"Result: 0x%08X\n\n"
						   L"CPU OC State: %d (1: ON, 2: OFF)\n\n"
						   L"Overclocking UI BIOS control: %d (0: not supported, 1: enabled, 2: disabled)\n\n"
						   L"Failsafe Status: %d",
						   result,
						   (BYTE)result, (BYTE)(result >> 8), (BYTE)(result >> 16));
		}
	}

	MessageBox(message, L"DomOCBiosSupportAPI::ReturnOverclockingReport()");
}

void CDominatorDlg::OnBnClickedButton3()
{
	CString message = L"Can't load DLL";

	if(DomOCBiosSupportAPI::SetOCUIBIOSControl)
	{
		const auto result = DomOCBiosSupportAPI::SetOCUIBIOSControl(true);

		switch(static_cast<DomOCBiosSupportAPI::Status>(result))
		{
		case DomOCBiosSupportAPI::Status::Success:
			message = L"Success";
			break;
		case DomOCBiosSupportAPI::Status::Failed:
			message = L"Failed";
			break;
		case DomOCBiosSupportAPI::Status::NotInitialized:
			message = L"Not Initialized";
			break;
		default:
			message.Format(L"Result: %d", result);
		}
	}

	MessageBox(message, L"DomOCBiosSupportAPI::SetOCUIBIOSControl(true)");
}

void CDominatorDlg::OnBnClickedButton4()
{
	CString message = L"Can't load DLL";

	if(DomOCBiosSupportAPI::SetOCUIBIOSControl)
	{
		const auto result = DomOCBiosSupportAPI::SetOCUIBIOSControl(false);

		switch(static_cast<DomOCBiosSupportAPI::Status>(result))
		{
		case DomOCBiosSupportAPI::Status::Success:
			message = L"Success";
			break;
		case DomOCBiosSupportAPI::Status::Failed:
			message = L"Failed";
			break;
		case DomOCBiosSupportAPI::Status::NotInitialized:
			message = L"Not Initialized";
			break;
		default:
			message.Format(L"Result: %d", result);
		}
	}

	MessageBox(message, L"DomOCBiosSupportAPI::SetOCUIBIOSControl(false)");
}

void CDominatorDlg::OnBnClickedButton5()
{
	CString message = L"Can't load DLL";

	if(DomOCBiosSupportAPI::ClearOCFailSafeFlag)
	{
		const auto result = DomOCBiosSupportAPI::ClearOCFailSafeFlag();

		switch(static_cast<DomOCBiosSupportAPI::Status>(result))
		{
		case DomOCBiosSupportAPI::Status::Success:
			message = L"Success";
			break;
		case DomOCBiosSupportAPI::Status::Failed:
			message = L"Failed";
			break;
		case DomOCBiosSupportAPI::Status::NotInitialized:
			message = L"Not Initialized";
			break;
		default:
			message.Format(L"Result: %d", result);
		}
	}

	MessageBox(message, L"DomOCBiosSupportAPI::ClearOCFailSafeFlag()");
}

void CDominatorDlg::OnBnClickedButton6()
{
	CString message = L"Can't load DLL";

	if(DomOCBiosSupportAPI::Release)
	{
		const auto result = DomOCBiosSupportAPI::Release();

		switch(static_cast<DomOCBiosSupportAPI::Status>(result))
		{
		case DomOCBiosSupportAPI::Status::Success:
			message = L"Success";
			break;
		case DomOCBiosSupportAPI::Status::Failed:
			message = L"Failed";
			break;
		case DomOCBiosSupportAPI::Status::NotInitialized:
			message = L"Not Initialized";
			break;
		default:
			message.Format(L"Result: %d", result);
		}
	}

	MessageBox(message, L"DomOCBiosSupportAPI::Release()");
}
