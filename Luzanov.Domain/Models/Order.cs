using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Luzanov.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerFullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DeliveryMethod { get; set; } = string.Empty; // "Пошта" або "Самовивіз"
        public string? PostService { get; set; } // Нова пошта, Укрпошта, Містекспрес, Делівері, Наш сервіс доставки
        public string DeliveryAddress { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty; // Готівка, Картка, Оплата онлайн, Накладений платіж
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } = "Нове"; // Нове, В обробці, Відправлено, Виконано, Скасовано
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Comment { get; set; } // Коментар до замовлення
        
        // JSON для збереження списку продуктів
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
