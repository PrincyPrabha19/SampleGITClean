namespace Dominator.Domain
{
    public interface IDataHeader
    {
        string ProfileName { get; set; }
        string Model { get; set; }
        string Path { get; set; }
        string Status { get; set; }
    }
}
