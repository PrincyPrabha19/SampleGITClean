


namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface GameModeProfileActionImageRepository
    {
        byte[] GetImageFromApplicationPath(string applicationPath);
        byte[] GetImageFromResourcePath(string path);
    }
}