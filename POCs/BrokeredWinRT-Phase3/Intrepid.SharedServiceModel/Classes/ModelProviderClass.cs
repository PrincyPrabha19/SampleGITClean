namespace Server
{
	public sealed class ModelProviderClass : ModelProvider
	{
		public ModelReader RegistryModelReader { get; set; }

		public string FromRegistry => RegistryModelReader.Read();
	}
}