namespace Textildom.Application.NovaPoshta.Commands
{
    public class CreateTtnCommand
    {
        public string PayerType { get; set; } = "Recipient";
        public string PaymentMethod { get; set; } = "Cash";
        public string ServiceType { get; set; } = "WarehouseWarehouse";
        public string CargoType { get; set; } = "Cargo";
        public string CitySenderRef { get; set; } = string.Empty;
        public string CityRecipientRef { get; set; } = string.Empty;
        public string WarehouseSenderRef { get; set; } = string.Empty;
        public string WarehouseRecipientRef { get; set; } = string.Empty;
        public string RecipientName { get; set; } = string.Empty;
        public string RecipientPhone { get; set; } = string.Empty;
        public string RecipientRef { get; set; } = string.Empty;
        public string ContactRecipientRef { get; set; } = string.Empty;
        public string SenderRef { get; set; } = string.Empty;
        public string ContactSenderRef { get; set; } = string.Empty;
        public string SendersPhone { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public decimal Weight { get; set; } = 0.1m;
        public int SeatsAmount { get; set; } = 1;
        public string Description { get; set; } = string.Empty;
    }
}