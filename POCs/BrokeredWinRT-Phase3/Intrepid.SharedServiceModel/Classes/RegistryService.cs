namespace Server
{
	public interface RegistryService
	{
		bool CreateRegistryValue(string valueName, object defaultValue);

		object GetRegistryValue(string keyName);
		object GetRegistryValue(string keyName, object defaultValue);
        
		bool SetRegistryValue(string valueName, object currentValue);
		string[] GetSubKeyNames();
        
		void DeleteSubKey(string subkey, bool throwOnMissingSubKey);
	    bool CreateSubKey(string subkey);
	}
}