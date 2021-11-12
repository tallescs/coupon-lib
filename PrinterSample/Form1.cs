using PrinterSample.Print;
using PrinterSample.Print.Samples;
using PrinterSample.Repository;
using System;
using System.Windows.Forms;

namespace PrinterSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            var data = new SaleCouponData
            {
                Company = MemoryRepository.Company,
                Order = MemoryRepository.Order,
            };

            var printDocument = new CouponPrintDocument();
            var style = new DefaultCouponStyle(printDocument.Width);

            var generator = new SaleCouponGenerator(data, style);

            var coupon = generator.CreateCoupon();

            printDocument.Print(coupon);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            ucCouponPreview.FillWithOrder(MemoryRepository.Order, MemoryRepository.Company);
        }
    }
}
