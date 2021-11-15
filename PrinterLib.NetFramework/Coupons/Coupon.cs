using System.Collections.Generic;

namespace PrinterLib
{
    public class Coupon
    {
        public int Width { get; set; }
        public List<Line> Lines { get; set; } = new List<Line>();
        protected readonly BlockStyle BlockStyle;

        public Coupon(int width, BlockStyle blockStyle)
        {
            Width = width;
            BlockStyle = blockStyle;
        }

        public Line AddLine()
        {
            var line = new Line(Width, BlockStyle);
            Lines.Add(line);
            
            return line;
        }

        public Line AddEmptyLine()
        {
            var line = new Line(Width, BlockStyle);
            line.AddBlock(" ");
            Lines.Add(line);

            return line;
        }
    }
}