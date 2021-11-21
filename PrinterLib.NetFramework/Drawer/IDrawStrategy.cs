using System.Drawing;

namespace PrinterLib.Drawer
{
    public interface IDrawStrategy
    {
        DrawResult Draw(Coupon coupon, Graphics graphics, Rectangle area);
    }
}
