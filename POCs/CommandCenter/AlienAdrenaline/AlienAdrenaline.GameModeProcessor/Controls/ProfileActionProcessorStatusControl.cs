using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.GameModeProcessor.Controls
{
    public class ProfileActionProcessorStatusControl : Control
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Public Properties
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource),
            typeof(ProfileActionProcessorStatusControl), null);
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(ProfileActionStatus),
            typeof(ProfileActionProcessorStatusControl), new PropertyMetadata(ProfileActionStatus.None, statusChanged));
        public ProfileActionStatus Status
        {
            get { return (ProfileActionStatus)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public static readonly DependencyProperty StatusMessageProperty = DependencyProperty.Register("StatusMessage", typeof(string),
            typeof(ProfileActionProcessorStatusControl), null);
        public string StatusMessage
        {
            get { return (string)GetValue(StatusMessageProperty); }
            set { SetValue(StatusMessageProperty, value); }
        }
        #endregion

        #region Static Constructors
        static ProfileActionProcessorStatusControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProfileActionProcessorStatusControl), new FrameworkPropertyMetadata(typeof(ProfileActionProcessorStatusControl)));
        }
        #endregion

        #region Private Methods
        private static void statusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var status = (ProfileActionStatus) e.NewValue;
            switch (status)
            {
                case ProfileActionStatus.ExecutionStarted:
                    ((ProfileActionProcessorStatusControl) d).ImageSource = new BitmapImage(
                            new Uri(@"/GameModeProcessor;component/Media/Buttons/executing.png", UriKind.Relative)
                        );
                    break;
                case ProfileActionStatus.ExecutionSucceeded:
                    ((ProfileActionProcessorStatusControl)d).ImageSource = new BitmapImage(
                            new Uri(@"/GameModeProcessor;component/Media/Buttons/executed.png", UriKind.Relative)
                        );
                    break;
                case ProfileActionStatus.ExecutionFailed:
                    ((ProfileActionProcessorStatusControl)d).ImageSource = new BitmapImage(
                            new Uri(@"/GameModeProcessor;component/Media/Buttons/failed.png", UriKind.Relative)
                        );
                    break;
                case ProfileActionStatus.RollbackingStarted:
                    ((ProfileActionProcessorStatusControl)d).ImageSource = new BitmapImage(
                            new Uri(@"/GameModeProcessor;component/Media/Buttons/executing.png", UriKind.Relative)
                        );
                    break;
                case ProfileActionStatus.RollbackingSucceeded:
                    ((ProfileActionProcessorStatusControl)d).ImageSource = new BitmapImage(
                            new Uri(@"/GameModeProcessor;component/Media/Buttons/rollbacked.png", UriKind.Relative)
                        );
                    break;
                case ProfileActionStatus.RollbackingFailed:
                    ((ProfileActionProcessorStatusControl)d).ImageSource = new BitmapImage(
                            new Uri(@"/GameModeProcessor;component/Media/Buttons/failed.png", UriKind.Relative)
                        );
                    break;
                default:
                    ((ProfileActionProcessorStatusControl) d).ImageSource = null;
                    break;
            }

            //if ((bool)e.NewValue)
            //    newUpdateAnimation.Begin((FrameworkElement)d, true);
            //else
            //{
            //    newUpdateAnimation.Stop((FrameworkElement)d);
            //    newUpdateAnimation.Seek(new TimeSpan(0));
            //}
        }
        #endregion        
    }
}
