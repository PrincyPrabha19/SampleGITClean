using System.Collections.Generic;
using System.ComponentModel;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
	public class ModifiableKeyValueDataClass : ModifiableKeyValueData
	{
		#region Properties
		public string Key { get; private set; }

		private double value;
		public double Value
		{
			get { return value; }
			set
			{
				this.value = value;
				OnPropertyChanged("Value");
			}
		}

		public int Index { get; set; }

		public List<ProcessData> ExtraData { get; set; }

		public MonitoringCategoryInfo CategoryInfo { get; private set; }
		#endregion

		#region Constructor
		public ModifiableKeyValueDataClass(string key, MonitoringCategoryInfo categoryInfo)
		{
			Key = key;
			CategoryInfo = categoryInfo;
		}
		#endregion

		#region INotifyPropertyChanged Members
		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}