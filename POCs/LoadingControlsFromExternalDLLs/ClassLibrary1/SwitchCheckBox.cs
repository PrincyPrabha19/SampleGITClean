using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ClassLibrary1
{
    public class SwitchCheckBox : Button
    {
    //    #region Properties
    //    public readonly DependencyProperty onTextProperty = DependencyProperty.Register("OnText", typeof(string), typeof(SwitchCheckBox), null);
    //    public string OnText
    //    {
    //        get { return (string)GetValue(onTextProperty); }
    //        set { SetValue(onTextProperty, value); }
    //    }

    //    public readonly DependencyProperty offTextProperty = DependencyProperty.Register("OffText", typeof(string), typeof(SwitchCheckBox), null);
    //    public string OffText
    //    {
    //        get { return (string)GetValue(offTextProperty); }
    //        set { SetValue(offTextProperty, value); }
    //    }
    //    #endregion

        #region Constructors
        public SwitchCheckBox()
        {
            DefaultStyleKey = typeof(Button);
            Height = 200;
            Width = 200;
            Content = "Here i am";
        }
        #endregion
    }
}
