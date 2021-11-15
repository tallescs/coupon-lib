using System.Drawing;
using System.Drawing.Printing;

namespace PrinterSample.Print
{
    public class DefaultBlockStyle : BlockStyle
    {
        public DefaultBlockStyle()
        {
            Font = new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular);
            Margins = new Margins(2, 2, 2, 2);
            Brush = Brushes.Black;
            StringFormat = StringFormat.GenericTypographic;
            Spacing = BlockSpacing.Narrow;
        }
    }
}