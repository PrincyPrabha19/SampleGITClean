#include "UIPlugin.xaml.h"
#include "UIPlugin3.xaml.h"

using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml::Controls;
using namespace WRTPluginWrapper;

namespace NativeUIPlugin 
{
	public ref class POCIntrepidPlugin sealed : public IIntrepidPlugin
	{
	public:
		virtual property String^ Name;
		virtual property Guid ID;
		virtual property IMap<Guid, UserControl^>^ DashboardViews;
		virtual property UserControl^ ModuleView;
		virtual property bool IsSystemSupported;

		//virtual event OnGameProfileSelected^ GameProfileSelected;

		virtual void LoadGameProfileInViews(Guid profileID) {}
		virtual bool ApplyProfile(Guid profileID) { return true; }
		
		POCIntrepidPlugin()
		{
			DashboardViews = ref new Map<Guid, UserControl^>();
			
			GUID guid;
			HRESULT handle = CoCreateGuid(&guid);
			DashboardViews->Insert(guid, ref new UIPlugin());

			handle = CoCreateGuid(&guid);
			DashboardViews->Insert(guid, ref new UIPlugin3());
		}
	};
}
