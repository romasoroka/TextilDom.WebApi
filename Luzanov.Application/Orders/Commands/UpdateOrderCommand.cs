namespace Luzanov.Application.Orders.Commands
{
    public class UpdateOrderCommand
    {
        public int Id { get; set; }
        public string CustomerFullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DeliveryMethod { get; set; } = string.Empty;
        public string? PostService { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;
        public string? Comment { get; set; }
    }
}
