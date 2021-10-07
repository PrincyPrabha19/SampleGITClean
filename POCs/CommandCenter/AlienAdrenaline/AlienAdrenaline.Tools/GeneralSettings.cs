using AlienLabs.CommandCenter.Tools;

namespace AlienLabs.AlienAdrenaline.Tools
{
    public static class GeneralSettings
    {
        private static readonly bool isInDesignMode = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject());
        public static bool IsInDesignMode { get { return isInDesignMode; } }

        private static readonly string _startupPath = PathProvider.CommandCenterPath;
    }
}
