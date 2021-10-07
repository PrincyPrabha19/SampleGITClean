
using System;
using System.ComponentModel;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileActionClass : ProfileAction, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

        #region Private Properties
        private ProfileActionInfo profileActionInfoBackup { get; set; }
        #endregion

        #region ProfileAction Members
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public GameModeActionType Type { get; set; }
        
        private int orderNo;
        public int OrderNo
        {
            get { return orderNo; }
            set 
            { 
                orderNo = value; 
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OrderNo"));
            }
        }

        public byte[] Image { get; set; }        

        public ProfileActionInfo ProfileActionInfo { get; set; }

        public void Execute(GameModeActionSummaryData gameModeActionSummaryData)
        {
            if (EnumHelper.GetAttributeValue<AllowRollbackAttributeClass, bool>(Type))
            {
                var profileActionCreator = new ProfileActionCreatorClass();
                var profileActionBackup = profileActionCreator.New(this, true);
                if (profileActionBackup != null)
                    profileActionInfoBackup = profileActionBackup.ProfileActionInfo;
            }

            ProfileActionInfo.Execute(gameModeActionSummaryData);
        }

        public void Rollback(GameModeActionSummaryData gameModeActionSummaryData)
        {
            if (profileActionInfoBackup != null)
                profileActionInfoBackup.Rollback(gameModeActionSummaryData);
        }
        #endregion

        #region Constructors
        public ProfileActionClass()
        {
            Guid = Guid.NewGuid();
            Name = String.Empty;
        }
        #endregion
    }
}
