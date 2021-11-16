using System.Drawing;
using System.Drawing.Printing;
using PrinterLib.Drawer;

namespace PrinterLib
{
    public class CouponPrintDocument : PrintDocument
    {
        private readonly Coupon _coupon;
        private readonly ICouponDrawer _drawer;

        public int Width => (int)this.DefaultPageSettings.PrintableArea.Width;

        public CouponPrintDocument(Coupon coupon, ICouponDrawer drawer)
        {
            _coupon = coupon;
            _drawer = drawer;

            this.DefaultPageSettings.Landscape = false;
            this.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            this.OriginAtMargins = true;
        }

        public new void Print()
        {
            _coupon.Build();
            base.Print();
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            base.OnPrintPage(e);
            var area = Rectangle.Round(e.PageSettings.PrintableArea);
            var drawResult = _drawer.Draw(_coupon, e.Graphics, area);
            e.HasMorePages = drawResult.WillCrossArea;
        }
    }
}