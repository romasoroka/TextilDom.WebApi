using Luzanov.Application.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace Luzanov.Application.Services
{
    public class MonoService : IMonoService
    {
        private readonly HttpClient _http;
        private readonly string _token;
        private readonly string _webhookUrl;
        private readonly string _redirectUrl;

        public MonoService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _token = config["Mono:Token"] ?? throw new InvalidOperationException("Mono:Token not configured");
            _webhookUrl = config["Mono:WebhookUrl"] ?? "";
            _redirectUrl = config["Mono:RedirectUrl"] ?? "";
        }

        public async Task<MonoInvoiceResult> CreateInvoiceAsync(int orderId, decimal amount, string comment)
        {
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("X-Token", _token);

            var body = new
            {
                amount = (long)(amount * 100), 
                ccy = 980,                      
                merchantPaymInfo = new
                {
                    reference = orderId.ToString(),
                    destination = comment,
                },
                redirectUrl = _redirectUrl,
                webHookUrl = _webhookUrl,
            };

            var response = await _http.PostAsJsonAsync("https://api.monobank.ua/api/merchant/invoice/create", body);
            var raw = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return new MonoInvoiceResult { Success = false, Error = raw };

            using var doc = JsonDocument.Parse(raw);
            var root = doc.RootElement;

            return new MonoInvoiceResult
            {
                Success = true,
                InvoiceId = root.GetProperty("invoiceId").GetString(),
                PageUrl = root.GetProperty("pageUrl").GetString(),
            };
        }
    }
}
