//
// UIPlugin3.xaml.cpp
// Implementation of the UIPlugin3 class
//

#include "pch.h"
#include "UIPlugin3.xaml.h"

using namespace NativeUIPlugin;

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

UIPlugin3::UIPlugin3()
{
	InitializeComponentFromOptionalPackage();
}

void UIPlugin3::InitializeComponentFromOptionalPackage()
{
	if (_contentLoaded)
	{
		return;
	}
	_contentLoaded = true;

	::Windows::Foundation::Uri^ resourceLocator = ref new ::Windows::Foundation::Uri(L"ms-appx://Demo.OptionalPackage/UIPlugin3.xaml");
	::Windows::UI::Xaml::Application::LoadComponent(this, resourceLocator, ::Windows::UI::Xaml::Controls::Primitives::ComponentResourceLocation::Nested);
}
