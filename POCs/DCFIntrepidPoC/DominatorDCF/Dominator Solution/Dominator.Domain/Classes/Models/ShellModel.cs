namespace Dominator.Domain.Classes.Models
{
    public class ShellModel : IShellModel
    {
        public IBIOSSupportProvider BIOSSupportProvider { get; set; }
        public bool IsBIOSInterfaceSupported => !BIOSSupportProvider.IsBIOSInterfaceNotSupported;
    }
}