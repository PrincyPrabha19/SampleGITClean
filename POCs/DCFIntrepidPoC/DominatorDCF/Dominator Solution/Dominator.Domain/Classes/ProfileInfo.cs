using System.ComponentModel;

namespace Dominator.Domain.Classes
{
    public class ProfileInfo : IProfileInfo, INotifyPropertyChanged
    {
        private string profileName;
        public string ProfileName
        {
            get { return profileName; }
            set
            {
                profileName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProfileName"));
            }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
            }
        }

        private bool isValid;
        public bool IsValid
        {
            get { return isValid; }
            set
            {
                isValid = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsValid"));
            }
        }
        public bool IsPredefinedProfile { get; set; }


        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}