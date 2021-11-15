using PrinterLib;
using PrinterLib.Samples.Sale;
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

            var saleCoupon = new SaleCoupon(printDocument.Width, new DefaultBlockStyle(), data);
            saleCoupon.Build();

            printDocument.Print(saleCoupon);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            ucCouponPreview.FillWithOrder(MemoryRepository.Order, MemoryRepository.Company);
        }
    }
}
