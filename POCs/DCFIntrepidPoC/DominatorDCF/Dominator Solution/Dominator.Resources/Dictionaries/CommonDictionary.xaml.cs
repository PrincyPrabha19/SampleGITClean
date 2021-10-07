using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dominator.Resources.Dictionaries
{
    public partial class CommonDictionary
    {
        #region Event Handlers
        private void textBlock_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            var text = sender as TextBlock;
            if (text == null) return;

            var txt = new FormattedText(text.Text, CultureInfo.CurrentCulture, text.FlowDirection, new Typeface(text.FontFamily, text.FontStyle, text.FontWeight, text.FontStretch), text.FontSize, text.Foreground);
            if (txt.Width <= text.RenderSize.Width)
                e.Handled = true;
        }
        #endregion
    }
}
