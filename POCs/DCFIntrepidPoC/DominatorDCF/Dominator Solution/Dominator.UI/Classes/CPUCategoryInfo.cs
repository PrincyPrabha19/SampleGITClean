using System.ComponentModel;

namespace Dominator.UI.Classes
{
    public class CPUCategoryInfo : INotifyPropertyChanged
    {
        private int temperature;        
        public int Temperature
        {
            get { return temperature; }
            set
            {
                temperature = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Temperature"));
            }
        }

        private int utilization;
        public int Utilization
        {
            get { return utilization; }
            set
            {
                utilization = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Utilization"));
            }
        }

        private decimal fanType;
        public decimal FanType
        {
            get { return fanType; }
            set
            {
                fanType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FanType"));
            }
        }

        private decimal fanSpeed;
        public decimal FanSpeed
        {
            get { return fanSpeed; }
            set
            {
                fanSpeed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FanSpeed"));
            }
        }

        private decimal voltage;
        public decimal Voltage
        {
            get { return voltage; }
            set
            {
                voltage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Voltage"));
            }
        }

        private decimal voltageMode;
        public decimal VoltageMode
        {
            get { return voltageMode; }
            set
            {
                voltageMode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VoltageMode"));
            }
        }

        private decimal frequency;
        public decimal Frequency
        {
            get { return frequency; }
            set
            {
                frequency = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Frequency"));
            }
        }

        private decimal maximumFrequency;
        public decimal MaximumFrequency
        {
            get { return maximumFrequency; }
            set
            {
                maximumFrequency = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaximumFrequency"));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
