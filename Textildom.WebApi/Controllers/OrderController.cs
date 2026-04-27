using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Textildom.Application.Orders.Commands;
using Textildom.Application.Orders.Dtos;
using Textildom.Application.Services;
using Textildom.Application.Services.Abstractions;
using Textildom.Domain.Constants;

namespace Textildom.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _service.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest("Id must be greater than 0");
            var order = await _service.GetByIdAsync(id);
            return order != null ? Ok(order) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _service.CreateAsync(command);

            return CreatedAtAction(nameof(GetById), new { id = result.Order.Id }, new
            {
                order = result.Order,
                paymentUrl = result.PaymentUrl
            });
        }

        // Mono викликає цей endpoint після оплати
        [HttpPost("mono-webhook")]
        public async Task<IActionResult> MonoWebhook([FromBody] JsonElement rawPayload)
        {
            Console.WriteLine($"=== MONO WEBHOOK HIT ===");
            Console.WriteLine(rawPayload.ToString());

            var payload = System.Text.Json.JsonSerializer.Deserialize<MonoWebhookPayload>(
                rawPayload.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            await _service.HandleMonoWebhookAsync(payload!);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        public async Task<IActionResult> Update([FromBody] UpdateOrderCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.UpdateAsync(command);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Id must be greater than 0");
            var result = await _service.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}