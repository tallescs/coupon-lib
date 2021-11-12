using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace PrinterSample.Print
{
    public class CouponPrintDocument : PrintDocument
    {
        private IEnumerable<Line> _lines;

        public CouponPrintDocument()
        {
            this.DefaultPageSettings.Landscape = false;
            this.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            this.OriginAtMargins = true;
        }

        public int Width => (int)this.DefaultPageSettings.PrintableArea.Width;

        public void Print(Coupon report, string printerName = null, bool preview = false)
        {
            var lines = report.Lines;
            if (!string.IsNullOrWhiteSpace(printerName))
            {
                this.PrinterSettings.PrinterName = printerName;
            }
            else if (!preview)
            {
                using (var dialog = new PrintDialog { UseEXDialog = true, Document = this })
                {
                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;
                }
            }

            _lines = lines;
            if (preview)
            {
                using (var dialog = new PrintPreviewDialog { Document = this })
                    dialog.ShowDialog();
            }
            else
            {
                base.Print();
            }
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            base.OnPrintPage(e);

            var printRectangle = e.PageSettings.PrintableArea;
            var startX = (int)printRectangle.X;
            var currentX = (int)printRectangle.X;
            var currentY = (int)printRectangle.Y;

            foreach (var line in _lines)
            {
                var heights = new List<int>(line.Blocks.Count);

                var breakPage = line.GetHeight(e.Graphics) + currentY > printRectangle.Height;
                if (breakPage)
                {
                    e.HasMorePages = true;
                    break;
                }

                foreach (var block in line.Blocks)
                {
                    var rectangle = GetPrintableRectangle(block, e.Graphics, currentX, currentY);

                    var text = GetBlockText(e.Graphics, block);

                    //TODO: criar brush em block
                    e.Graphics.DrawString(text, block.Font, Brushes.Black, rectangle, block.Format);
                    currentX += block.Width;
                    heights.Add(rectangle.Height);
                }
                currentY += heights.Max();
                currentX = startX;
            }
        }

        private string GetBlockText(Graphics g, Block block)
        {
            return string.IsNullOrEmpty(block.Filler) ? block.Text : GetFilledText(g, block);
        }

        private string GetFilledText(Graphics g, Block block)
        {
            var blockSize = new SizeF(block.TextWidth, block.GetTextHeight(g));

            var resultText = block.Text;
            var textSize = g.MeasureString(block.Text, block.Font,
               blockSize, block.Format, out int charsFilled, out int linesFilled);

            var availableWidth = block.TextWidth - textSize.Width;

            if (linesFilled == 1 && availableWidth > 0 && block.Filler.Length > 0)
            {
                var nextText = GetFilledText(resultText, block.Filler, block.FillPosition);
                var nextWidth = g.MeasureString(nextText, block.Font).ToSize().Width;

                while (nextWidth < block.TextWidth)
                {
                    resultText = nextText;
                    nextText = GetFilledText(resultText, block.Filler, block.FillPosition);
                    nextWidth = g.MeasureString(nextText, block.Font).ToSize().Width;
                }
            }
            return resultText;
        }

        private string GetFilledText(string text, string filler, FillPosition fillPosition)
        {
            if (fillPosition == FillPosition.Right)
                return $"{text}{filler}";
            else if (fillPosition == FillPosition.Left)
                return $"{filler}{text}";
            else
                return $"{filler}{text}{filler}";
        }

        private Rectangle GetPrintableRectangle(Block block, Graphics g, int x, int y)
        {
            var xFinal = x + block.Margins.Left;
            var yFinal = y + block.Margins.Top;

            return new Rectangle(xFinal, yFinal, block.TextWidth, block.GetTextHeight(g));
        }
    }
}
