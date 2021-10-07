using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace LoadingExternalPackage.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            SelectedZones = new List<string>();
            SelectedColor = "Red";
            
            //GetAllColors();
        }

        public List<AvailableZone> AvailableZones { get; set; }
        public AvailableZone SelectedZone { get; set; }
        public List<string> SelectedZones { get; set; }

        private string _selectedColor;

        public string SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                if (_selectedColor != value)
                {
                    _selectedColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedColor"));
                }
            }
        }

        public IEnumerable<AvailableColor> AvailableColors { get; set; }

        private List<AvailableColor> GetAllColors()
        {
            List<AvailableColor> colors = new List<AvailableColor>();

            return colors;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class AvailableColor
    {
        public string Tag { get; set; }
        public string DisplayText { get; set; }
        public SolidColorBrush ForegroundColor { get; set; }
    }

    public class AvailableZone
    {
        public string Key { get; set; }
        public string DisplayText { get; set; }
    }
}
