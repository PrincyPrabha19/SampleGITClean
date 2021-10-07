using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace ResponsiveUISampleApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuUserControl : UserControl
    {
        public MenuUserControl()
        {
            this.InitializeComponent();
        }

        private void toggleButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (!(sender is ToggleButton button)) return;
            if (!(button.Tag is Popup popup)) return;
            if (!(popup.Child is FrameworkElement popupChildElement)) return;

            if (!button.IsChecked.Value)
            {
                popup.IsOpen = false;
                return;
            }

            var transform = button.TransformToVisual(this);
            var point = transform.TransformPoint(new Point(0, 0));

            popup.IsOpen = true;
            popup.HorizontalOffset = point.X - popupChildElement.Width + button.ActualWidth;
            popup.VerticalOffset = point.Y + button.Height;            
        }
    }
}
