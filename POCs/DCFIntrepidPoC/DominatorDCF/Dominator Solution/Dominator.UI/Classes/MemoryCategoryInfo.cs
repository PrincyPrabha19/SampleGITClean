using System.ComponentModel;

namespace Dominator.UI.Classes
{
    public class MemoryCategoryInfo : INotifyPropertyChanged
    {
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

        private decimal utilization;
        public decimal Utilization
        {
            get { return utilization; }
            set
            {
                utilization = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Utilization"));
            }
        }

        private bool isXMPSupported;
        public bool IsXMPSupported
        {
            get { return isXMPSupported; }
            set
            {
                isXMPSupported = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsXMPSupported"));
            }
        }

        private int xmpProfileID;
        public int XMPProfileID
        {
            get { return xmpProfileID; }
            set
            {
                xmpProfileID = value;
                XMPProfileName = value > 0 ? string.Format(Properties.Resources.XMPFormat, xmpProfileID-1) : Properties.Resources.Disabled;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("XMPProfileID"));
            }
        }

        private string xmpProfileName;
        public string XMPProfileName
        {
            get { return xmpProfileName; }
            private set
            {
                xmpProfileName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("XMPProfileName"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}