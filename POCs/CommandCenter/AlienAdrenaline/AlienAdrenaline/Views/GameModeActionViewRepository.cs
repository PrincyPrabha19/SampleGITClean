using System;
using System.Collections.Generic;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeActionViewRepository
    {
        GameModeActionView GetByType(GameModeActionViewType type, Guid id);
        IList<GameModeActionContentView> GetAllContentViews();
        void ClearAllContentViews();
    }
}