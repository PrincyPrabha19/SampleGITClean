namespace Dominator.Domain.Enums
{
    public enum ValidationStatus
    {
        Validated = 1,
        Invalidated = 2,
        UnderValidation = 3,
        ValidationCancelled = 4,
        LoadingFailed = 5
    }
}
