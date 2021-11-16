using PrinterLib;
using PrinterLib.Drawer;
using PrinterLib.Samples.Sale;
using PrinterSample.Repository;
using System;
using System.Drawing.Printing;
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

            var printerWidth = (int) new PrintDocument().DefaultPageSettings.PrintableArea.Width;

            var saleCoupon = new SaleCoupon(printerWidth, new DefaultBlockStyle(), data);
            var printDocument = new CouponPrintDocument(saleCoupon, new DefaultDrawer());

            Print(printDocument);
        }

        public void Print(CouponPrintDocument couponPrint, 
            string printerName = null, bool preview = false)
        {
            if (!string.IsNullOrWhiteSpace(printerName))
            {
                couponPrint.PrinterSettings.PrinterName = printerName;
            }
            else if (!preview)
            {
                using (var dialog = new PrintDialog { UseEXDialog = true, Document = couponPrint })
                {
                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;
                }
            }

            if (preview)
            {
                using (var dialog = new PrintPreviewDialog { Document = couponPrint })
                    dialog.ShowDialog();
            }
            else
            {
                couponPrint.Print();
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            ucCouponPreview.FillWithOrder(MemoryRepository.Order, MemoryRepository.Company);
        }
    }
}
