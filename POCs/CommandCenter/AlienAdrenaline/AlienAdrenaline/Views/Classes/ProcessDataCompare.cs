
using System.Windows.Media;

namespace AlienLabs.AlienAdrenaline.App.Views.Classes
{
    public class ProcessDataCompare
    {
	    #region Properties
	    public string Name1 { get; set; }
	    public string Description1 { get; set; }
	    public string Name2 { get; set; }
	    public string Description2 { get; set; }
	    public SolidColorBrush Color1 { get; set; }
	    public SolidColorBrush Color2 { get; set; }
	    #endregion


		#region Overriding Methods
		public override string ToString()
		{
			return string.Format("{0},{1},{2},{3}", Name1, Description1, Name2, Description2);
		}
		#endregion
    }
}
