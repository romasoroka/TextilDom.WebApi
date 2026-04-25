using Textildom.Domain.Models;

namespace Textildom.Application.Orders.Commands
{
    public class CreateOrderCommand
    {
        public string CustomerFullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string CityRef { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string WarehouseRef { get; set; } = string.Empty;
        public string WarehouseAddress { get; set; } = string.Empty;
        public string? Comment { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}