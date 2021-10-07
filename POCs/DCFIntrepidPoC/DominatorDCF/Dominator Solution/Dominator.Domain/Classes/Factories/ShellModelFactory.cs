using Dominator.Domain.Classes.Models;

namespace Dominator.Domain.Classes.Factories
{
    public static class ShellModelFactory
    {
        private static IShellModel shellModel;
        public static IShellModel NewShellModel()
        {
            return shellModel ?? (shellModel = new ShellModel() {BIOSSupportProvider = BIOSSupportProviderFactory.NewBIOSSupportProvider()});
        }
    }
}