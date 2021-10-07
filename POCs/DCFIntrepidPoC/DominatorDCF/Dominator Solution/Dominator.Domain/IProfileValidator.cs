namespace Dominator.Domain
{
    public interface IProfileValidator
    {
        bool IsValidProfileForSystem(IProfile profile);
    }
}