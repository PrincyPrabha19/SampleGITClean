using System;
using System.Collections.Generic;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using ResponsiveUISampleApp.Classes;

namespace ResponsiveUISampleApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LayoutUserControl : UserControl
    {
        public static readonly DependencyProperty ColumnTupleProperty = DependencyProperty.Register(nameof(ColumnTuple), typeof(Tuple<double, double>), typeof(LayoutUserControl), new PropertyMetadata(Tuple.Create(0, 0)));
        public Tuple<double, double> ColumnTuple
        {
            get => (Tuple<double, double>)GetValue(ColumnTupleProperty);
            set => SetValue(ColumnTupleProperty, value);
        }

        public static readonly DependencyProperty ContentWidthProperty = DependencyProperty.Register(nameof(ContentWidth), typeof(double), typeof(LayoutUserControl), new PropertyMetadata(0));
        public double ContentWidth
        {
            get => (double)GetValue(ContentWidthProperty);
            set => SetValue(ContentWidthProperty, value);
        }

        public static readonly DependencyProperty GameSettingsWidthProperty = DependencyProperty.Register(nameof(GameSettingsWidth), typeof(double), typeof(LayoutUserControl), new PropertyMetadata(0));
        public double GameSettingsWidth
        {
            get => (double)GetValue(GameSettingsWidthProperty);
            set => SetValue(GameSettingsWidthProperty, value);
        }

        public static readonly DependencyProperty DashboardGamesWidthProperty = DependencyProperty.Register(nameof(DashboardGamesWidth), typeof(double), typeof(LayoutUserControl), new PropertyMetadata(0));
        public double DashboardGamesWidth
        {
            get => (double)GetValue(DashboardGamesWidthProperty);
            set => SetValue(DashboardGamesWidthProperty, value);
        }

        public static readonly DependencyProperty ModelWidthProperty = DependencyProperty.Register(nameof(ModelWidth), typeof(double), typeof(LayoutUserControl), new PropertyMetadata(0));
        public double ModelWidth
        {
            get => (double)GetValue(ModelWidthProperty);
            set => SetValue(ModelWidthProperty, value);
        }

        public static readonly DependencyProperty ActiveSettingsWidthProperty = DependencyProperty.Register(nameof(ActiveSettingsWidth), typeof(double), typeof(LayoutUserControl), new PropertyMetadata(0));
        public double ActiveSettingsWidth
        {
            get => (double)GetValue(ActiveSettingsWidthProperty);
            set => SetValue(ActiveSettingsWidthProperty, value);
        }

        public LayoutUserControl()
        {
            this.InitializeComponent();
            DataContext = this;
        }

        private void MainPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var width = e.NewSize.Width;

            string widthStr = string.Empty;
            if (width >= 1920 - 96 * 2)
            {
                widthStr = ">= 1920";
                calculateColumnWidth(1920, ref width);
            }
            else if (width >= 1366 - 64 * 2)
            {
                widthStr = ">= 1366";
                calculateColumnWidth(1366, ref width);
            }
            else if (width >= 1024 - 48 * 2)
            {
                widthStr = ">= 1024";
                calculateColumnWidth(1024, ref width);
            }
            else if (width >= 960 - 32 * 2)
            {
                widthStr = ">= 960";
                calculateColumnWidth(960, ref width);
            }
            else if (width >= 720 - 32 * 2)
            {
                widthStr = ">= 720";
                calculateColumnWidth(720, ref width);
            }
            else if (width >= 500 - 16 * 2)
            {
                widthStr = ">= 500";
                calculateColumnWidth(500, ref width);
            }

            var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            appView.Title = $"{widthStr} [{(int)width}]";
        }

        private void calculateColumnWidth(int width, ref double actualWidth)
        {
            if (!ResponsiveUIData.ResponsiveUIElements.ContainsKey(width)) return;
            var uiData = ResponsiveUIData.ResponsiveUIElements[width];

            var actualTotalWidth = actualWidth - ((uiData.Columns - 1) * uiData.GutterWidth);// - (uiData.MarginWidth * 2);
            var columnWidth = actualTotalWidth / uiData.Columns;

            ContentWidth = actualWidth;// - (uiData.MarginWidth * 2);
            ColumnTuple = Tuple.Create(columnWidth, uiData.GutterWidth);

            GameSettingsWidth = columnWidth * uiData.ControlColumns.Item1 + uiData.GutterWidth * (uiData.ControlColumns.Item1 - 1);
            DashboardGamesWidth = columnWidth * uiData.ControlColumns.Item2 + uiData.GutterWidth * (uiData.ControlColumns.Item2 - 1);
            if (uiData.ControlColumns.Item3 > 0)
                ModelWidth = columnWidth * uiData.ControlColumns.Item3 + uiData.GutterWidth * (uiData.ControlColumns.Item3 - 1);
            else ModelWidth = 0;
            ActiveSettingsWidth = columnWidth * uiData.ControlColumns.Item4 + uiData.GutterWidth * (uiData.ControlColumns.Item4 - 1);

            actualWidth += uiData.MarginWidth * 2;
        }

        private void GameSettings_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!(sender is Viewbox viewBox)) return;
            if (!(VisualTreeHelper.GetChild(sender as DependencyObject, 0) is UIElement elem)) return;

            var transform = elem.RenderTransform;
            if (transform != null)
            {
                if (ControlHelper.FindControl<TextBlock>(viewBox, typeof(TextBlock), "gameTitleTextBlock") is TextBlock textblock)
                    textblock.RenderTransform = (Transform) transform.Inverse;
            }
        }
    }
}
