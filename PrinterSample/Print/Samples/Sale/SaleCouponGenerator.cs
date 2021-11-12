using System.Collections.Generic;
using System.Linq;

namespace PrinterSample.Print.Samples
{
    public class SaleCouponGenerator : CouponGenerator
    {
        private readonly OrderDTO _order;
        private readonly CompanyDTO _company;
        private const int NAME_PERCENT_WIDTH = 70;
        private const int VALUE_PERCENT_WIDTH = 30;

        public SaleCouponGenerator(SaleCouponData couponData, CouponStyle style) : base(style)
        {
            _order = couponData.Order;
            _company = couponData.Company;
        }

        public override Coupon CreateCoupon()
        {
            var lines = new List<Line>();
            lines.AddRange(GenerateHeader());
            lines.AddRange(GenerateClientInfo());
            lines.AddRange(GenerateDeliveryInfo());
            lines.AddRange(GenerateOrderItemsInfo());
            lines.AddRange(GenerateTotals());
            lines.AddRange(GeneratePayments());
            lines.AddRange(GenerateFooter());

            return new Coupon { Lines = lines };
        }

        private IEnumerable<Line> GenerateOrderItemsInfo()
        {
            yield return GenerateHeaderLine(" Itens do Pedido ");

            var itemsTitle = new Line(_documentWidth);
            var itemColumn = GenerateBlock("Item", 50);
            itemsTitle.AddBlock(itemColumn);

            var priceColumn = GenerateBlock("Preço", _rightFormat, 50);
            itemsTitle.AddBlock(priceColumn);

            yield return itemsTitle;
            yield return GenerateBlankLine();

            foreach (var item in _order.Items)
            {
                var itemLines = GenerateItem(item).ToList();

                foreach (var it in itemLines)
                    yield return it;

                var block = GenerateFillerBlock("-", " -", _centerFormat);
                yield return GenerateLine(block);
                yield return GenerateBlankLine();
            }
        }

        private IEnumerable<Line> GenerateItem(OrderItemDTO item)
        {
            foreach (var line in GenerateNameLines(item))
                yield return line;

            if (!string.IsNullOrEmpty(item.Obs))
            {
                var obsText = $"Obs: {item.Obs}";
                var obsBlock = GenerateBlock(obsText);
                yield return GenerateLine(obsBlock);
            }

            yield return GenerateBlankLine();

            var ammountBlock = GenerateBlock($"Quantidade: {item.Ammount}", 50);
            var itemTotalStr = $"Total: R$ {item.TotalValue:F2}";
            var totalBlock = GenerateBlock(itemTotalStr, _rightFormat, 50);

            var summaryLine = new Line(_documentWidth);
            summaryLine.AddBlock(ammountBlock);
            summaryLine.AddBlock(totalBlock);
            yield return summaryLine;
        }

        private IEnumerable<Line> GenerateNameLines(OrderItemDTO item)
        {
            var itemPrice = item.UnityValue * item.Ammount;
            var line = GenerateNamePriceBlock(item.ProductName.Trim(),
                itemPrice, bold: true, blockSpacing: BlockSpacing.Medium);
            yield return line;
        }

        private Line GenerateNamePriceBlock(string name,
            double price, bool bold = false, BlockSpacing blockSpacing = BlockSpacing.Narrow)
        {
            var line = new Line(_documentWidth);

            var nameFont = bold ? _boldFont : _defaultFont;
            var nameBlock = GenerateBlock(name, nameFont, NAME_PERCENT_WIDTH, blockSpacing);
            line.AddBlock(nameBlock);

            if (price > 0)
            {
                var priceStr = $"R$ {price:F2}";
                var priceBlock = GenerateBlock(priceStr, _rightFormat, VALUE_PERCENT_WIDTH);
                line.AddBlock(priceBlock);
            }
            return line;
        }

        private IEnumerable<Line> GenerateClientInfo()
        {
            if (!string.IsNullOrEmpty(_order.ClientName))
            {
                yield return GenerateHeaderLine(" Dados do Cliente ");

                var clientName = $"Nome: {_order.ClientName}";
                yield return GenerateLine(clientName);

                var clientPhone = $"Telefone: {_order.ClientPhone}";
                yield return GenerateLine(clientPhone);

                yield return GenerateBlankLine();
            }
        }

        private IEnumerable<Line> GenerateDeliveryInfo()
        {
            yield return GenerateHeaderLine(" Dados da Entrega ");

            var address = $"Endereço: {_order.DeliveryAddress}";
            yield return GenerateLine(address);

            var neighbourhood = $"{_order.DeliveryNeighborhood}";
            yield return GenerateLine(neighbourhood);

            if (!string.IsNullOrEmpty(_order.DeliverymenName))
            {
                yield return GenerateBlankLine();
                yield return GenerateLine($"ENTREGADOR: {_order.DeliverymenName}");
            }

            yield return GenerateBlankLine();
        }

        private Line GenerateHeaderLine(string text)
        {
            const string filler = "=";
            var headerBlock = GenerateFillerBlock(text, filler, _centerFormat, fillPosition: FillPosition.Both);
            return GenerateLine(headerBlock);
        }

        private IEnumerable<Line> GenerateHeader()
        {
            var nameBlock = GenerateBlock(_company.Name, _boldFont, _centerFormat);
            var phoneBlock = GenerateBlock(_company.Phone, _centerFormat);

            yield return GenerateLine(nameBlock);
            yield return GenerateLine(phoneBlock);
            yield return GenerateBlankLine();
        }

        private IEnumerable<Line> GenerateTotals()
        {
            yield return GenerateSubtotalLine();

            foreach (var feeLine in GenerateDeliveryFeeLines())
                yield return feeLine;

            foreach (var discountLine in GenerateDiscountLines())
                yield return discountLine;

            yield return GenerateTotalLine();
        }

        private Line GenerateSubtotalLine()
        {
            var itemsTotal = _order.Items.Sum(p => p.TotalValue);

            var subTotalLabel = $"(+) Subtotal ";
            var subTotalValue = $"R$ {itemsTotal:F2}";
            return GenerateTotalizerLine(subTotalLabel, subTotalValue);
        }

        private Line GenerateTotalizerLine(string labelText, string valueText)
        {
            var labelBlock = GenerateFillerBlock(labelText, ".", NAME_PERCENT_WIDTH);

            var valueBlock = GenerateBlock(valueText, _rightFormat, VALUE_PERCENT_WIDTH);

            var line = new Line(_documentWidth);
            line.AddBlock(labelBlock);
            line.AddBlock(valueBlock);
            return line;
        }

        private IEnumerable<Line> GenerateDeliveryFeeLines()
        {
            if (_order.DeliveryFee > 0)
            {
                var feeLabel = $"(+) Taxa de Entrega";
                var feeValue = $"R$ {_order.DeliveryFee:F2}";
                yield return GenerateTotalizerLine(feeLabel, feeValue);
            }
        }

        private IEnumerable<Line> GenerateDiscountLines()
        {
            if (_order.Discount > 0)
            {
                var discountLabel = $"(-) Desconto";
                var discountValue = $"R$ {_order.Discount:F2}";
                yield return GenerateTotalizerLine(discountLabel, discountValue);
            }
        }

        private Line GenerateTotalLine()
        {
            var label = $"(=) Valor Total ";
            var value = $"R$ {_order.Total:F2}";
            return GenerateTotalizerLine(label, value);
        }

        private IEnumerable<Line> GeneratePayments()
        {
            if (_order.Payments.Any())
            {
                yield return GenerateHeaderLine(" Forma de Pagamento ");

                foreach (var paymentLine in GeneratePaymentLines())
                    yield return paymentLine;
            }
        }

        private IEnumerable<Line> GeneratePaymentLines()
        {
            foreach (var payment in _order.Payments)
            {
                yield return GeneratePaymentValueLine(payment.Name, $"R$ {payment.Value:F2}");
            }

            var paymentsTotal = _order.Payments.Sum(p => p.Value);
            if (paymentsTotal > _order.Total)
            {
                var diff = paymentsTotal - _order.Total;
                yield return GeneratePaymentValueLine("Valor do Troco:", $"R$ {diff:F2}", bold: true);
            }
        }

        private Line GeneratePaymentValueLine(string name, string value, bool bold = false)
        {
            var nameBlock = GenerateBlock(name, NAME_PERCENT_WIDTH);

            var valueFont = bold ? _boldFont : _defaultFont;
            var valueBlock = GenerateBlock(value, valueFont, _rightFormat, VALUE_PERCENT_WIDTH);

            var line = new Line(_documentWidth);
            line.AddBlock(nameBlock);
            line.AddBlock(valueBlock);
            return line;
        }

        private IEnumerable<Line> GenerateFooter()
        {
            string footerText = _company.FooterMessage;

            var separatorBlock = GenerateFillerBlock("=", "=");
            yield return GenerateLine(separatorBlock);

            if (!string.IsNullOrEmpty(footerText))
            {
                var thanksblock = GenerateBlock(footerText, _centerFormat);
                yield return GenerateLine(thanksblock);
            }

            yield return GenerateBlankLine();
            yield return GenerateBlankLine();
        }
    }
}