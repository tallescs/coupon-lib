using System.Drawing;
using System.Drawing.Printing;
namespace PrinterSample.Print
{
    public class BlockStyle
    {
        public Font Font { get; set; }
        public Brush Brush { get; set; }
        public Margins Margins { get; set; }
        public StringFormat StringFormat { get; set; }
        public BlockSpacing Spacing { get; set; }

        public BlockStyle Copy()
        {
            return this.MemberwiseClone() as BlockStyle;
        }
    }
}