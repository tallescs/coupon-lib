using System;
using System.Drawing;
using System.Drawing.Printing;

namespace PrinterLib
{
    public class Block
    {
        protected string Text { get; }
        public Font Font { get; }
        public Brush Brush { get; }
        public int Width { get; }
        public Margins Margins { get; }
        public StringFormat Format { get; }
        public BlockSpacing Spacing { get; }

        public Block(string text, int width, BlockStyle style)
        {
            if (width <= 0)
                throw new ArgumentException("Block width must be greater than 0.");
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Block text cannot be null or empty");
            if (style == null)
                style = new DefaultBlockStyle();

            Text = text;
            Width = width;
            Font = style.Font;
            Format = style.StringFormat;
            Brush = style.Brush;
            Spacing = style.Spacing;
            Margins = style.Margins;
        }

        public virtual string GetText(Graphics g) => Text;

        public virtual int GetWidth() =>
            Width - Margins.Left - Margins.Right;

        public virtual int GetHeight(Graphics g)
        {
            var area = g.MeasureString(Text, Font, GetWidth(), Format);
            return area.ToSize().Height + GetExtraHeight();
        }

        protected int GetExtraHeight() =>
            Spacing == BlockSpacing.Narrow ? 1 : 8;
    }
}
