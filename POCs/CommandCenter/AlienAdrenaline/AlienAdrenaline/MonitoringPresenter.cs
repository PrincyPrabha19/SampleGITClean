using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;

namespace AlienLabs.AlienAdrenaline.App
{
	public interface MonitoringPresenter
	{
		PerformanceMonitoringView View { get; set; }
		ViewRepository ViewRepository { get; set; }
		CommandContainer CommandContainer { get; set; }
		EventTrigger EventTrigger { get; set; }
		bool IsDirty { get; }
	}
}