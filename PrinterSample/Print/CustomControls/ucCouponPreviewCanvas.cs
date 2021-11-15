using PrinterSample.Print.Samples;
using PrinterSample.Print.Samples.Sale;
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
            var data = new SaleCouponData
            {
                Order = order,
                Company = company,
            };

            var printDocument = new CouponPrintDocument();
            var style = new DefaultBlockStyle();
            var saleCoupon = new SaleCoupon(printDocument.Width, style, data);
            saleCoupon.Build();

            printDocument.Print(saleCoupon);

            SetLines(saleCoupon.Lines);
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

                            imgGraphics.DrawString(block.GetText(graphics),
                                block.Font, block.Brush, rectangle, block.Format);
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

        private Rectangle GetPrintableRectangle(Block block, Graphics g, int x, int y)
        {
            var xFinal = x + block.Margins.Left;
            var yFinal = y + block.Margins.Top;

            return new Rectangle(xFinal, yFinal, block.GetWidth(), block.GetHeight(g));
        }
    }
}