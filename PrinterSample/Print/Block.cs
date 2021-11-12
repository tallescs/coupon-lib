using System.Drawing;
using System.Drawing.Printing;

namespace PrinterSample.Print
{
    public class Block
    {
        public string Text { get; set; }
        public Font Font { get; set; }
        public Brush Brush { get; set; }
        public int Width { get; set; }
        public Margins Margins { get; set; }
        public StringFormat Format { get; set; }
        public string Filler { get; set; }
        public FillPosition FillPosition { get; set; }

        public BlockSpacing BlockSpacing { get; set; } 

        public Block(string text, Font font, 
            StringFormat format, int width, Margins margins,
            BlockSpacing blockSpacing = BlockSpacing.Narrow)
        {
            this.Text = text;
            this.Font = font;
            this.Format = format;
            this.Width = width;
            this.Margins = margins;
            this.BlockSpacing = blockSpacing;
        }

        public int TextWidth =>
            Width - Margins.Left - Margins.Right;

        public int GetTextHeight(Graphics g)
        {
            var area = g.MeasureString(Text, Font, TextWidth, Format);
            return area.ToSize().Height + GetExtraHeight();
        }

        private int GetExtraHeight() =>
            BlockSpacing == BlockSpacing.Narrow ? 1 : 8;
    }
}
