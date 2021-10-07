using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace AlienLabs.AlienAdrenaline.App.Classes.Navigation
{
    /// <summary>
    //// Custom Panel that can be animated closed and open.  
    //// When the IsOpened Property changes, it either expands or contracts
    /// </summary>
    /// 

    public enum ExpandDirection
    {
        Width, Height
    }

    public class ExpandableBorder : Border
    {

        # region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty IsOpenedProperty =
            DependencyProperty.Register("IsOpened", typeof(bool), typeof(ExpandableBorder), new PropertyMetadata(false, OnIsOpenedPropertyChanged));

        [Category("Common Properties")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public bool IsOpened
        {
            get { return (bool)GetValue(IsOpenedProperty); }
            set { SetValue(IsOpenedProperty, value); }
        }

        public static readonly DependencyProperty IsAnimatedProperty =
            DependencyProperty.Register("IsAnimated", typeof(bool), typeof(ExpandableBorder), new PropertyMetadata(true));

        [Category("Common Properties")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public bool IsAnimated
        {
            get { return (bool)GetValue(IsAnimatedProperty); }
            set { SetValue(IsAnimatedProperty, value); }
        }

        public static readonly DependencyProperty SpeedMultiplierProperty =
            DependencyProperty.Register("SpeedMultiplier", typeof(double), typeof(ExpandableBorder), new PropertyMetadata(100.0));

        [Category("Common Properties")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public double SpeedMultiplier
        {
            get { return (double)GetValue(SpeedMultiplierProperty); }
            set { SetValue(SpeedMultiplierProperty, value); }
        }

        public static readonly DependencyProperty ExpandDirectionProperty =
            DependencyProperty.Register("ExpandDirection", typeof(ExpandDirection), typeof(ExpandableBorder), new PropertyMetadata(ExpandDirection.Height));

        [Category("Common Properties")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public ExpandDirection ExpandDirection
        {
            get { return (ExpandDirection)GetValue(ExpandDirectionProperty); }
            set { SetValue(ExpandDirectionProperty, value); }
        }

        # endregion DEPENDENCY PROPERTIES


        # region VARIABLES

        double _currSize;
        double _destSize;
        bool _firstTime = true;
        bool _skipCustomArrange = false;
        DependencyProperty AnimProperty;

        # endregion VARIABLES


        # region CONSTRUCTORS


        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (!IsOpened)
            {
                _skipCustomArrange = true;
                SetupAnimation();
            }
        }

        # endregion CONSTRUCTORS


        # region CALLBACKS

        private static void OnIsOpenedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExpandableBorder THIS = (ExpandableBorder)d;
            THIS.SetupAnimation();
        }

        private void SetupAnimation()
        {
            //  Set a Dependency property to either Width or Height to keep
            //  upcomming code cleaner by avoiding many conditional statements.
            if (ExpandDirection == ExpandDirection.Width)
                AnimProperty = FrameworkElement.WidthProperty;
            else
                AnimProperty = FrameworkElement.HeightProperty;

            // Temporarily set the Width or Height to Auto or Zero in
            // order to get its destination size in the ArrangeOverride Method
            if (IsOpened)
                this.SetValue(AnimProperty, Double.NaN);
            else
                this.SetValue(AnimProperty, 0.0);

            _skipCustomArrange = false;
            InvalidateVisual();
        }


        # endregion CALLBACKS


        # region ARRANGE OVERRIDE

        Size prevSize = new Size(0, 0);

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_skipCustomArrange || AnimProperty == null)
                return base.ArrangeOverride(finalSize);

            _skipCustomArrange = true;

            // If animation is turned off, skip all that code
            if (!IsAnimated)
            {
                if (IsOpened)
                    this.SetValue(AnimProperty, Double.NaN);
                else
                    this.SetValue(AnimProperty, 0.0);

                return base.ArrangeOverride(finalSize);
            }

            // get the resulting finalSize to use for animating
            finalSize = base.ArrangeOverride(finalSize);

            // Setup Animation parameters
            if (ExpandDirection == ExpandDirection.Width)
            {
                _currSize = ActualWidth;

                if (IsOpened)
                    _destSize = finalSize.Width;
                else
                    _destSize = MinWidth;
            }
            else
            {
                _currSize = ActualHeight;

                if (IsOpened)
                    _destSize = finalSize.Height;
                else
                    _destSize = MinHeight;
            }

            // set size back to its pre-temporary value 
            // since we want to Animate to this value.
            this.SetValue(AnimProperty, _currSize);
            PlayStoryboard();

            return finalSize;
        }

        # endregion


        # region ANIMATION

        private void PlayStoryboard()
        {
            KeySpline easeCurve = new KeySpline(0, 0, 0, 1);
            KeyTime duration = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(300));

            if (_firstTime)
                duration = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(10));

            SplineDoubleKeyFrame key = new SplineDoubleKeyFrame(_destSize, duration, easeCurve);

            DoubleAnimationUsingKeyFrames anim = new DoubleAnimationUsingKeyFrames();
            anim.KeyFrames.Add(key);

            Storyboard.SetTarget(anim, this);
            Storyboard.SetTargetProperty(anim, new PropertyPath(AnimProperty));

            Storyboard sizeStoryboard = new Storyboard();
            sizeStoryboard.Children.Add(anim);
            sizeStoryboard.FillBehavior = FillBehavior.Stop;
            sizeStoryboard.Completed += new EventHandler(storyboard_Completed);

            BeginStoryboard(sizeStoryboard);

            _firstTime = false;
        }

        void storyboard_Completed(object sender, EventArgs e)
        {
            if (IsOpened)
                this.SetValue(AnimProperty, Double.NaN);
            else
                this.SetValue(AnimProperty, 0.0);
        }

        # endregion ANIMATION
    }
   
}