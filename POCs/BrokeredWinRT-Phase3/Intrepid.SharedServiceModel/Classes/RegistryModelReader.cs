namespace Server
{
	public sealed class RegistryModelReader : ModelReader
	{
		public string Read()
		{
			var result = new RegistryServiceClass(false, @"HARDWARE", @"DESCRIPTION\System\BIOS").GetRegistryValue("SystemProductName");
			return result?.ToString();
		}
	}
}