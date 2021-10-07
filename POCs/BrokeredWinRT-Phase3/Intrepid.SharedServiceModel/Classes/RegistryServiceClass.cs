using Microsoft.Win32;

namespace Server
{
	public sealed class RegistryServiceClass : RegistryService
	{
		public string FullPath => rootKeyPath + (subKeyPath == "" ? "" : @"\" + subKeyPath);

	    private readonly RegistryKey registryRootKey;
	    private readonly string subKeyPath;
	    private readonly string rootKeyPath;
        private readonly bool forceCreation;

        public RegistryServiceClass(bool force, string rootPath, string keyPath)
		{
			forceCreation = force;
			rootKeyPath = rootPath;
			subKeyPath = keyPath;
			registryRootKey = Registry.LocalMachine;
		}
	
		public bool CreateRegistryValue(string valueName, object defaultValue)
		{
			try
			{
				var key = openSubKey(false);
			    if (key != null) return true;

			    key = openSubKey(true);
			    if (key == null)
			        return false;

                key.CreateSubKey(subKeyPath);
			    key = openSubKey(true);
			    if (key == null)
			        return false;

			    key.SetValue(valueName, defaultValue);
			    return true;
			}
			catch { return false; }
		}

	    private RegistryKey openSubKey(bool writable)
	    {
	        return registryRootKey.OpenSubKey(FullPath, writable);
	    }

	    public object GetRegistryValue(string keyName)
		{
			return GetRegistryValue(keyName, null);
		}

		public object GetRegistryValue(string keyName, object defaultValue)
		{
			if (registryRootKey != null && keyName != "")
			{
				var key = openSubKey(false);
			    if (key == null) return defaultValue;

			    var registryValue = key.GetValue(keyName);
			    return registryValue ?? defaultValue;
			}

			return defaultValue;
		}

		public bool SetRegistryValue(string valueName, object currentValue)
		{
			var success = false;
			if (registryRootKey != null && currentValue != null && valueName != "")
			{
				var key = openSubKey(true);
				if (key == null && forceCreation)
					success = CreateRegistryValue(valueName, currentValue);
				else if (key != null)
				{
					key.SetValue(valueName, currentValue);
					success = true;
				}
			}

			return success;
		}

		public string[] GetSubKeyNames()
		{
			if (registryRootKey != null)
			{
				var key = openSubKey(false);
				if (key != null)
					return key.GetSubKeyNames();
			}
			
			return null;
		}

		public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
		{
			if (registryRootKey == null) return;

			var key = openSubKey(true);
		    key?.DeleteSubKey(subkey, throwOnMissingSubKey);
		}

        public bool CreateSubKey(string subkey)
        {
            if (registryRootKey == null)
                return false;

            var key = openSubKey(true);
            if (key == null)
                return false;

            try
            {
                return key.CreateSubKey(subkey) != null;
            }
            catch
            {
                // ignored
            }

            return false;
        }
    }
}