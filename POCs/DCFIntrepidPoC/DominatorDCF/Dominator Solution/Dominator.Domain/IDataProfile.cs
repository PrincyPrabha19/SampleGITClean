namespace Dominator.Domain
{
    public interface IDataProfile
    {
        string Name { get; set; }
        string Path { get; set; }
        bool IsRestartRequired { get; set; }
    }
}
