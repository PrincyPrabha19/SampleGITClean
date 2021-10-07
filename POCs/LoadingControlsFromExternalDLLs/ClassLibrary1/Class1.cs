using Windows.UI.Xaml.Controls;

namespace ClassLibrary1
{
    public class Class1
    {
        public UserControl Control;
        public Class1()
        {
            Control = new UserControl();
        }

        public UserControl GetControl()
        {
            return new DynamicControl();
        }

        public SwitchCheckBox GetCheckBoxControl()
        {
            return new SwitchCheckBox();
        }

        public UserControl GetXamlControl()
        {
            return new DynamicUI();
        }
    }
}
