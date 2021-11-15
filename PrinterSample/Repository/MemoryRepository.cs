using PrinterSample.Print.Samples;
using PrinterSample.Print.Samples.Sale;

namespace PrinterSample.Repository
{
    public static class MemoryRepository
    {
        public static readonly CompanyDTO Company = new CompanyDTO
        {
            Name = "Pizzaria Quero Mais",
            FooterMessage = "Volte sempre",
            Phone = "21 90900-1010"
        };

        public static readonly OrderDTO Order = new OrderDTO
        {
            ClientName = "Talles Santana",
            ClientPhone = "21 97278-1943",
            DeliveryAddress = "Rua Conde de Bonfim, 155 - Ap 101",
            DeliveryNeighborhood = "Tijuca",
            DeliveryFee = 10,
            DeliverymenName = "João",
            Discount = 1,
            Payments = new PaymentDTO[] {
                new PaymentDTO { Name = "DINHEIRO", Value = 10 },
                new PaymentDTO { Name = "CARTÃO DE CRÉDITO", Value = 40}
            },
            Items = new OrderItemDTO[]
            {
                new OrderItemDTO { ProductName = "Pizza Calabresa Especial Suprema do Chefe Média", 
                    Ammount = 1, UnityValue = 30, Obs = "Sem cebola"},
                new OrderItemDTO { ProductName = "Coca-Cola 2L", Ammount = 1, UnityValue = 8}
            },
            Total = 47
        };
    }
}
