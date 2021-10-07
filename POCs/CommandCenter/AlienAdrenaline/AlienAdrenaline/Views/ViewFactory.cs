namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface ViewFactory
    {
        View NewView(ViewType type);
    }
}