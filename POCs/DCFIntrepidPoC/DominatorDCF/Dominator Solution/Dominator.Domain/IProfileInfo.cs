namespace Dominator.Domain
{
    public interface IProfileInfo
    {
        string ProfileName { get; set; }
        bool IsSelected { get; set; }
        bool IsValid { get; set; }
        bool IsPredefinedProfile { get; set; }
    }
}
