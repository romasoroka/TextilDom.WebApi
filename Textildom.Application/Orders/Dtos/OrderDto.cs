namespace Textildom.Application.Orders.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string CustomerFullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string CityRef { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string WarehouseRef { get; set; } = string.Empty;
        public string WarehouseAddress { get; set; } = string.Empty;

        public string? MonoInvoiceId { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? Comment { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; } = string.Empty;
        public string Colour { get; set; } = string.Empty;
    }
}