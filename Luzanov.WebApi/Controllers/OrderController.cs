using Luzanov.Application.Orders.Commands;
using Luzanov.Application.Orders.Dtos;
using Luzanov.Application.Services;
using Luzanov.Application.Services.Abstractions;
using Luzanov.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luzanov.WebApi.Controllers
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
        public async Task<IActionResult> MonoWebhook([FromBody] MonoWebhookPayload payload)
        {
            await _service.HandleMonoWebhookAsync(payload);
            return Ok(); // Mono чекає 200, інакше буде ретраїти
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