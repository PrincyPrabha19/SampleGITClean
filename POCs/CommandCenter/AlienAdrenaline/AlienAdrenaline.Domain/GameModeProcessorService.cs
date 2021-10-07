using System;
using System.Collections.ObjectModel;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface GameModeProcessorService
    {
        event Action<GameModeActionSummaryData, int> ProfileActionExecuteEnded;
        event Action<GameModeActionSummaryData, int> ProfileActionRollbackEnded;
        event Action<GameModeProfileActionsSummaryData> ProfileExecuteEnded;
        event Action<GameModeProfileActionsSummaryData> ProfileRollbackEnded;
        event Action GameProfileActionEngaged;
        
        ProfileRepository ProfileRepository { get; set; }
        GameModeActionSummaryService GameModeActionSummaryService { get; }
        ObservableCollection<GameModeActionSummaryData> GameModeActionSummaryDataList { get; }
        bool IsSteamGame { get; }
        string SteamGameInstallPath { get; }

        void Refresh(string profileName);
        void ExecuteActions();
        void RollbackActions();
        void CancelGameDetection();
        GameModeProfileActionsSummaryData GetProfileActionExecutionSummary();
        GameModeProfileActionsSummaryData GetProfileActionRollbackSummary();
    }
}
