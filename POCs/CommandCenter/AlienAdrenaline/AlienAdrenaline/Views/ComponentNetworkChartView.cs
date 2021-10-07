

using System;
using System.Windows.Controls;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface ComponentNetworkChartView : ComponentChartView
    {
        event Action<ComponentChartView> MoreInfoChartsOpened;
        event Action<ComponentChartView> MoreInfoChartsClosed;
        event Action<ComponentChartView> ChartClosed;

        bool AreMoreInfoChartsVisible { get; set; }
        Panel ChartsPanel { get; set; }
        int CardsCount { get; }
    }
}
