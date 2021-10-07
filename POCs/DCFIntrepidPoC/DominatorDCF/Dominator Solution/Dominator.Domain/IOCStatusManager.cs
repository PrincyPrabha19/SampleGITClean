namespace Dominator.Domain
{
    public interface IOCStatusManager
    {
        bool IsOCEnabled { get; }
        string ActiveProfileName { get; }
        string CurrentProfileName { get; }
        void Initialize();
        bool SaveOCStatus(bool enabled);        
        bool SaveOCActiveProfile(string profileName);
        bool SaveOCCurrentProfile(string profileName);
        bool SaveOCActiveAndCurrentProfile(string profileName);
    }
}
