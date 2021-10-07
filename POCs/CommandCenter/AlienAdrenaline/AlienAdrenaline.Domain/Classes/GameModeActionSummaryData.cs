
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class GameModeActionSummaryData : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public string ProfileActionName { get; set; }
        public string ProfileActionInfoDescription { get; set; }
        public string ProfileActionInfoDetails { get; set; }
        public byte[] ProfileActionImage { get; set; }


        private ProfileActionStatus profileActionStatus;
        public ProfileActionStatus ProfileActionStatus
        {
            get { return profileActionStatus; }
            set
            {
                profileActionStatus = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ProfileActionStatus"));
            }
        }

        private string profileActionStatusMessage;
        public string ProfileActionStatusMessage
        {
            get { return profileActionStatusMessage; }
            set
            {
                profileActionStatusMessage = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ProfileActionStatusMessage"));
            }
        }

        private double? profileActionStatusProgress;
        public double? ProfileActionStatusProgress
        {
            get { return profileActionStatusProgress; }
            set
            {
                profileActionStatusProgress = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ProfileActionStatusProgress"));
            }
        }

        public ProfileAction ProfileAction { get; set; }        
        #endregion
    }
}
