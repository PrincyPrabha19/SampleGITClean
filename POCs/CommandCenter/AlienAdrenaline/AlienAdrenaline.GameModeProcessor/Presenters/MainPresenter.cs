
using System;
using System.Linq;
using System.Threading;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.GameModeProcessor.Presenters
{
    public class MainPresenter
    {
        #region Public Properties
        public MainView View { get; set; }
        public GameModeProcessorService Model { get; set; }
        #endregion

        #region Private Properties
        private GameModeProfileActionsSummaryData _gameModeProfileActionsExecuteSummaryData;
        private const int WAIT_TIME_MINIMIZE_WINDOW = 5000;
        private const int WAIT_TIME_CLOSE_WINDOW = 3000;
        #endregion

        #region Public Methods
        public void Refresh()
        {
            Model.Refresh(View.GameModeName);
            View.GameModeActionSummaryDataList = Model.GameModeActionSummaryDataList;
            View.GameApplicationNotEngaged = true;
            //View.GameRealPathNotFound = String.IsNullOrEmpty(Model.ProfileRepository.CurrentProfile.GameRealPath);
            View.SetActionSummaryItemsSource();

            updateGameModeActionExecutionSummary();

            Model.ProfileActionExecuteEnded += profileActionExecuteEnded;
            Model.ProfileActionRollbackEnded += profileActionRollbackEnded;
            Model.ProfileExecuteEnded += profileExecuteEnded;
            Model.ProfileRollbackEnded += profileRollbackEnded;
            Model.GameProfileActionEngaged += gameProfileActionEngaged;
        }

        public void Execute()
        {
            ThreadPool.QueueUserWorkItem(executeActions, Model);
        }

        public void Rollback()
        {
            ThreadPool.QueueUserWorkItem(rollbackActions, Model);
        }

        public bool AreThereGameModeActions()
        {
            return Model.GameModeActionSummaryDataList != null && Model.GameModeActionSummaryDataList.Count > 0;
        }

        public void Close()
        {
            if (ServiceFactory.IsThermalProfileServiceInitialized)
                ServiceFactory.ThermalProfileService.Dispose();

            Model.CancelGameDetection();
        }
        #endregion

        #region Event Handlers
        private void profileActionExecuteEnded(GameModeActionSummaryData gameModeActionSummaryData, int index)
        {            
            updateGameModeActionExecutionSummary();
            View.ScrollIntoView(index);

            if (gameModeActionSummaryData.ProfileAction.Type == GameModeActionType.GameApplication)
            {
                var profileActionInfoGameApplication = gameModeActionSummaryData.ProfileAction.ProfileActionInfo as ProfileActionInfoGameApplication;
                if (profileActionInfoGameApplication != null && 
                    !String.IsNullOrEmpty(profileActionInfoGameApplication.ApplicationRealPath))                        
                    ThreadPool.QueueUserWorkItem(rollbackActions, Model);
            }
        }

        private void profileActionRollbackEnded(GameModeActionSummaryData gameModeActionSummaryData, int index)
        {
            updateGameModeActionRollbackSummary();
            View.ScrollIntoView(index);
        }

        private void profileExecuteEnded(GameModeProfileActionsSummaryData gameModeProfileActionsExecuteSummaryData)
        {
            _gameModeProfileActionsExecuteSummaryData = gameModeProfileActionsExecuteSummaryData;
            if (gameModeProfileActionsExecuteSummaryData.ProfileActionsFailed == 0 && !gameModeProfileActionsExecuteSummaryData.GameLaunchDidNotWait)
                new Timer(o => View.MinimizeWindow(), null, WAIT_TIME_MINIMIZE_WINDOW, Timeout.Infinite);       
            else
            if (gameModeProfileActionsExecuteSummaryData.GameLaunchFailed)
                View.RestoreWindow();
        }

        private void profileRollbackEnded(GameModeProfileActionsSummaryData gameModeProfileActionsRollbackSummaryData)
        {
            View.RollBackActionEnded = true;
            View.RestoreWindow();

            if (gameModeProfileActionsRollbackSummaryData.ProfileActionsFailed == 0 && 
                (_gameModeProfileActionsExecuteSummaryData == null || !_gameModeProfileActionsExecuteSummaryData.GameLaunchFailed))
                new Timer(o => View.CloseWindow(), null, WAIT_TIME_CLOSE_WINDOW, Timeout.Infinite);
        }

        private void gameProfileActionEngaged()
        {
            View.GameApplicationNotEngaged = false;
        }
        #endregion

        #region Private Methods
        private void updateGameModeActionExecutionSummary()
        {
            var summaryData = Model.GetProfileActionExecutionSummary();

            View.GameModeActionExecutionSummary = String.Format(
                Properties.Resources.ActionExecutionSummaryText, summaryData.ProfileActionsSucceeded, summaryData.ProfileActionsFailed);
        }

        private void updateGameModeActionRollbackSummary()
        {
            var summaryData = Model.GetProfileActionRollbackSummary();

            View.GameModeActionExecutionSummary = String.Format(
                Properties.Resources.ActionRollbackSummaryText, summaryData.ProfileActionsSucceeded, summaryData.ProfileActionsFailed);
        }
        #endregion

        #region Static Methods
        private static void executeActions(object model)
        {
            ((GameModeProcessorService)model).ExecuteActions();
        }

        private static void rollbackActions(object model)
        {
            ((GameModeProcessorService)model).RollbackActions();
        }
        #endregion
    }
}
