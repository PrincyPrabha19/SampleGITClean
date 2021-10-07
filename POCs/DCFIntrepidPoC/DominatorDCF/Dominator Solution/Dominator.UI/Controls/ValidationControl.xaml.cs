using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Dominator.UI.Classes.Enums;

namespace Dominator.UI.Controls
{
    public partial class ValidationControl : INotifyPropertyChanged
    {
        private Uri currentImageSource;
        public Uri CurrentImageSource
        {
            get { return currentImageSource; }
            set
            {
                currentImageSource = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentImageSource"));
            }
        }

        public static readonly DependencyProperty validationStatusProperty = DependencyProperty.Register("ValidationStatus", typeof(ValidationControlStatus), typeof(ValidationControl),
            new PropertyMetadata(ValidationControlStatus.None, onValidationStatusChanged));

        public ValidationControlStatus ValidationStatus
        {
            get { return (ValidationControlStatus)GetValue(validationStatusProperty); }
            set { SetValue(validationStatusProperty, value); }
        }

        private bool _rotate;
        public bool Rotate
        {
            get { return _rotate; }
            set
            {
                _rotate = value;

                if (_rotate)
                    rotateStart();
                else
                    rotateStop();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private DoubleAnimation doubleAnimation;
        private static Uri validationInProgressSource;
        private static Uri validationSuccessfulSource;
        private static Uri validationFailedSource;

        static ValidationControl()
        {
            validationInProgressSource = new Uri("pack://application:,,,/OCControls.Resources;component/Media/Icons/validation-inprogress.png", UriKind.Absolute);
            validationSuccessfulSource = new Uri("pack://application:,,,/OCControls.Resources;component/Media/Icons/validation-successful.png", UriKind.Absolute);
            validationFailedSource = new Uri("pack://application:,,,/OCControls.Resources;component/Media/Icons/validation-failed.png", UriKind.Absolute);
        }

        public ValidationControl()
        {
            InitializeComponent();

            img.RenderTransform = new RotateTransform();
            img.RenderTransformOrigin = new Point(0.5, 0.5);

            doubleAnimation = new DoubleAnimation
            {
                By = 360,
                Duration = TimeSpan.FromSeconds(2),
                RepeatBehavior = RepeatBehavior.Forever
            };
        }

        private static void onValidationInProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var validationControl = d as ValidationControl;
            if (validationControl == null) return;
            validationControl.CurrentImageSource = (bool) e.NewValue ? validationInProgressSource : null;
            validationControl.Rotate = (bool) e.NewValue;
        }

        private static void onValidationSuccessfulChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var validationControl = d as ValidationControl;
            if (validationControl == null) return;
            validationControl.Rotate = false;
            validationControl.CurrentImageSource = (bool)e.NewValue ? validationSuccessfulSource : validationFailedSource;
        }

        private static void onValidationStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var validationControl = d as ValidationControl;
            if (validationControl == null) return;

            var validationStatus = (ValidationControlStatus) e.NewValue;
            switch (validationStatus)
            {
                case ValidationControlStatus.InProgress:
                    validationControl.CurrentImageSource = validationInProgressSource;
                    validationControl.Rotate = true;
                    break;
                case ValidationControlStatus.Passed:
                    validationControl.Rotate = false;
                    validationControl.CurrentImageSource = validationSuccessfulSource;
                    break;
                case ValidationControlStatus.Failed:
                    validationControl.Rotate = false;
                    validationControl.CurrentImageSource = validationFailedSource;
                    break;
                default:
                    validationControl.Rotate = false;
                    validationControl.CurrentImageSource = null;
                    break;
            }
        }

        private void rotateStart()
        {
            img.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
        }

        private void rotateStop()
        {
            var rotateTransform = (RotateTransform)img.RenderTransform;
            //rotateTransform.Angle = rotateTransform.Angle; // looks strange, but works.
            //rotateTransform.SetValue(RotateTransform.AngleProperty, rotateTransform.GetValue(RotateTransform.AngleProperty));
            rotateTransform.Angle = 0;
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, null);
        }
    }
}
