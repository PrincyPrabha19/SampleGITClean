//
// UIPlugin3.xaml.h
// Declaration of the UIPlugin3 class
//

#pragma once

#include "UIPlugin3.g.h"

namespace NativeUIPlugin
{
	[Windows::Foundation::Metadata::WebHostHidden]
	public ref class UIPlugin3 sealed
	{
	public:
		UIPlugin3();

	private:
		void InitializeComponentFromOptionalPackage();
	};
}
