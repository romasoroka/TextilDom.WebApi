namespace Textildom.Application.Orders.Dtos
{
    public class CreateOrderResult
    {
        public OrderDto Order { get; set; } = null!;
        public string? PaymentUrl { get; set; }
    }
}
