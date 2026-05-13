using Textildom.Application.NovaPoshta.Commands;
using Textildom.Application.NovaPoshta.Dtos;
using Textildom.Application.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace Textildom.Application.Services
{
    public class NovaPoshtaService : INovaPoshtaService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public NovaPoshtaService(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _apiKey = configuration["NovaPoshta:ApiKey"] ?? string.Empty;
        }

        public async Task<IEnumerable<CityDto>> SearchCitiesAsync(string query)
        {
            var req = new
            {
                apiKey = _apiKey,
                modelName = "Address",
                calledMethod = "getCities",
                methodProperties = new { FindByString = query }
            };

            var res = await PostAsync<NovaResponse<CityDto>>(req);
            return res?.Data ?? Enumerable.Empty<CityDto>();
        }

        public async Task<IEnumerable<WarehouseDto>> GetWarehousesAsync(string cityRef)
        {
            var req = new
            {
                apiKey = _apiKey,
                modelName = "Address",
                calledMethod = "getWarehouses",
                methodProperties = new { CityRef = cityRef }
            };

            var res = await PostAsync<NovaResponse<WarehouseDto>>(req);
            return res?.Data ?? Enumerable.Empty<WarehouseDto>();
        }

        public async Task<(string RecipientRef, string ContactRef)> CreateRecipientAsync(
            string lastName, string firstName, string phone, string cityRef)
        {
            var req = new
            {
                apiKey = _apiKey,
                modelName = "Counterparty",
                calledMethod = "save",
                methodProperties = new
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Phone = phone,
                    CityRef = cityRef,
                    CounterpartyType = "PrivatePerson",
                    CounterpartyProperty = "Recipient"
                }
            };

            var res = await PostAsync<NovaResponse<JsonElement>>(req);
            if (res?.Success == true && res.Data?.Any() == true)
            {
                var d = res.Data.First();
                var recipientRef = d.GetProperty("Ref").GetString() ?? "";
                var contactRef = d.GetProperty("ContactPerson")
                                  .GetProperty("data")[0]
                                  .GetProperty("Ref").GetString() ?? "";
                return (recipientRef, contactRef);
            }
            return ("", "");
        }

        public async Task<NovaTtnResultDto> CreateShipmentAsync(CreateTtnCommand command)
        {
            // Важливо: для контролю оплати CargoType зазвичай Parcel.
            // Платник доставки - Recipient (Отримувач), Метод - Cash (Готівка)
            var payerType = "Recipient";
            var paymentMethod = "Cash";
            var cargoType = "Parcel";

            var methodProperties = new Dictionary<string, object>
            {
                ["PayerType"] = payerType,
                ["PaymentMethod"] = paymentMethod,
                ["ServiceType"] = command.ServiceType, // WarehouseWarehouse
                ["CargoType"] = cargoType,
                ["CitySender"] = command.CitySenderRef,
                ["CityRecipient"] = command.CityRecipientRef,
                ["SenderAddress"] = command.WarehouseSenderRef,
                ["RecipientAddress"] = command.WarehouseRecipientRef,
                ["Sender"] = command.SenderRef,
                ["ContactSender"] = command.ContactSenderRef,
                ["SendersPhone"] = command.SendersPhone,
                ["Recipient"] = command.RecipientRef,
                ["ContactRecipient"] = command.ContactRecipientRef,
                ["RecipientsPhone"] = command.RecipientPhone,
                ["Cost"] = command.Cost.ToString(),
                ["Weight"] = command.Weight.ToString(),
                ["SeatsAmount"] = "1",
                ["Description"] = command.Description,
                ["OptionsSeat"] = new[]
                {
            new
            {
                volumetricVolume = "1.88",
                weight = command.Weight.ToString(),
                volumetricWidth = "25",
                volumetricLength = "30",
                volumetricHeight = "10"
            }
        }
            };

            // ЛОГІКА ДЛЯ МАГАЗИНІВ (ПІСЛЯПЛАТА НА РАХУНОК)
            if (command.CashOnDelivery && command.CashOnDeliveryAmount > 0)
            {
                // 1. Прибираємо BackwardDeliveryData (вона для приватних осіб)
                // 2. Використовуємо вузол AfterpaymentOnGoodsCost
                methodProperties["AfterpaymentOnGoodsCost"] = command.CashOnDeliveryAmount.ToString();

                // 3. Деякі ФОП мають використовувати таку структуру PaymentControl
                // Якщо AfterpaymentOnGoodsCost не спрацює, НП очікує таке:
                /*
                methodProperties["PaymentControl"] = command.CashOnDeliveryAmount.ToString();
                */
            }

            var req = new
            {
                apiKey = _apiKey,
                modelName = "InternetDocument",
                calledMethod = "save",
                methodProperties
            };

            var res = await PostAsync<NovaResponse<object>>(req);

            if (res == null) return new NovaTtnResultDto { Success = false, Error = "Empty response" };

            if (res.Success && res.Data != null && res.Data.Any())
            {
                var first = res.Data.First();
                var json = JsonSerializer.Serialize(first);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                return new NovaTtnResultDto
                {
                    Success = true,
                    IntDocNumber = root.TryGetProperty("IntDocNumber", out var intProp) ? intProp.GetString() : null,
                    Ref = root.TryGetProperty("Ref", out var refProp) ? refProp.GetString() : null
                };
            }

            return new NovaTtnResultDto
            {
                Success = false,
                Error = res.Errors != null ? string.Join(", ", res.Errors) : "Unknown"
            };
        }

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = null
        };

        private async Task<T?> PostAsync<T>(object payload) where T : class
        {
            var response = await _http.PostAsJsonAsync(
                "https://api.novaposhta.ua/v2.0/json/",
                payload,
                _jsonOptions
            );
            if (!response.IsSuccessStatusCode) return null;

            var raw = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Nova response: " + raw); 

            return JsonSerializer.Deserialize<T>(raw,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private class NovaResponse<TData>
        {
            public bool Success { get; set; }
            public List<TData>? Data { get; set; }
            public List<string>? Errors { get; set; }
        }
    }
}