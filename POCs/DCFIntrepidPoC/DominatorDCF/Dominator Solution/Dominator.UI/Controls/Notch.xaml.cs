using System.Windows;

namespace Dominator.UI.Controls
{
    public partial class Notch
    {
        private static readonly DependencyProperty selectedProperty = DependencyProperty.Register("Selected", typeof(bool), typeof(Notch), new UIPropertyMetadata(false, selectedChanged));

        public bool Selected
        {
            get { return (bool)GetValue(selectedProperty); }
            set { SetValue(selectedProperty, value); }
        }

        public Notch()
        {
            InitializeComponent();
        }

        private static void selectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Notch;
            if (control?.rectSelected == null) return;

            control.rectSelected.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
