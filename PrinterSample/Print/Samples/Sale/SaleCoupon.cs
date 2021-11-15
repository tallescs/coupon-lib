using System;
using System.Drawing;
using System.Linq;

namespace PrinterSample.Print.Samples.Sale
{
    public class SaleCoupon : Coupon
    {
        public SaleCouponData CouponData { get; }
        private BlockStyle BlockStyleBold { get; set; }
        private BlockStyle BlockStyleBoldCenter { get; set; }
        public BlockStyle RightStyle { get; set; }

        private const int NAME_PERCENT_WIDTH = 70;
        private const int PRICE_PERCENT_WIDTH = 30;
        private const int FIFTY_PERCENT_WIDTH = 50;

        public SaleCoupon(int width, BlockStyle blockStyle,
            SaleCouponData data) : base(width, blockStyle)
        {
            CouponData = data;
            BlockStyleBold = blockStyle.Copy();
            BlockStyleBold.Font = new Font(prototype: BlockStyleBold.Font, FontStyle.Bold);

            BlockStyleBoldCenter = blockStyle.Copy();
            BlockStyleBoldCenter.Font = new Font(prototype: BlockStyleBoldCenter.Font, FontStyle.Bold);
            BlockStyleBoldCenter.StringFormat = new StringFormat(BlockStyleBoldCenter.StringFormat)
            {
                Alignment = StringAlignment.Center
            };

            RightStyle = blockStyle.Copy();
            RightStyle.StringFormat = new StringFormat(BlockStyleBoldCenter.StringFormat)
            {
                Alignment = StringAlignment.Far
            };
        }

        public void Build()
        {
            BuildHeader();
            BuildClientInfo();
            BuildDeliveryInfo();
            BuildOrdersInfo();
            BuildTotals();
            BuildPayments();
            BuildFooter();
        }

        private void BuildHeader()
        {
            AddLine().AddBlock(CouponData.Company.Name, 100, BlockStyleBoldCenter);
            AddLine().AddBlock(CouponData.Company.Phone, 100, BlockStyleBoldCenter);
            AddEmptyLine();
        }

        private void BuildClientInfo()
        {
            if (!string.IsNullOrEmpty(CouponData.Order.ClientName))
            {
                var block = GenerateFillerBlock(" Dados do Cliente ", "=", 100);
                AddLine().AddBlock(block);

                AddLine().AddBlock($"Nome: {CouponData.Order.ClientName}");
                AddLine().AddBlock($"Telefone: {CouponData.Order.ClientPhone}");
                AddEmptyLine();
            }
        }

        private void BuildDeliveryInfo()
        {
            var block = GenerateFillerBlock(" Dados da Entrega ", "=", 100);
            AddLine().AddBlock(block);

            AddLine().AddBlock($"Endereço: {CouponData.Order.DeliveryAddress}");
            AddLine().AddBlock(CouponData.Order.DeliveryNeighborhood);

            if (!string.IsNullOrEmpty(CouponData.Order.DeliverymenName))
            {
                AddEmptyLine();
                AddLine().AddBlock($"ENTREGADOR: {CouponData.Order.DeliverymenName}");
            }
            AddEmptyLine();
        }

        private void BuildOrdersInfo()
        {
            AddSectionLine("Itens do Pedido");

            var title = AddLine();
            title.AddBlock("Item", FIFTY_PERCENT_WIDTH, BlockStyle);
            title.AddBlock("preco", FIFTY_PERCENT_WIDTH, BlockStyle);

            AddEmptyLine();
            foreach (var item in CouponData.Order.Items)
            {
                AddNamePriceLine(item.ProductName, (decimal)item.TotalValue);
                if (!string.IsNullOrEmpty(item.Obs))
                {
                    AddLine().AddBlock(item.Obs);
                }

                AddEmptyLine();

                var totalLine = AddLine();
                totalLine.AddBlock($"Quantidade: {item.Ammount}", FIFTY_PERCENT_WIDTH, BlockStyle);
                totalLine.AddBlock($"Total: R$ {item.TotalValue:F2}", FIFTY_PERCENT_WIDTH, RightStyle);

                AddSeparatorLine();
                AddEmptyLine();
            }
        }

        private void BuildTotals()
        {
            var subTotalLabel = "(+) Subtotal ";
            var itemsTotal = CouponData.Order.Items.Sum(p => p.TotalValue);
            AddTotalizerLine(subTotalLabel, (decimal) itemsTotal);

            if (CouponData.Order.DeliveryFee > 0)
                AddTotalizerLine("(+) Taxa de Entrega", CouponData.Order.DeliveryFee);

            if (CouponData.Order.Discount > 0)
                AddTotalizerLine("(-) Desconto", CouponData.Order.Discount);

            AddTotalizerLine("(=) Valor Total", CouponData.Order.Total);
        }

        private void BuildPayments()
        {
            var payments = CouponData.Order.Payments;
            if (payments.Any())
            {
                AddSectionLine(" Forma de Pagamento ");

                foreach (var payment in payments)
                    AddNamePriceLine(payment.Name, payment.Value);

                var change = CouponData.Order.GetChange();
                if (change > 0)
                    AddNamePriceLine("Valor do Troco:", change);
            }
        }

        private void BuildFooter()
        {
            AddSeparatorLine();

            if(!string.IsNullOrEmpty(CouponData.Company.FooterMessage))
            {
                AddLine().AddBlock(CouponData.Company.FooterMessage);
            }

            AddEmptyLine();
            AddEmptyLine();
        }

        private void AddTotalizerLine(string text, decimal value)
        {
            var line = AddLine();
            line.AddBlock(GenerateFillerBlock(text, ".", NAME_PERCENT_WIDTH, BlockFillPosition.Right));
            line.AddBlock($"R$ {value:F2}", PRICE_PERCENT_WIDTH, RightStyle);
        }
        private Block GenerateFillerBlock(string text, string filler, int widthPercent,
           BlockFillPosition fillPosition = BlockFillPosition.Both)
        {
            decimal adjust = (decimal)widthPercent / 100;
            int blockWidth = (int)Math.Floor(adjust * Width);
            return new FillerBlock(text, blockWidth, BlockStyle, filler, fillPosition);
        }

        private void AddNamePriceLine(string label, decimal price)
        {
            var line = AddLine();
            line.AddBlock(label, NAME_PERCENT_WIDTH, BlockStyleBold);
            line.AddBlock($"R$ {price:F2}", PRICE_PERCENT_WIDTH, RightStyle);
        }

        private void AddSeparatorLine()
        {
            AddLine().AddBlock(GenerateFillerBlock("-", " -", 100));
        }

        private void AddSectionLine(string text)
        {
            var block = GenerateFillerBlock(text, "=", 100);
            AddLine().AddBlock(block);
        }
    }
}