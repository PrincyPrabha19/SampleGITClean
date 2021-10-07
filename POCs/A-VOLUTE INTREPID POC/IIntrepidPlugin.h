namespace NativeUIPlugin 
{
	public interface class IIntrepidPlugin
	{
		property Windows::UI::Xaml::Controls::UserControl^ userControl1;
	};

	public interface class IIntepridPluginWrapper
	{
		property IIntrepidPlugin^ Plugin;
	};


	public ref class IntrepidPlugin sealed : public IIntrepidPlugin
	{
	public:
		virtual property Windows::UI::Xaml::Controls::UserControl^ userControl1;
		
		IntrepidPlugin() 
		{
			userControl1 = ref new NativeUIPlugin::UIPlugin();
		}
	};
}
