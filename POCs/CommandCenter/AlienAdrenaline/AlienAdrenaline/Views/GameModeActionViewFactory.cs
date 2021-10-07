namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface GameModeActionViewFactory
    {
        GameModeActionView NewView(GameModeActionViewType type);
    }
}