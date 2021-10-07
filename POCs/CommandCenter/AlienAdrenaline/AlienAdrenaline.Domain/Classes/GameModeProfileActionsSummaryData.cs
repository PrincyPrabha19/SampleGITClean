
namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class GameModeProfileActionsSummaryData
    {
        public int ProfileActionsSucceeded { get; set; }
        public int ProfileActionsFailed { get; set; }
        public int ProfileActionsTotal { get; set; }
        public bool GameLaunchFailed { get; set; }
        public bool GameLaunchDidNotWait { get; set; }
    }
}
