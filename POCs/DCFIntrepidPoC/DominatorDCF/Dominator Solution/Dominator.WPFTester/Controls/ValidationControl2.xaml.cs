using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using DominatorTester.Enums;

namespace DominatorTester.Controls
{
    public partial class ValidationControl2 : INotifyPropertyChanged
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

        public static readonly DependencyProperty validationStatusProperty = DependencyProperty.Register("ValidationStatus", typeof(ValidationControlStatus), typeof(ValidationControl2),
            new PropertyMetadata(ValidationControlStatus.None, onValidationStatusChanged));
        public ValidationControlStatus ValidationStatus
        {
            get { return (ValidationControlStatus)GetValue(validationStatusProperty); }
            set { SetValue(validationStatusProperty, value); }
        }

        public static readonly DependencyProperty percentageProperty = DependencyProperty.Register("ValidationPercentage", typeof(int), typeof(ValidationControl2),
            new PropertyMetadata(0, onValidationPercentageChanged));
        public int ValidationPercentage
        {
            get { return (int)GetValue(percentageProperty); }
            set { SetValue(percentageProperty, value); }
        }

        public static readonly DependencyProperty showPercentageValueProperty = DependencyProperty.Register("ShowPercentageValue", typeof(bool), typeof(ValidationControl2),
            new PropertyMetadata(false));
        public bool ShowPercentageValue
        {
            get { return (bool)GetValue(showPercentageValueProperty); }
            set { SetValue(showPercentageValueProperty, value); }
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

        private Storyboard storyboard;
        private DoubleAnimation doubleAnimation;        
        private static Uri validationInProgressSource;
        private static Uri validationSuccessfulSource;
        private static Uri validationFailedSource;

        static ValidationControl2()
        {
            validationInProgressSource = new Uri("pack://application:,,,/OCControls.Resources;component/Media/Icons/validation-inprogress.png", UriKind.Absolute);
            validationSuccessfulSource = new Uri("pack://application:,,,/OCControls.Resources;component/Media/Icons/validation-successful.png", UriKind.Absolute);
            validationFailedSource = new Uri("pack://application:,,,/OCControls.Resources;component/Media/Icons/validation-failed.png", UriKind.Absolute);
        }

        public ValidationControl2()
        {
            InitializeComponent();

            storyboard = new Storyboard();
            doubleAnimation = new DoubleAnimation(); ;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            Storyboard.SetTarget(doubleAnimation, progressBar);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(ProgressBar.ValueProperty));
            storyboard.Children.Add(doubleAnimation);
        }

        private static void onValidationInProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var validationControl = d as ValidationControl2;
            if (validationControl == null) return;
            validationControl.CurrentImageSource = (bool) e.NewValue ? validationInProgressSource : null;
            validationControl.Rotate = (bool) e.NewValue;
        }

        private static void onValidationSuccessfulChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var validationControl = d as ValidationControl2;
            if (validationControl == null) return;
            validationControl.Rotate = false;
            validationControl.CurrentImageSource = (bool)e.NewValue ? validationSuccessfulSource : validationFailedSource;
        }

        private static void onValidationStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var validationControl = d as ValidationControl2;
            if (validationControl == null) return;

            var validationStatus = (ValidationControlStatus) e.NewValue;
            switch (validationStatus)
            {
                case ValidationControlStatus.InProgress:
                    validationControl.CurrentImageSource = validationInProgressSource;
                    validationControl.Rotate = true;
                    break;
                case ValidationControlStatus.Success:
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

        private static void onValidationPercentageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var validationControl = d as ValidationControl2;
            if (validationControl == null) return;

            var storyBoard = validationControl.storyboard;
            var doubleAnimation = validationControl.doubleAnimation;

            if ((int) e.OldValue > (int) e.NewValue)
            {
                storyBoard.Stop();
                return;
            }

            doubleAnimation.From = (int) e.OldValue;
            doubleAnimation.To = (int) e.NewValue;
            storyBoard.SkipToFill();
            storyBoard.Begin();
        }

        private void rotateStart()
        {
            //img.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
        }

        private void rotateStop()
        {
            //var rotateTransform = (RotateTransform)img.RenderTransform;
            ////rotateTransform.Angle = rotateTransform.Angle; // looks strange, but works.
            ////rotateTransform.SetValue(RotateTransform.AngleProperty, rotateTransform.GetValue(RotateTransform.AngleProperty));
            //rotateTransform.Angle = 0;
            //rotateTransform.BeginAnimation(RotateTransform.AngleProperty, null);
        }
    }
}
