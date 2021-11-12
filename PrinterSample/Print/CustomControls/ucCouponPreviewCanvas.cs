using PrinterSample.Print.Samples;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PrinterSample.Print.CustomControls
{
    public partial class ucCouponPreviewCanvas : Panel
    {
        private readonly PictureBox _pictureBox;

        public ucCouponPreviewCanvas()
        {
            _pictureBox = new PictureBox()
            {
                SizeMode = PictureBoxSizeMode.AutoSize,
            };

            InitializeComponent();

            Controls.Add(_pictureBox);
            this.DoubleBuffered = true;
        }

        public void FillWithOrder(OrderDTO order, CompanyDTO company)
        {
            var couponData = new SaleCouponData
            {
                Order = order,
                Company = company,
            };

            var style = new DefaultCouponStyle(this.Width - 30);
            var gen = new SaleCouponGenerator(couponData, style);
            var report = gen.CreateCoupon();

            SetLines(report.Lines);
        }


        public void SetLines(IEnumerable<Line> lines)
        {
            if (lines.Any())
            {
                SuspendLayout();
                VerticalScroll.Value = 0;
                UpdateImageReport(lines);
                ResumeLayout();
            }
        }

        private void UpdateImageReport(IEnumerable<Line> lines)
        {
            var currentX = 1;
            var currentY = 1;
            var startX = 1;
            var imageWidth = Width;

            using (var graphics = this.CreateGraphics())
            {
                var maxHeight = GetMaxHeight(lines, graphics);
                var image = new Bitmap(imageWidth, maxHeight, graphics);

                using (var imgGraphics = Graphics.FromImage(image))
                {
                    imgGraphics.Clear(Color.White);
                    foreach (var line in lines)
                    {
                        var height = line.GetHeight(graphics);
                        var heights = new List<int>(line.Blocks.Count);
                        foreach (var block in line.Blocks)
                        {
                            var rectangle = GetPrintableRectangle(block, graphics, currentX, currentY);

                            var text = GetBlockText(graphics, block);
                            imgGraphics.DrawString(text, block.Font, Brushes.Black, rectangle,
                                block.Format);
                            currentX += block.Width;
                            heights.Add(rectangle.Height);
                        }
                        currentX = startX;
                        currentY += heights.Max();
                    }
                }

                _pictureBox.Image = image;
                _pictureBox.Location = new Point(4, 0);
            }

            Controls.Clear();
            Controls.Add(_pictureBox);
        }

        private int GetMaxHeight(IEnumerable<Line> lines, Graphics graphics) =>
            lines.Sum(p => p.GetHeight(graphics));

        private string GetBlockText(Graphics g, Block block) =>
            string.IsNullOrEmpty(block.Filler) ? block.Text : GetFilledText(g, block);

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