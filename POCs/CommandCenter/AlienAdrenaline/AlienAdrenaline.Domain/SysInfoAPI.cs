namespace AlienLabs.AlienAdrenaline.Domain
{
    public enum Platform { Unknown, Desktop, Mobile }
    public enum OS { Unknown, Vista, Win7, Win8 }

    public interface SysInfoAPI
    {
        Platform platform { get; }
        OS os { get; }
    }
}