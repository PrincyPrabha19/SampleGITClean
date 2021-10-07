using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AlienLabs.AlienAdrenaline.App.Classes.Navigation
{   
    public class Link : Button
    {
        static Link()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Link), new FrameworkPropertyMetadata(typeof(Link)));
        }

        # region DEPENDENCY PROPERTIES


        // IsUnderlined Property
        [Category("Common Properties")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public bool IsUnderlined
        {
            get { return (bool)GetValue(IsUnderlinedProperty); }
            set { SetValue(IsUnderlinedProperty, value); }
        }

        public static readonly DependencyProperty IsUnderlinedProperty =
            DependencyProperty.Register("IsUnderlined", typeof(bool), typeof(Link), new PropertyMetadata(true, OnPropertiesChanged));

         // Hover Color
        public Brush HoverColor
        {
            get { return (Brush)GetValue(HoverColorProperty); }
            set { SetValue(HoverColorProperty, value); }
        }

        public static readonly DependencyProperty HoverColorProperty =
            DependencyProperty.Register("HoverColor", typeof(Brush), typeof(Link), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(50, 255, 255, 255)), OnPropertiesChanged));


        // Pressed Color
        public Brush PressedColor
        {
            get { return (Brush)GetValue(PressedColorProperty); }
            set { SetValue(PressedColorProperty, value); }
        }

        public static readonly DependencyProperty PressedColorProperty =
            DependencyProperty.Register("PressedColor", typeof(Brush), typeof(Link), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(50, 255, 255, 255)), OnPropertiesChanged));

     
        # endregion DEPENDENCY PROPERTIES

        private static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        	var uiElement = d as UIElement;
        	if (uiElement != null) uiElement.InvalidateVisual();
        }
    }
}
