using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using ResponsiveUISampleApp.Classes;

namespace ResponsiveUISampleApp
{
    public sealed partial class MainPage0 : Page
    {
        public MainPage0()
        {
            this.InitializeComponent();
        }

        private void MainPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();

            var width = (int)e.NewSize.Width;
            var height = (int)e.NewSize.Height;

            string widthStr = string.Empty;
            foreach (int widthBreak in ResponsiveUIData.ResponsiveUIElements.Keys)
            {                
                if (width >= widthBreak)
                {
                    widthStr = $"> {widthBreak}";
                    drawColumnsGrid(widthBreak, width);
                    break;
                }
            }

            appView.Title = $"{widthStr} [{width}x{height}]";
        }

        private bool drawingColumns;
        private void drawColumnsGrid(int width, double actualWidth)
        {
            if (!ResponsiveUIData.ResponsiveUIElements.ContainsKey(width)) return;

            if (drawingColumns) return;
            drawingColumns = true;

            try
            {
                var uiData = ResponsiveUIData.ResponsiveUIElements[width];

                if (uiGrid.Children.Count > 0)
                    uiGrid.Children.RemoveAt(0);

                var newGrid = new Grid();

                var columnFirstDefinition = new ColumnDefinition();
                columnFirstDefinition.Width = new GridLength(uiData.MarginWidth);
                newGrid.ColumnDefinitions.Add(columnFirstDefinition);

                var actualTotalWidth =
                    actualWidth - (uiData.MarginWidth * 2) - ((uiData.Columns - 1) * uiData.GutterWidth);
                var columnWidth = actualTotalWidth / uiData.Columns;

                for (var i = 0; i < uiData.Columns; i++)
                {
                    var columnDefinition = new ColumnDefinition();
                    columnDefinition.Width = new GridLength(columnWidth);
                    newGrid.ColumnDefinitions.Add(columnDefinition);

                    var border = new Border();
                    border.Background = new SolidColorBrush(new Color() {A = 80, R = 128, G = 128, B = 128});
                    newGrid.Children.Add(border);
                    Grid.SetColumn(border, newGrid.ColumnDefinitions.Count - 1);

                    if (i >= uiData.Columns - 1) continue;

                    var columnGutterDefinition = new ColumnDefinition();
                    columnGutterDefinition.Width = new GridLength(uiData.GutterWidth);
                    newGrid.ColumnDefinitions.Add(columnGutterDefinition);

                    var border2 = new Border();
                    newGrid.Children.Add(border2);
                    Grid.SetColumn(border2, newGrid.ColumnDefinitions.Count - 1);
                }

                var columnLastDefinition = new ColumnDefinition();
                columnLastDefinition.Width = new GridLength(uiData.MarginWidth);
                newGrid.ColumnDefinitions.Add(columnLastDefinition);

                uiGrid.Children.Add(newGrid);
            }
            finally
            {
                drawingColumns = false;
            }
        }
    }
}
