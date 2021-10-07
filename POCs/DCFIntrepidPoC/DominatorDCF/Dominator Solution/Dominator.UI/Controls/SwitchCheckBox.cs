using System.Windows;
using System.Windows.Controls;

namespace Dominator.UI.Controls
{
    public class SwitchCheckBox : CheckBox
    {
        #region Properties
        public static readonly DependencyProperty onTextProperty = DependencyProperty.Register("OnText", typeof(string), typeof(SwitchCheckBox));
        public string OnText
        {
            get { return (string)GetValue(onTextProperty); }
            set { SetValue(onTextProperty, value); }
        }

        public static readonly DependencyProperty offTextProperty = DependencyProperty.Register("OffText", typeof(string), typeof(SwitchCheckBox));
        public string OffText
        {
            get { return (string)GetValue(offTextProperty); }
            set { SetValue(offTextProperty, value); }
        }
        #endregion

        #region Constructors
        static SwitchCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwitchCheckBox), new FrameworkPropertyMetadata(typeof(SwitchCheckBox)));
        }
        #endregion
    }
}
