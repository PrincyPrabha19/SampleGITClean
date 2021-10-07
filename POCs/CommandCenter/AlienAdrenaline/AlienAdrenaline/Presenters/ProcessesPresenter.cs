using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using AlienLabs.AlienAdrenaline.App.Helpers;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.App.Views.Classes;
using AlienLabs.AlienAdrenaline.App.Views.Xaml;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
	public class ProcessesPresenter
	{
		#region Properties
		private ProcessesView view;
		private List<ProcessData> regularData;
		private List<ProcessDataCompare> compareData;
		#endregion

		#region Methods
        public void ShowData(ExtendedPoint extendedPoint)
        {
			regularData = extendedPoint.ExtraData;
			compareData = null; 
			
            view = new ProcessesDialog();
			view.CopyDataToClipboard +=view_CopyDataToClipboard;
	        view.Label1ProcessCount = extendedPoint.ExtraData.Count;
            view.Label1CPUUsage = (int) extendedPoint.Y;
            view.SetData(extendedPoint.ExtraData);
            view.ShowDialog();
        }

		public void ShowData(ExtendedPoint extendedPoint1, ExtendedPoint extendedPoint2)
		{
			view =  new ProcessesDialog();
			view.CopyDataToClipboard += view_CopyDataToClipboard;

            var data = new List<ProcessDataCompare>();

            var result0 = extendedPoint1.ExtraData.Where(p1 => extendedPoint2.ExtraData.Exists(p2 => p2.Name == p1.Name)).OrderBy(value => value.Name)
                .Select(value => new ProcessDataCompare { Name1 = value.Name, Description1 = value.Description, Name2 = value.Name, Description2 = value.Description });
            var result1 = extendedPoint1.ExtraData.Where(p1 => extendedPoint2.ExtraData.All(p2 => p2.Name != p1.Name)).OrderBy(value => value.Name)
                .Select(value => new ProcessDataCompare { Name1 = value.Name, Description1 = value.Description, Color1 = extendedPoint1.Color });
            var result2 = extendedPoint2.ExtraData.Where(p2 => extendedPoint1.ExtraData.All(p1 => p1.Name != p2.Name)).OrderBy(value => value.Name)
                .Select(value => new ProcessDataCompare { Name2 = value.Name, Description2 = value.Description, Color2 = extendedPoint2.Color });

            data.AddRange(result1);
            data.AddRange(result2);
            data.AddRange(result0);

			regularData = null;
			compareData = data; 

            view.Label1Foreground = new SolidColorBrush(Color.FromArgb(0xff, extendedPoint1.Color.Color.R, extendedPoint1.Color.Color.G, extendedPoint1.Color.Color.B));
            view.Label1ProcessCount = extendedPoint1.ExtraData.Count;
            view.Label1CPUUsage = (int)extendedPoint1.Y;

            view.Label2Foreground = new SolidColorBrush(Color.FromArgb(0xff, extendedPoint2.Color.Color.R, extendedPoint2.Color.Color.G, extendedPoint2.Color.Color.B));
            view.Label2ProcessCount = extendedPoint2.ExtraData.Count;
            view.Label2CPUUsage = (int)extendedPoint2.Y;

            view.SetData(data);
			
            view.ShowDialog();
		}
	    #endregion	

		#region Event Handlers
		private void view_CopyDataToClipboard()
		{
			if (regularData == null)
				ClipboardHelper<ProcessDataCompare>.Copy(compareData);
			else
				ClipboardHelper<ProcessData>.Copy(regularData);
		}
		#endregion
	}
}
