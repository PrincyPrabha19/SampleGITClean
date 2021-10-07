//
// UIPlugin.xaml.h
// Declaration of the UIPlugin class
//

#pragma once

#include "UIPlugin.g.h"
namespace NativeUIPlugin
{
	[Windows::Foundation::Metadata::WebHostHidden]
	public ref class UIPlugin sealed
	{
	public:
		UIPlugin();

	private:
		void InitializeComponentFromOptionalPackage();
		void Button_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
	};
}
