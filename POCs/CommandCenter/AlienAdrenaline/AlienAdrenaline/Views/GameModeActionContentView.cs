


using System;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeActionContentView : GameModeActionView
    {
        event Action ProfileAction_Changed;

        void Refresh();
    }
}
