using System.Drawing;
using System.Drawing.Printing;

namespace PrinterSample.Print
{
    public class DefaultCouponStyle : CouponStyle
    {
        public DefaultCouponStyle(int couponWidth)
        {
            DefaultFont = new Font("Courier New", 10, FontStyle.Regular);
            BoldFont = new Font("Courier New", 10, FontStyle.Bold);
            Margins = new Margins(2, 2, 2, 2);
            CouponWidth = couponWidth;
        }
    }
}