using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.GameDiscoveryHelper.Helpers;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class GameModeProcessorServiceClass : GameModeProcessorService
    {
        #region Public Properties
        public delegate void delThreadSafeHandleException(Exception ex);
        #endregion

        #region Private Properties
        private ProfileActionProcessor gameLaunchProfileActionProcessor;
        #endregion

        #region GameModeProcessorService Members
        public event Action<GameModeActionSummaryData, int> ProfileActionExecuteEnded;
        public event Action<GameModeActionSummaryData, int> ProfileActionRollbackEnded;
        public event Action<GameModeProfileActionsSummaryData> ProfileExecuteEnded;
        public event Action<GameModeProfileActionsSummaryData> ProfileRollbackEnded;
        public event Action GameProfileActionEngaged;
        public ProfileRepository ProfileRepository { get; set; }
        public GameModeActionSummaryService GameModeActionSummaryService { get; private set; }
        public bool IsSteamGame { get; private set; }
        public string SteamGameInstallPath { get; private set; }

        public ObservableCollection<GameModeActionSummaryData> GameModeActionSummaryDataList
        {
            get
            {
                if (GameModeActionSummaryService != null)
                    return GameModeActionSummaryService.GameModeActionSummaryDataList;
                return null;
            }
        }        

        public void Refresh(string profileName)
        {
            setCurrentProfile(profileName);
        }

        public void ExecuteActions()
        {
            int i = 0;
            foreach (var gameModeActionSummaryData in GameModeActionSummaryDataList)
            {
                try
                {
                    if (gameModeActionSummaryData.ProfileActionStatus != ProfileActionStatus.NotReady)
                    {                        
                        if (gameModeActionSummaryData.ProfileAction.Type == GameModeActionType.GameApplication)
                            if (ProfileExecuteEnded != null)
                            {
                                var profileActionInfoGameApplication = gameModeActionSummaryData.ProfileAction.ProfileActionInfo as ProfileActionInfoGameApplication;
                                if (profileActionInfoGameApplication == null || !String.IsNullOrEmpty(profileActionInfoGameApplication.ApplicationRealPath))
                                    ProfileExecuteEnded(GetProfileActionExecutionSummary());
                            }
                        
                        executeAction(gameModeActionSummaryData);

                        if (gameModeActionSummaryData.ProfileAction.Type == GameModeActionType.GameApplication)
                            if (ProfileExecuteEnded != null)
                                ProfileExecuteEnded(GetProfileActionExecutionSummary());
                    }                        

                    if (ProfileActionExecuteEnded != null)
                        ProfileActionExecuteEnded(gameModeActionSummaryData, i);
                }
                finally
                {
                    i++;
                }
            }
        }

        public void RollbackActions()
        {
            int i = 0;
            foreach (var gameModeActionSummaryData in GameModeActionSummaryDataList)
            {
                try
                {
                    if (gameModeActionSummaryData.ProfileActionStatus == ProfileActionStatus.ExecutionSucceeded &&
                        EnumHelper.GetAttributeValue<AllowRollbackAttributeClass, bool>(gameModeActionSummaryData.ProfileAction.Type))
                    {
                        rollbackAction(gameModeActionSummaryData);
                        if (ProfileActionRollbackEnded != null)
                            ProfileActionRollbackEnded(gameModeActionSummaryData, i);
                    }
                }
                catch (Exception e)
                {
                }
                finally
                {
                    i++;
                }
            }

            if (ProfileRollbackEnded != null)
                ProfileRollbackEnded(GetProfileActionRollbackSummary());
        }

        public void CancelGameDetection()
        {
            if (gameLaunchProfileActionProcessor != null)
                gameLaunchProfileActionProcessor.Cancel();
        }

        public GameModeProfileActionsSummaryData GetProfileActionExecutionSummary()
        {
            int succeeded = 0, failed = 0, total = 0;
            bool gameLaunchFailed = false;
            bool gameLaunchDidNotWait = false;

            if (GameModeActionSummaryDataList != null)
            {                
                succeeded = GameModeActionSummaryDataList.Count(
                    p => p.ProfileActionStatus == ProfileActionStatus.ExecutionSucceeded);
                failed = GameModeActionSummaryDataList.Count(
                    p => p.ProfileActionStatus == ProfileActionStatus.ExecutionFailed);
                total = GameModeActionSummaryDataList.Count(
                    p => p.ProfileActionStatus != ProfileActionStatus.NotReady);

                gameLaunchFailed = GameModeActionSummaryDataList.Count(
                    p => p.ProfileAction.Type == GameModeActionType.GameApplication && 
                        p.ProfileActionStatus == ProfileActionStatus.ExecutionFailed) > 0;

                gameLaunchDidNotWait = GameModeActionSummaryDataList.Count(
                    p => p.ProfileAction.Type == GameModeActionType.GameApplication &&
                        p.ProfileActionStatus == ProfileActionStatus.ExecutionSucceededDidNotWait) > 0;
            }

            return new GameModeProfileActionsSummaryData()
            {
                ProfileActionsSucceeded = succeeded,
                ProfileActionsFailed = failed,
                ProfileActionsTotal = total,
                GameLaunchFailed = gameLaunchFailed,
                GameLaunchDidNotWait = gameLaunchDidNotWait
            };
        }

        public GameModeProfileActionsSummaryData GetProfileActionRollbackSummary()
        {
            int succeeded = 0, failed = 0, total = 0;
            if (GameModeActionSummaryDataList != null)
            {
                succeeded = GameModeActionSummaryDataList.Count(
                    p => p.ProfileActionStatus == ProfileActionStatus.RollbackingSucceeded);
                failed = GameModeActionSummaryDataList.Count(
                    p => p.ProfileActionStatus == ProfileActionStatus.RollbackingFailed);
                total = GameModeActionSummaryDataList.Count(
                    p => p.ProfileActionStatus != ProfileActionStatus.NotReady);
            }

            return new GameModeProfileActionsSummaryData()
            {
                ProfileActionsSucceeded = succeeded,
                ProfileActionsFailed = failed,
                ProfileActionsTotal = total
            };
        }
        #endregion

        #region Private Methods
        private void executeAction(GameModeActionSummaryData gameModeActionSummaryData)
        {
            gameModeActionSummaryData.ProfileActionStatus = ProfileActionStatus.ExecutionStarted;
            gameModeActionSummaryData.ProfileAction.ProfileActionInfo.ExecuteProgressed += ProfileActionInfo_ExecuteProgressed;

            try
            {
                var profileActionProcessor = new ProfileActionProcessor(gameModeActionSummaryData, IsSteamGame, SteamGameInstallPath);
                profileActionProcessor.ProfileActionProcessorEnded += profileActionProcessor_ProfileActionProcessorExecutionEnded;
                if (gameModeActionSummaryData.ProfileAction.Type == GameModeActionType.GameApplication)
                {                    
                    gameLaunchProfileActionProcessor = profileActionProcessor;
                    gameLaunchProfileActionProcessor.ProfileActionProcessorEngaged += gameLaunchProfileActionProcessor_ProfileActionProcessorEngaged;
                }

                var thread = new Thread(profileActionProcessor.Execute);
                thread.Start();
                thread.Join();
            }
            catch (Exception e)
            {
            }
        }

        private void rollbackAction(GameModeActionSummaryData gameModeActionSummaryData)
        {
            gameModeActionSummaryData.ProfileActionStatus = ProfileActionStatus.RollbackingStarted;
            gameModeActionSummaryData.ProfileAction.ProfileActionInfo.RollbackProgressed += ProfileActionInfo_RollbackProgressed;

            try
            {
                var profileActionProcessor = new ProfileActionProcessor(gameModeActionSummaryData, IsSteamGame, SteamGameInstallPath);
                profileActionProcessor.ProfileActionProcessorEnded += profileActionProcessor_ProfileActionProcessorRollbackingEnded;
                var thread = new Thread(profileActionProcessor.Rollback);
                thread.Start();
                thread.Join();
            }
            catch (Exception e)
            {
            }
        }

        private void ProfileActionInfo_ExecuteProgressed(GameModeActionSummaryData gameModeActionSummaryData, int completed, int total)
        {
            gameModeActionSummaryData.ProfileActionStatusProgress = completed;
        }

        private void ProfileActionInfo_RollbackProgressed(GameModeActionSummaryData gameModeActionSummaryData, int completed, int total)
        {
            gameModeActionSummaryData.ProfileActionStatusProgress = completed;
        }

        public void setCurrentProfile(string profileName)
        {
            Profile profile = (from p in ProfileRepository.Profiles
                               where String.Compare(p.Name, profileName, true) == 0
                               select p).FirstOrDefault();

            if (profile != null)
            {
                IsSteamGame = profile.SteamId > 0;
                SteamGameInstallPath = profile.GameInstallPath;
                ServiceFactory.ProfileService.SetCurrentProfile(profile);
                GameModeActionSummaryService = ServiceFactory.GameModeActionSummaryService;
                GameModeActionSummaryService.GameModeActionSequenceService.Refresh();
                GameModeActionSummaryService.Refresh();                
            }
        }
        #endregion

        #region Event Handlers
        private void profileActionProcessor_ProfileActionProcessorExecutionEnded(
            GameModeActionSummaryData gameModeActionSummaryData, ProfileActionStatus status, Exception e)
        {
            if (e != null)
            {
                gameModeActionSummaryData.ProfileActionStatus = ProfileActionStatus.ExecutionFailed;
                gameModeActionSummaryData.ProfileActionStatusMessage = e.Message;
                return;
            }

            gameModeActionSummaryData.ProfileActionStatus = status;
            gameModeActionSummaryData.ProfileActionStatusMessage = null;
            gameModeActionSummaryData.ProfileActionStatusProgress = null;
        }

        private void profileActionProcessor_ProfileActionProcessorRollbackingEnded(
            GameModeActionSummaryData gameModeActionSummaryData, ProfileActionStatus status, Exception e)
        {
            if (e != null)
            {
                gameModeActionSummaryData.ProfileActionStatus = ProfileActionStatus.RollbackingFailed;
                gameModeActionSummaryData.ProfileActionStatusMessage = e.Message;
                return;
            }

            gameModeActionSummaryData.ProfileActionStatus = status;
            gameModeActionSummaryData.ProfileActionStatusMessage = null;
        }

        private void gameLaunchProfileActionProcessor_ProfileActionProcessorEngaged()
        {
            if (GameProfileActionEngaged != null)
                GameProfileActionEngaged();
        }
        #endregion

        /// <summary>
        /// ProfileActionProcessor Class
        /// </summary>
        internal class ProfileActionProcessor : GameSteamPathHelper
        {
            #region Public Properties
            public event Action<GameModeActionSummaryData, ProfileActionStatus, Exception> ProfileActionProcessorEnded;
            public event Action ProfileActionProcessorEngaged;
            public GameModeActionSummaryData GameModeActionSummaryData { get; private set; }
            public bool IsSteamGame { get; private set; }
            public string SteamGameInstallPath { get; private set; }
            #endregion

            #region Private Properties
            private bool cancelGameProcessDetection;
            #endregion

            public ProfileActionProcessor(GameModeActionSummaryData gameModeActionSummaryData, bool isSteamGame, string steamGameInstallPath)
            {
                GameModeActionSummaryData = gameModeActionSummaryData;
                IsSteamGame = isSteamGame;
                SteamGameInstallPath = steamGameInstallPath;
            }

            public void Execute()
            {
                Exception exception = null;

                try
                {
                    if (GameModeActionSummaryData.ProfileAction.Type != GameModeActionType.GameApplication)
                        GameModeActionSummaryData.ProfileAction.Execute(GameModeActionSummaryData);
                    else
                    {                 
                        var profileActionInfoGameApplication = GameModeActionSummaryData.ProfileAction.ProfileActionInfo as ProfileActionInfoGameApplication;
                        if (profileActionInfoGameApplication != null)
                        {
                            var profileActionInfoApplicationProcessor = GameModeActionSummaryData.ProfileAction.ProfileActionInfo as ProfileActionInfoApplicationProcessor;
                            if (profileActionInfoApplicationProcessor != null)
                            {
                                Process process;
                                profileActionInfoApplicationProcessor.Execute(out process);
                                if (process != null)
                                {
                                    bool waitForCurrentApplication = String.Compare(
                                        profileActionInfoGameApplication.ApplicationPath, profileActionInfoGameApplication.ApplicationRealPath, StringComparison.OrdinalIgnoreCase) == 0;
                                    if (waitForCurrentApplication)
                                    {
                                        if (ProfileActionProcessorEngaged != null)
                                            ProfileActionProcessorEngaged();
                                        process.WaitForExit();
                                    }
                                    else
                                    {
                                        if (String.IsNullOrEmpty(profileActionInfoGameApplication.ApplicationRealPath))
                                        {
                                            if (ProfileActionProcessorEnded != null)
                                                ProfileActionProcessorEnded(GameModeActionSummaryData,
                                                                            ProfileActionStatus
                                                                                .ExecutionSucceededDidNotWait, null);
                                            return;
                                        }

                                        Process gameProcess = null;
                                        while (gameProcess == null && !cancelGameProcessDetection)
                                        {
                                            Thread.Sleep(1000);
                                            gameProcess =
                                                ProcessHelper.GetApplicationProcess(
                                                    profileActionInfoGameApplication.ApplicationRealPath);
                                        }

                                        if (cancelGameProcessDetection)
                                            return;

                                        if (gameProcess != null)
                                        {
                                            if (ProfileActionProcessorEngaged != null)
                                                ProfileActionProcessorEngaged();
                                            gameProcess.WaitForExit();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    exception = e;
                }

                if (ProfileActionProcessorEnded != null)
                    ProfileActionProcessorEnded(GameModeActionSummaryData, ProfileActionStatus.ExecutionSucceeded, exception);
            }

            public void Rollback()
            {
                Exception exception = null;

                try
                {
                    GameModeActionSummaryData.ProfileAction.Rollback(GameModeActionSummaryData);
                }
                catch (Exception e)
                {
                    exception = e;
                }

                if (ProfileActionProcessorEnded != null)
                    ProfileActionProcessorEnded(GameModeActionSummaryData, ProfileActionStatus.RollbackingSucceeded, exception);
            }

            public void Cancel()
            {
                cancelGameProcessDetection = true;
            }
        }
    }
}
