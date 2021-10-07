namespace Dominator.Domain.Classes.Profiles
{
    public class DataHeader : IDataHeader
    {   
        public string ProfileName { get; set; }
        public string Model { get; set; }
        public string Path { get; set; }
        public string Status { get; set; }
    }
}