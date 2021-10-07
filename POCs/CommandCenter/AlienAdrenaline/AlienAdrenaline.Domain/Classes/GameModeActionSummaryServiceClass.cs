using System.Collections.ObjectModel;
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class GameModeActionSummaryServiceClass : GameModeActionSummaryService
    {
        #region GameModeActionSummaryService Members
        public GameModeActionSequenceService GameModeActionSequenceService { get; set; }
        public GameModeProfileActionImageRepository GameModeProfileActionImageRepository { get; set; }
        public ProfileActionFormatter ProfileActionFormatter { get; set; }
        public ObservableCollection<GameModeActionSummaryData> GameModeActionSummaryDataList { get; set; }

        public void Refresh()
        {
            GameModeActionSummaryDataList = new ObservableCollection<GameModeActionSummaryData>();

            foreach (var profileAction in GameModeActionSequenceService.GameModeProfileActions)
            {
                var gameModeActionSummaryData = new GameModeActionSummaryData() {
                    ProfileActionImage = GameModeActionSequenceService.GetProfileActionImage(profileAction),
                    ProfileAction = profileAction
                };

                ProfileActionFormatter.Format(profileAction, gameModeActionSummaryData);

                GameModeActionSummaryDataList.Add(gameModeActionSummaryData);
            }
        }
        #endregion
    }
}