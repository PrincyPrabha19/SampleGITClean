
using System;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles
{
    public interface ProfileAction
    {
        int Id { get; set; }
        int ProfileId { get; set; }
        Guid Guid { get; set; }
        string Name { get; set; }
        GameModeActionType Type { get; set; }
        int OrderNo { get; set; }
        byte[] Image { get; set; }        

        ProfileActionInfo ProfileActionInfo { get; set; }

        void Execute(GameModeActionSummaryData gameModeActionSummaryData);
        void Rollback(GameModeActionSummaryData gameModeActionSummaryData);
    }
}
