using Textildom.Application.NovaPoshta.Commands;
using Textildom.Application.NovaPoshta.Dtos;
using Textildom.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Textildom.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NovaPoshtaController : ControllerBase
    {
        private readonly INovaPoshtaService _nova;

        public NovaPoshtaController(INovaPoshtaService nova)
        {
            _nova = nova;
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
