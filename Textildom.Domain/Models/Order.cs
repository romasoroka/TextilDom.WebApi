using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Textildom.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerFullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // НП: місто і відділення
        public string CityRef { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string WarehouseRef { get; set; } = string.Empty;
        public string WarehouseAddress { get; set; } = string.Empty;

        // Оплата через Mono
        public string? MonoInvoiceId { get; set; }
        public string PaymentStatus { get; set; } = "Очікує оплати"; 

        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } = "Нове";
        public string PaymentType { get; set; } = "Online";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Comment { get; set; }

        public string OrderItemsJson { get; set; } = string.Empty;

        [NotMapped]
        public List<OrderItem> OrderItems
        {
            get => string.IsNullOrEmpty(OrderItemsJson)
                ? new List<OrderItem>()
                : JsonSerializer.Deserialize<List<OrderItem>>(OrderItemsJson)!;
            set => OrderItemsJson = JsonSerializer.Serialize(value ?? new List<OrderItem>());
        }
    }

    public class OrderItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; } = string.Empty;
        public string Colour { get; set; } = string.Empty;
    }
}