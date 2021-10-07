using System.Collections.ObjectModel;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface GameModeActionSummaryService
    {
        GameModeActionSequenceService GameModeActionSequenceService { get; set; }
        GameModeProfileActionImageRepository GameModeProfileActionImageRepository { get; set; }
        ProfileActionFormatter ProfileActionFormatter { get; set; }
        ObservableCollection<GameModeActionSummaryData> GameModeActionSummaryDataList { get; }
        
        void Refresh();
    }
}