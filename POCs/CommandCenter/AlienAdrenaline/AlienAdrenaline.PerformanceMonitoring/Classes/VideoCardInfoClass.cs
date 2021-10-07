using System.Collections.Generic;
using AlienLabs.Tools;
using VideoCardHelperLibrary;
using VideoCardHelperLibrary.Classes;
using VideoCardHelperLibrary.Classes.Factories;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
	public class VideoCardInfoClass : ServiceWorkerBaseClass, HWInfo
	{
		#region Properties
		public List<ModifiableKeyValueData> Results { get { return results; } }

		public string DataTitle { get; private set; }
		public double Maximum { get; private set; }

		//private readonly List<NvidiaAPI> graphicCards = new List<NvidiaAPI>();
	    private VideoCards videoCards;
		private int settingsCount;
		#endregion

		#region Methods
		public void Initialize()
		{
			initializeData();
			initializeDataTitle();
			InitializeWorker();
		}

		private void initializeData()
		{
			waitingTime = 1000;

		    videoCards = VideoCardFactory.NewVideoCards();
            if (videoCards == null) return;

            for (var i = 0; i < videoCards.Count; i++)
            {
                var cardVideoSettings = videoCards[i];
                results.Add(new ModifiableKeyValueDataClass(string.Format(Properties.Resources.GpuUsage, cardVideoSettings.VideoCardName), MonitoringCategoryInfo.Video_GPUUsage) { Value = 0 });
                results.Add(new ModifiableKeyValueDataClass(string.Format(Properties.Resources.VideoTemperature, cardVideoSettings.VideoCardName), MonitoringCategoryInfo.Video_Temperature) { Value = 0 });
                
                if (videoCards is NvidiaVideoCards)
                    results.Add(new ModifiableKeyValueDataClass(string.Format(Properties.Resources.MemoryLoad, cardVideoSettings.VideoCardName), MonitoringCategoryInfo.Video_MemoryLoad) { Value = 0 });

                if (SystemInfo.Platform == Platform.Desktop || SystemInfo.Platform == Platform.Desktopx64)
                    results.Add(new ModifiableKeyValueDataClass(string.Format(Properties.Resources.FanSpeed, cardVideoSettings.VideoCardName), MonitoringCategoryInfo.Video_FanSpeed) { Value = 0 });
            }

            if (videoCards.Count > 0)
                settingsCount = results.Count / videoCards.Count;

			Maximum = 100;
		}

		private void initializeDataTitle()
		{
            if (videoCards == null) return;

			if (!string.IsNullOrEmpty(DataTitle))
				return;

            switch (videoCards.Count)
            {
                case 0:
                    DataTitle = ""; //Properties.Resources.NVidiaCardNotFound;
                    break;
//                case 1:
//                    DataTitle = graphicCards[0].VideoCardName;
//                    break;
                default:
                    DataTitle = Properties.Resources.GrapichsCard;
                    break;
            }
		}

		protected override void SetResultValues()
		{
            if (videoCards == null) return;

            videoCards.RefreshValues();
            
            for (var i = 0; i < videoCards.Count; i++)
		    {
                int index = 0;

                var cardVideoSettings = videoCards[i];
                results[i * settingsCount + index++].Value = cardVideoSettings.GPUUsage;
                results[i * settingsCount + index++].Value = cardVideoSettings.GPUTemperature;

                if (videoCards is NvidiaVideoCards)
                    results[i * settingsCount + index++].Value = cardVideoSettings.MemoryLoad;

                if (SystemInfo.Platform == Platform.Desktop || SystemInfo.Platform == Platform.Desktopx64)
                    results[i * settingsCount + index++].Value = cardVideoSettings.FanSpeed;
		    }

            //for (var i = 0; i < graphicCards.Count; i++)
            //{
            //    graphicCards[i].RefreshValues();
            //    results[i * settingsCount].Value = graphicCards[i].GpuUsage;
            //    results[i * settingsCount + 1].Value = graphicCards[i].GPUTemperature;
            //    results[i * settingsCount + 2].Value = graphicCards[i].MemoryLoad;

            //    if (SystemInfo.Platform == Platform.Desktop || SystemInfo.Platform == Platform.Desktopx64)
            //        results[i * settingsCount + 3].Value = graphicCards[i].FanSpeed;
            //}
		}
		#endregion
	}
}
