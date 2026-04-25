namespace Luzanov.Application.Services.Abstractions
{
    public interface IMonoService
    {
        Task<MonoInvoiceResult> CreateInvoiceAsync(int orderId, decimal amount, string comment);
    }

    public class MonoInvoiceResult
    {
        public bool Success { get; set; }
        public string? InvoiceId { get; set; }
        public string? PageUrl { get; set; }
        public string? Error { get; set; }
    }
}
