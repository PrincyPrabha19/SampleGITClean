using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Dominator.UI.Classes.Enums;

namespace Dominator.UI.Controls
{
    public partial class ValidationControl2 : INotifyPropertyChanged
    {
        private bool showCurrentImage;
        public bool ShowCurrentImage
        {
            get { return showCurrentImage; }
            set
            {
                showCurrentImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ShowCurrentImage"));
            }
        }

        private bool showProgressBar;
        public bool ShowProgressBar
        {
            get { return showProgressBar; }
            set
            {
                showProgressBar = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ShowProgressBar"));
            }
        }

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

        public static readonly DependencyProperty showPercentageValueProperty = DependencyProperty.Register("ShowPercentageValue", typeof(bool), typeof(ValidationControl2));
        public bool ShowPercentageValue
        {
            get { return (bool)GetValue(showPercentageValueProperty); }
            set { SetValue(showPercentageValueProperty, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private Storyboard storyboard;
        private DoubleAnimation doubleAnimation;
        private static Uri validationPassedSource;
        private static Uri validationFailedSource;

        static ValidationControl2()
        {
            validationPassedSource = new Uri("pack://application:,,,/OCControls.Resources;component/Media/Icons/validation-passed.png", UriKind.Absolute);
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

        private static void onValidationStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var validationControl = d as ValidationControl2;
            if (validationControl == null) return;

            var validationStatus = (ValidationControlStatus)e.NewValue;
            switch (validationStatus)
            {
                case ValidationControlStatus.InProgress:
                    validationControl.ShowCurrentImage = false;
                    validationControl.ShowProgressBar = true;
                    break;
                case ValidationControlStatus.Passed:
                    validationControl.ShowCurrentImage = true;
                    validationControl.ShowProgressBar = false;
                    validationControl.CurrentImageSource = validationPassedSource;
                    break;
                case ValidationControlStatus.Failed:
                    validationControl.ShowCurrentImage = true;
                    validationControl.ShowProgressBar = false;
                    validationControl.CurrentImageSource = validationFailedSource;
                    break;
                default:
                    validationControl.ShowCurrentImage = false;
                    validationControl.ShowProgressBar = false;
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

            if ((int)e.OldValue > (int)e.NewValue)
            {
                storyBoard.Stop();
                return;
            }

            doubleAnimation.From = (int)e.OldValue;
            doubleAnimation.To = (int)e.NewValue;
            storyBoard.SkipToFill();
            storyBoard.Begin();
        }
    }
}
