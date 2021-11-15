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

        public void Print(Coupon coupon, string printerName = null, bool preview = false)
        {
            var lines = coupon.Lines;
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

                    var text = block.GetText(e.Graphics);

                    if (text.StartsWith("Quantidade"))
                    {
                        var x = 1;
                    }
                    e.Graphics.DrawString(text, block.Font, block.Brush, rectangle, block.Format);
                    currentX += block.Width;
                    heights.Add(rectangle.Height);
                }
                currentY += heights.Max();
                currentX = startX;
            }
        }

        private Rectangle GetPrintableRectangle(Block block, Graphics g, int x, int y)
        {
            var xFinal = x + block.Margins.Left;
            var yFinal = y + block.Margins.Top;

            return new Rectangle(xFinal, yFinal, block.GetWidth(), block.GetHeight(g));
        }
    }
}
