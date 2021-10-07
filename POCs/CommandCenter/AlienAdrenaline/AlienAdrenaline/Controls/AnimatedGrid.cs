using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AlienLabs.AlienAdrenaline.App.Controls
{
        //<Grid x:Name="gridInfo" Grid.Row="1" Background="#D6000000" Margin="10,0,0,0" Height="0">
        //    <Border BorderThickness="2" BorderBrush="#FF9F9F9F" CornerRadius="10"/>
        //</Grid>

    public class AnimatedGrid : Grid
    {
        //public static readonly DependencyProperty BorderBrushProperty =
        //    DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(AnimatedGrid), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0xff, 0x9f, 0x9f, 0x9f)), onPropertiesChanged));
        
        //[Category("Common Properties")]
        //[EditorBrowsable(EditorBrowsableState.Always)]
        //public Brush BorderBrush
        //{
        //    get { return (Brush)GetValue(BorderBrushProperty); }
        //    set { SetValue(BorderBrushProperty, value); }
        //}

        public static readonly DependencyProperty OpenGridProperty =
            DependencyProperty.Register("BorderBrush", typeof(bool), typeof(AnimatedGrid), new PropertyMetadata(false, onExpandedChanged));

        [Category("Common Properties")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public bool Expanded
        {
            get { return (bool)GetValue(OpenGridProperty); }
            set { SetValue(OpenGridProperty, value); }
        }

        static AnimatedGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedGrid), new FrameworkPropertyMetadata(typeof(AnimatedGrid)));                                  
        }

        public AnimatedGrid()
        {
            var border = new Border() {
                    BorderThickness = new Thickness(2,2,2,2),
                    CornerRadius = new CornerRadius(10),
                    BorderBrush = Brushes.Red
                };
            Children.Add(border);
        }

        private static void onPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as UIElement;
            if (uiElement != null) uiElement.InvalidateVisual();
        }

        private static void onExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //bool expanded = (bool)d.GetValue(ExpandedProperty);
            //double? from = expanded ? 0 : rowChart.ActualHeight;
            //double? to = buttonChartHelp.IsChecked.HasValue && buttonChartHelp.IsChecked.Value ? rowChart.ActualHeight : 0;

            //var animation = new DoubleAnimation
            //{
            //    From = from,
            //    To = to,
            //    Duration = new Duration(TimeSpan.FromMilliseconds(350))
            //};

            //gridInfo.BeginAnimation(HeightProperty, animation);
        }
    }
}
