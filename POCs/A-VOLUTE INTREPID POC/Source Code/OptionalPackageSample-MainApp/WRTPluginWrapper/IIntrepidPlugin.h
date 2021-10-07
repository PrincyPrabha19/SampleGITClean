using namespace Platform;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml::Controls;

namespace WRTPluginWrapper
{
	public ref struct GameProfileSelectedEventArgs sealed
	{
		property IVector<String^>^ Games;
		property Guid ProfileID;
	};

	public delegate void OnGameProfileSelected(Object^ sender, GameProfileSelectedEventArgs^ a);

	/// contain a list of s tuple< GUID, IntrepidPlugIn^>
	public interface class IIntrepidPlugin
	{
		property String^ Name;
		property Guid ID;
		property IMap<Guid, UserControl^>^ DashboardViews;
		property UserControl^ ModuleView;	
		property bool IsSystemSupported;
		
		//event OnGameProfileSelected^ GameProfileSelected;

		void LoadGameProfileInViews(Guid profileID);
		bool ApplyProfile(Guid profileID);
	};

	/// contain a list of s tuple< GUID, IntrepidPlugIn^>
	public interface class IIntepridPluginWrapper
	{
		property IIntrepidPlugin^ Plugin;
		property int PluginCount;		
	};

	public ref class IntepridPluginWrapper sealed : public IIntepridPluginWrapper
    {
    public:
		virtual property IIntrepidPlugin^ Plugin;
		virtual property int PluginCount;

		IntepridPluginWrapper() 
		{
		}
    };
}