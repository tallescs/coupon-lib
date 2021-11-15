using System.Collections.Generic;
using System.Linq;

namespace PrinterSample.Print.Samples.Sale
{
    public class OrderDTO
    {
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryNeighborhood { get; set; }
        public string DeliverymenName { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal DeliveryFee { get; set; }
        public IEnumerable<OrderItemDTO> Items;
        public IEnumerable<PaymentDTO> Payments { get; set; }

        public decimal GetChange()
        {
            if (!Payments.Any())
                return 0;

            return Payments.Sum(p => p.Value) - Total;
        }
    }

    public class OrderItemDTO
    {
        public string ProductName { get; set; }
        public int UnityValue { get; set; }
        public int Ammount { get; set; }
        public double Discount { get; set; }
        public double TotalValue => (UnityValue * Ammount) - Discount;
        public string Obs { get; set; }
    }
}