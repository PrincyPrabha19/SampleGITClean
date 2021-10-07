namespace Dominator.UI.Classes.Helpers
{
    public class DesignerProperties
    {
        public static bool IsInDesignMode { get; } = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject());
    }
}