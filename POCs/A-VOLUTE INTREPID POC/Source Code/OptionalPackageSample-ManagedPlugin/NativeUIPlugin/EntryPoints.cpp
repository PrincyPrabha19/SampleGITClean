#include "pch.h"

using namespace Platform;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;



// used in order to export a function without the __stdcall decorator
#define EXPORT comment(linker, "/EXPORT:" __FUNCTION__ "=" __FUNCDNAME__)

extern "C" __declspec(dllexport) void __stdcall InsertUIPlugin(IInspectable* grid)
{
	auto gridRef = reinterpret_cast<Grid^>(grid);

	#pragma EXPORT
	try
	{
		auto uiFrame = ref new ManagedUIPlugin::UIPlugin();
		gridRef->Children->Append(uiFrame);
	} 
	catch (COMException^ e)
	{
		//Add a textblock with the error message
		auto errTextBlock = ref new TextBlock();
		errTextBlock->Text = e->Message;
		gridRef->Children->Append(errTextBlock);
	}

}

extern "C" __declspec(dllexport) BSTR __stdcall GetPluginName()
{
	#pragma EXPORT
	return SysAllocString(L"Audio C#");
}




