namespace Luzanov.Application.Orders.Commands
{
    public class UpdateOrderCommand
    {
        public int Id { get; set; }
        public string CustomerFullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string CityRef { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string WarehouseRef { get; set; } = string.Empty;
        public string WarehouseAddress { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;
        public string? Comment { get; set; }
    }
}