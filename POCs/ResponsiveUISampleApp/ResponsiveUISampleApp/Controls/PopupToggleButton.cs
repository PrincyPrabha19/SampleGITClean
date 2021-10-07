using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace ResponsiveUISampleApp.Controls
{
    public class PopupToggleButton : ToggleButton
    {
        public static readonly DependencyProperty PopupProperty = DependencyProperty.Register(nameof(Popup), typeof(Popup), typeof(PopupToggleButton), new PropertyMetadata(null, 
            (d, e) => ((PopupToggleButton)d).PopupPropertyChanged(e.NewValue)));
        public Popup Popup
        {
            get => (Popup)GetValue(PopupProperty);
            set => SetValue(PopupProperty, value);
        }

        public static readonly DependencyProperty MainContainerProperty = DependencyProperty.Register(nameof(MainContainer), typeof(UIElement), typeof(PopupToggleButton), new PropertyMetadata(null));
        public UIElement MainContainer
        {
            get => (UIElement)GetValue(MainContainerProperty);
            set => SetValue(MainContainerProperty, value);
        }

        protected override void OnToggle()
        {
            base.OnToggle();

            if (MainContainer is null || !(Popup?.Child is FrameworkElement popupChildElement)) return;

            Popup.IsOpen = IsChecked.GetValueOrDefault();
            if (!Popup.IsOpen) return;

            var transform = TransformToVisual(MainContainer);
            var point = transform.TransformPoint(new Point(0, 0));

            Popup.HorizontalOffset = point.X - popupChildElement.Width + ActualWidth;
            Popup.VerticalOffset = point.Y + ActualHeight;
        }

        private void PopupPropertyChanged(object newValue)
        {
            if (!(newValue is Popup popup)) return;

            popup.Closed += (sender, e) => {
                IsChecked = false;
            };
        }
    }
}
