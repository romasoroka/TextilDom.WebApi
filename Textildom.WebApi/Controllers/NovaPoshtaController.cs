using Microsoft.AspNetCore.Mvc;
using Textildom.Application.NovaPoshta.Commands;
using Textildom.Application.Services.Abstractions;

namespace Textildom.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NovaPoshtaController : ControllerBase
    {
        private readonly INovaPoshtaService _nova;
        private readonly IConfiguration _configuration;

        public NovaPoshtaController(INovaPoshtaService nova, IConfiguration configuration)
        {
            _nova = nova;
            _configuration = configuration;
        }

        [HttpGet("cities")]
        public async Task<IActionResult> SearchCities([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q)) return BadRequest("Query is required");
            var cities = await _nova.SearchCitiesAsync(q);
            return Ok(cities);
        }

        [HttpGet("warehouses")]
        public async Task<IActionResult> GetWarehouses([FromQuery] string cityRef)
        {
            if (string.IsNullOrWhiteSpace(cityRef)) return BadRequest("cityRef is required");
            var wh = await _nova.GetWarehousesAsync(cityRef);
            return Ok(wh);
        }

        [HttpPost("create-ttn")]
        public async Task<IActionResult> CreateTtn([FromBody] CreateTtnCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Підставляємо дані відправника з конфігу
            command.SenderRef = _configuration["NovaPoshta:Sender:SenderRef"] ?? "";
            command.ContactSenderRef = _configuration["NovaPoshta:Sender:ContactSenderRef"] ?? "";
            command.CitySenderRef = _configuration["NovaPoshta:Sender:CitySenderRef"] ?? "";
            command.WarehouseSenderRef = _configuration["NovaPoshta:Sender:WarehouseSenderRef"] ?? "";
            command.SendersPhone = _configuration["NovaPoshta:Sender:SendersPhone"] ?? "";

            var nameParts = command.RecipientName.Trim().Split(' ');
            var (recipientRef, contactRef) = await _nova.CreateRecipientAsync(
                lastName: nameParts.ElementAtOrDefault(0) ?? "",
                firstName: nameParts.ElementAtOrDefault(1) ?? "",
                phone: command.RecipientPhone,
                cityRef: command.CityRecipientRef
            );

            command.RecipientRef = recipientRef;
            command.ContactRecipientRef = contactRef;

            var res = await _nova.CreateShipmentAsync(command);
            if (!res.Success) return BadRequest(new { message = res.Error });

            return Ok(res);
        }
    }
}
