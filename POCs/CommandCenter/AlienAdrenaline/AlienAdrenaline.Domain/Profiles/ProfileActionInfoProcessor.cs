


using System;
using AlienLabs.AlienAdrenaline.Domain.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles
{
    public interface ProfileActionInfoProcessor
    {
        event Action<GameModeActionSummaryData, int, int> ExecuteProgressed;
        event Action<GameModeActionSummaryData, int, int> RollbackProgressed;

        void Execute();
        void Rollback();
        void Execute(GameModeActionSummaryData gameModeActionSummaryData);
        void Rollback(GameModeActionSummaryData gameModeActionSummaryData);
    }
}
