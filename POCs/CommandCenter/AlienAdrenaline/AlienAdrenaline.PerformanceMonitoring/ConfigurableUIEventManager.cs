using System;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring
{
	public interface ConfigurableUIEventManager
	{
		event Action RefreshUIHasArrived;

		void StartMonitoring();
		void StopMonitoring();
	}
}