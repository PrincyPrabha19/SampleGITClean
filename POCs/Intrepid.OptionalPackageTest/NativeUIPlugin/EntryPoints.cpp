#include "pch.h"
#include "IIntrepidPlugin.h"

using namespace Platform;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;

using namespace IntrepidPlugin;

// used in order to export a function without the __stdcall decorator
#define EXPORT comment(linker, "/EXPORT:" __FUNCTION__ "=" __FUNCDNAME__)

extern "C" __declspec(dllexport) void __stdcall LoadPlugins(IInspectable* wrapper)
{
	#pragma EXPORT
	auto pluginWrapper = reinterpret_cast<IntrepidPlugin::IntepridPluginWrapper^>(wrapper);
	pluginWrapper->Plugin = ref new NativeUIPlugin::POCIntrepidPlugin();
}



