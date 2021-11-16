using System.Drawing;

namespace PrinterLib.Drawer
{
    public interface ICouponDrawer
    {
        DrawResult Draw(Coupon coupon, Graphics graphics, Rectangle area);
    }
}
