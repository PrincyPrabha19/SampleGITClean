using System;
using System.ComponentModel;
using Dominator.Domain;

namespace Dominator.UI.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase HostViewModel { get; set; }
        public ShellViewModel NavigationViewModel { get; set; }
        public IOverclockingModel Model { get; set; }
        public IRiskData RiskInfo { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual event Action RiskChanged;

        protected void SetProperty<T>(ref T targetField, T value, string propertyName)
        {
            if (targetField != null && targetField.Equals(value)) return;
            targetField = value;
            onPropertyChanged(propertyName);
        }

        public virtual void Initialize()
        {            
        }

        public virtual void Load()
        {
        }

        public virtual void Unload()
        {
        }

        private void onPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
