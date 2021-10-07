using System.ComponentModel;

namespace Dominator.UI.Classes
{
    public class ThermalModeData : INotifyPropertyChanged
    {
        private string mode;
        private string name;

        public string Mode
        {
            get { return mode; }
            set
            {
                mode = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Mode"));
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
