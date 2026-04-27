using AutoMapper;
using Textildom.Application.IRepositories;
using Textildom.Application.Orders.Commands;
using Textildom.Application.Orders.Dtos;
using Textildom.Application.Services.Abstractions;
using Textildom.Domain.Models;
using System.Text;

namespace Textildom.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        private readonly ITelegramBotService _telegramBot;
        private readonly IMonoService _mono;

        public OrderService(
            IOrderRepository orderRepo,
            IMapper mapper,
            ITelegramBotService telegramBot,
            IMonoService mono)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
            _telegramBot = telegramBot;
            _mono = mono;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _orderRepo.GetAllOrdersAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _orderRepo.GetOrderByIdAsync(id);
            return order == null ? null : _mapper.Map<OrderDto>(order);
        }

        public async Task<CreateOrderResult> CreateAsync(CreateOrderCommand command)
        {
            var order = _mapper.Map<Order>(command);
            order.OrderItems = command.OrderItems;
            order.TotalAmount = command.OrderItems.Sum(item => item.Price * item.Quantity);
            order.OrderStatus = "Нове";
            order.PaymentType = command.PaymentType;
            order.CreatedAt = DateTime.UtcNow;

            order.PaymentStatus = command.PaymentType == "IBAN"
                ? "Очікує переказу"
                : "Очікує оплати";

            await _orderRepo.AddAsync(order);

            string? paymentUrl = null;

            if (command.PaymentType == "Online")
            {
                var invoiceComment = $"Замовлення #{order.Id}";
                var invoice = await _mono.CreateInvoiceAsync(order.Id, order.TotalAmount, invoiceComment);

                if (invoice.Success && invoice.InvoiceId != null)
                {
                    order.MonoInvoiceId = invoice.InvoiceId;
                    await _orderRepo.UpdateAsync(order);
                    paymentUrl = invoice.PageUrl;
                }
            }

            var telegramMessage = FormatOrderMessage(order, paymentUrl);
            await _telegramBot.SendOrderNotificationAsync(telegramMessage);

            return new CreateOrderResult
            {
                Order = _mapper.Map<OrderDto>(order),
                PaymentUrl = paymentUrl,
            };
        }

        public async Task<bool> HandleMonoWebhookAsync(MonoWebhookPayload payload)
        {
            Console.WriteLine($"Mono webhook: status={payload.Status}, ref={payload.Reference}");

            if (!int.TryParse(payload.Reference, out var orderId))
                return false;

            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null) return false;

            order.PaymentStatus = payload.Status switch
            {
                "success" => "Оплачено",
                "failure" or "reversed" => "Скасовано",
                _ => order.PaymentStatus
            };

            if (order.PaymentStatus == "Оплачено")
                order.OrderStatus = "В обробці";

            await _orderRepo.UpdateAsync(order);
            return true;
        }

        public async Task<bool> UpdateAsync(UpdateOrderCommand command)
        {
            var existing = await _orderRepo.GetByIdAsync(command.Id);
            if (existing == null) return false;
            _mapper.Map(command, existing);
            return await _orderRepo.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return false;
            return await _orderRepo.RemoveAsync(order);
        }

        private static string FormatOrderMessage(Order order, string? paymentUrl)
        {
            var sb = new StringBuilder();

            sb.AppendLine("🛍️ <b>НОВЕ ЗАМОВЛЕННЯ</b>");
            sb.AppendLine("────────────────────");
            sb.AppendLine($"🆔 <b>Номер:</b> <code>#{order.Id}</code>");
            sb.AppendLine($"👤 <b>Замовник:</b> {order.CustomerFullName}");
            sb.AppendLine($"📞 <b>Телефон:</b> {order.PhoneNumber}");
            sb.AppendLine();

            sb.AppendLine("🚚 <b>ДОСТАВКА (Нова Пошта)</b>");
            sb.AppendLine($"🏙️ <b>Місто:</b> {order.CityName}");
            sb.AppendLine($"📦 <b>Відділення:</b> {order.WarehouseAddress}");
            sb.AppendLine();

            sb.AppendLine("🛒 <b>ТОВАРИ:</b>");
            foreach (var item in order.OrderItems)
            {
                sb.AppendLine($"🔹 <b>{item.ProductName}</b>");
                sb.AppendLine($"   └ {item.Size} | {item.Colour} | {item.Quantity} шт. × {item.Price:N0} грн");
            }

            sb.AppendLine("────────────────────");
            sb.AppendLine($"💰 <b>ЗАГАЛЬНА СУМА: {order.TotalAmount:N2} грн</b>");

            if (!string.IsNullOrEmpty(paymentUrl))
                sb.AppendLine($"💳 <b>Оплата:</b> {paymentUrl}");

            sb.AppendLine();
            sb.AppendLine($"📅 <i>{order.CreatedAt:dd.MM.yyyy HH:mm}</i>");

            return sb.ToString();
        }
    }
}