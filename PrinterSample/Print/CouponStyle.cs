using System.Drawing;
using System.Drawing.Printing;

namespace PrinterSample.Print
{
    public abstract class CouponStyle
    {
        public Font DefaultFont { get; set; }
        public Font BoldFont { get; set; }
        public Margins Margins { get; set; }
        public int CouponWidth { get; set; }
    }
}