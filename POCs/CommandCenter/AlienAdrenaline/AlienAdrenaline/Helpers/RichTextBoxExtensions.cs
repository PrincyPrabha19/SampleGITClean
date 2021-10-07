using System.Windows.Controls;
using System.Windows.Documents;

namespace AlienLabs.AlienAdrenaline.App.Helpers
{
    public static class RichTextBoxExtensions
    {
        public static string SelectText(this RichTextBox rtb, int index, int length)
        {
            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);

            if (textRange.Text.Length >= (index + length))
            {
                TextPointer start = textRange.Start.GetPositionAtOffset(index, LogicalDirection.Forward);
                TextPointer end = textRange.Start.GetPositionAtOffset(index + length, LogicalDirection.Backward);
                rtb.Selection.Select(start, end);
            }
            return rtb.Selection.Text;
        }
    }
}
