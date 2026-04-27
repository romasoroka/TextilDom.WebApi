namespace Textildom.Application.Orders.Dtos
{
    public class MonoWebhookPayload
    {
        public string? InvoiceId { get; set; }
        public string? Status { get; set; }
        public long Amount { get; set; }
        public string? Reference { get; set; }
        public string? Destination { get; set; }
    }
}
