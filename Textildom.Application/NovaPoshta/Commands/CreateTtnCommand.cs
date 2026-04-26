namespace Textildom.Application.NovaPoshta.Commands
{
    public class CreateTtnCommand
    {
        // Дефолти для штор/тюлю
        public string ServiceType { get; set; } = "WarehouseWarehouse";
        public string CargoType { get; set; } = "Cargo";
        public decimal Weight { get; set; } = 0.5m;
        public int SeatsAmount { get; set; } = 1;
        public string Description { get; set; } = "Штори, тюль";

        // Оплата — за замовчуванням без накладеного (оплата онлайн)
        public bool CashOnDelivery { get; set; } = false;
        public decimal CashOnDeliveryAmount { get; set; } = 0;

        // Отримувач — приходить з форми
        public string CityRecipientRef { get; set; } = string.Empty;
        public string WarehouseRecipientRef { get; set; } = string.Empty;
        public string RecipientName { get; set; } = string.Empty;
        public string RecipientPhone { get; set; } = string.Empty;

        // Заповнюється в контролері автоматично
        public string RecipientRef { get; set; } = string.Empty;
        public string ContactRecipientRef { get; set; } = string.Empty;

        // Відправник — заповнюється з appsettings в контролері
        public string CitySenderRef { get; set; } = string.Empty;
        public string WarehouseSenderRef { get; set; } = string.Empty;
        public string SenderRef { get; set; } = string.Empty;
        public string ContactSenderRef { get; set; } = string.Empty;
        public string SendersPhone { get; set; } = string.Empty;

        public decimal Cost { get; set; }
    }
}