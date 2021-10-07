using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
	public abstract class ServiceWorkerBaseClass 
	{
		#region Properties
		protected readonly List<ModifiableKeyValueData> results = new List<ModifiableKeyValueData>();

		private readonly BackgroundWorker worker = new BackgroundWorker();
		private bool stopped = true; 

		private readonly AutoResetEvent wait = new AutoResetEvent(false);
		protected int waitingTime = 1000;
		#endregion

		#region Methods
		public void Start()
		{
			if (!stopped)
				return;

			stopped = false;
			worker.RunWorkerAsync();
		}

		public void Stop()
		{
			if (stopped)
				return;

			stopped = true;
			wait.Set();
		}

		protected void InitializeWorker()
		{
			worker.DoWork -= process;
			worker.DoWork += process;
		}

		protected virtual void SetResultValues()
		{
		}

		private void sleep(int milliseconds)
		{
			wait.Reset();
			wait.WaitOne(milliseconds);
		}		
		#endregion

		#region Event Handlers
		private void process(object sender, DoWorkEventArgs e)
		{
			do
			{
				var locked = Monitor.TryEnter(results, 300);
				if (!locked)
					continue;

				SetResultValues();
				Monitor.Exit(results);

				sleep(waitingTime);
			} while (!stopped);
		}
		#endregion
	}
}
