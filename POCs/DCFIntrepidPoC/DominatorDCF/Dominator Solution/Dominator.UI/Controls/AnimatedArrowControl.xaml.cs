using System.Windows.Media.Animation;

namespace Dominator.UI.Controls
{
    public partial class AnimatedArrowControl
    {
        private Storyboard flow;


        public AnimatedArrowControl()
        {
            InitializeComponent();
            initAnimation();
        }

        private void initAnimation()
        {
            try
            {
                flow = (Storyboard)FindResource("Flow");
                if (flow != null)
                    flow.RepeatBehavior = RepeatBehavior.Forever;
            }
            catch
            {
                // ignored
            }
        }

        private void isEnabledChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if ((bool) IsEnabled)
                flow.Begin(this, true);
            else
            {
//                flow.Pause();
//                flow.Seek(TimeSpan.Zero);
                flow.Stop(this);
            }
        }
    }
}
