using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PrinterLib.Drawer
{
    /// <summary>
    /// Draws a coupon using Graphics in the area.
    /// Draws Lines top to bottom and Blocks left to right.
    /// </summary>
    public class DefaultDrawStrategy : IDrawStrategy
    {
        public DrawResult Draw(Coupon coupon, Graphics graphics, Rectangle area)
        {
            var startX = area.X;
            var currentX = area.X;
            var currentY = area.Y;
            var drawResult = new DrawResult { WillCrossArea = false };
            foreach (var line in coupon.Lines)
            {
                var heights = new List<int>(line.Blocks.Count);

                var crossArea = line.GetHeight(graphics) + currentY > area.Height;
                if (crossArea)
                {
                    drawResult.WillCrossArea = true;
                    break;
                }

                foreach (var block in line.Blocks)
                {
                    var rectangle = block.GetRectangle(graphics, currentX, currentY);

                    var text = block.GetText(graphics);

                    graphics.DrawString(text, block.Font, block.Brush, rectangle, block.Format);
                    currentX += block.Width;
                    heights.Add(rectangle.Height);
                }
                currentY += heights.Max();
                currentX = startX;
            }

            return drawResult;
        }
    }
}
