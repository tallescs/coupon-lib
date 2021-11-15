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

            Print(printDocument, saleCoupon);
        }

        public void Print(CouponPrintDocument printDocument, Coupon coupon,
            string printerName = null, bool preview = false)
        {
            if (!string.IsNullOrWhiteSpace(printerName))
            {
                printDocument.PrinterSettings.PrinterName = printerName;
            }
            else if (!preview)
            {
                using (var dialog = new PrintDialog { UseEXDialog = true, Document = printDocument })
                {
                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;
                }
            }

            if (preview)
            {
                using (var dialog = new PrintPreviewDialog { Document = printDocument })
                    dialog.ShowDialog();
            }
            else
            {
                printDocument.Print(coupon);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            ucCouponPreview.FillWithOrder(MemoryRepository.Order, MemoryRepository.Company);
        }
    }
}
