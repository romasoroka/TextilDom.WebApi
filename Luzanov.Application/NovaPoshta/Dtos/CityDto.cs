using System.Text.Json.Serialization;

namespace Luzanov.Application.NovaPoshta.Dtos
{
    public class CityDto
    {
        public string Ref { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class SearchCityData
    {
        [JsonPropertyName("TotalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("Addresses")]
        public List<CityAddressItem> Addresses { get; set; }
    }

    public class CityAddressItem
    {
        [JsonPropertyName("MainDescription")]
        public string MainDescription { get; set; }

        [JsonPropertyName("DeliveryCity")]
        public string DeliveryCity { get; set; }

        [JsonPropertyName("Present")]
        public string Present { get; set; }
    }

    public class WarehouseDto
    {
        public string Ref { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class NovaTtnResultDto
    {
        public bool Success { get; set; }
        public string? IntDocNumber { get; set; }
        public string? Ref { get; set; }
        public string? Error { get; set; }
    }
}
