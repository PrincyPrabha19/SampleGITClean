
namespace AlienLabs.AlienAdrenaline.App
{
    public interface FolderOperations
    {
        string GetSelectedPath(string defaultShotcutPath = null);
        bool IsValidFolderPath(string path);
    }
}
