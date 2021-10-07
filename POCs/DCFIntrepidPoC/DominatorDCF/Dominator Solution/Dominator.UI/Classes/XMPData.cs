using System.ComponentModel;

namespace Dominator.UI.Classes
{
    public class XMPData : INotifyPropertyChanged
    {
        private int profileID;       
        public int ProfileID
        {
            get { return profileID; }
            set
            {
                profileID = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProfileID"));
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
