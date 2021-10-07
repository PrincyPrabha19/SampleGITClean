using Windows.UI.Xaml.Controls;

namespace ManagedUIPlugin
{
    public interface IIntrepidPlugin
    {
        UserControl View1 { get; set; }
    }

    public sealed class IntrepidPlugin : IIntrepidPlugin
    {
        public UserControl View1 { get; set; }
    }
}
