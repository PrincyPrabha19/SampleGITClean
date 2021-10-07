
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace AlienLabs.AlienAdrenaline.App.Controls
{
    public partial class LineChartControl : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

        #region Properties
        private static readonly DependencyProperty chartSecondsProperty = DependencyProperty.Register("ChartSeconds", typeof(int), typeof(LineChartControl), new UIPropertyMetadata(0));
        public int ChartSeconds
        {
            get { return (int)GetValue(chartSecondsProperty); }
            set { SetValue(chartSecondsProperty, value); }
        }

        private static readonly DependencyProperty intervalXProperty = DependencyProperty.Register("IntervalX", typeof(int), typeof(LineChartControl), new UIPropertyMetadata(3));
        public int IntervalX
        {
            get { return (int)GetValue(intervalXProperty); }
            set { SetValue(intervalXProperty, value); }
        }

        private static readonly DependencyProperty intervalYProperty = DependencyProperty.Register("IntervalY", typeof(int), typeof(LineChartControl), new UIPropertyMetadata(8));
        public int IntervalY
        {
            get { return (int)GetValue(intervalYProperty); }
            set { SetValue(intervalYProperty, value); }
        }

        private ObservableCollection<Point> chartPointCollection;
        public ObservableCollection<Point> ChartPointCollection
        {
            get { return chartPointCollection; }
            set
            {
                chartPointCollection = value;
                if (chartPointCollection.Count >= ChartSeconds)
                {
                    MaximumX = chartPointCollection[chartPointCollection.Count - 1].X;
                    MinimumX = chartPointCollection[0].X;  
                }
                else
                {
                    MaximumX = ChartSeconds;
                    MinimumX = 1;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ChartPointCollection"));
            }
        }

        private double minimumX;
        public double MinimumX
        {
            get { return minimumX; }
            set
            {
                if (minimumX == value)
                    return;

                minimumX = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("MinimumX"));
            }
        }

        private double maximumX;
        public double MaximumX
        {
            get { return maximumX; }
            set
            {
                if (maximumX == value)
                    return;

                maximumX = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("MaximumX"));
            }
        }
        #endregion

        #region Constructor
        public LineChartControl()
        {
            InitializeComponent();            
        }
        #endregion

        #region Public Methods
        public void SetDataContext(ObservableCollection<Point> chartValues)
        {
            ChartPointCollection = chartValues;
        }
        #endregion
    }
}
