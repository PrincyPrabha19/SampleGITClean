
using System;
using AlienLabs.AlienAdrenaline.Domain.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public abstract class ProfileActionInfoProcessorClass : ProfileActionInfoProcessor
    {
        public event Action<GameModeActionSummaryData, int, int> ExecuteProgressed;
        public event Action<GameModeActionSummaryData, int, int> RollbackProgressed;

        public GameModeActionSummaryData GameModeActionSummaryData { get; set; }

        protected void OnExecuteProgressed(int completed, int total)
        {
            if (ExecuteProgressed != null)
                ExecuteProgressed(GameModeActionSummaryData, completed, total);
        }

        protected void OnRollbackProgressed(int completed, int total)
        {
            if (RollbackProgressed != null)
                RollbackProgressed(GameModeActionSummaryData, completed, total);
        }

        public abstract void Execute();

        public virtual void Rollback()
        {
            Execute();
        }

        public void Execute(GameModeActionSummaryData gameModeActionSummaryData)
        {
            GameModeActionSummaryData = gameModeActionSummaryData;
            Execute();
        }

        public void Rollback(GameModeActionSummaryData gameModeActionSummaryData)
        {
            GameModeActionSummaryData = gameModeActionSummaryData;
            Rollback();
        }
    }
}
