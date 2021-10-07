//
// UIPlugin.xaml.cpp
// Implementation of the UIPlugin class
//

#include "pch.h"
#include "UIPlugin.xaml.h"

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
using namespace Windows::UI::Xaml::Markup;
using namespace Windows::Storage;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

UIPlugin::UIPlugin()
{
	InitializeComponentFromOptionalPackage();
}

void  UIPlugin::InitializeComponentFromOptionalPackage()
{
	if (_contentLoaded)
	{
		return;
	}
	_contentLoaded = true;

	::Windows::Foundation::Uri^ resourceLocator = ref new ::Windows::Foundation::Uri(L"ms-appx:///NativeUIPlugin/UIPlugin.xaml");
	::Windows::UI::Xaml::Application::LoadComponent(this, resourceLocator, ::Windows::UI::Xaml::Controls::Primitives::ComponentResourceLocation::Nested);
}


void NativeUIPlugin::UIPlugin::Button_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	dateTextBlock->Text = "Button was clicked!";
}
