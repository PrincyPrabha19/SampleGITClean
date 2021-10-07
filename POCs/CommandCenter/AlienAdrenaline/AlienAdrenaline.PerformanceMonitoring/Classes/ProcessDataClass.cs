namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
	public class ProcessDataClass : ProcessData
	{
		#region Properties
		public string Name { get; private set; }
		public string Description { get; private set; }
		#endregion

		#region Constructor
		public ProcessDataClass(string name, string description)
		{
			Name = name;
			Description = description;
		}
		#endregion

		#region Overriding Methods
		public override string ToString()
		{
			return string.Format("{0},{1}", Name, Description);
		}
		#endregion
	}
}