using AutoMapper;
using Luzanov.Application.IRepositories;
using Luzanov.Application.Orders.Commands;
using Luzanov.Application.Orders.Dtos;
using Luzanov.Application.Services.Abstractions;
using Luzanov.Domain.Models;
using System.Text;

namespace Luzanov.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        private readonly ITelegramBotService _telegramBot;

        public OrderService(IOrderRepository orderRepo, IMapper mapper, ITelegramBotService telegramBot)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
            _telegramBot = telegramBot;
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

        public async Task<OrderDto> CreateAsync(CreateOrderCommand command)
        {
            var order = _mapper.Map<Order>(command);
            
            // Set order items
            order.OrderItems = command.OrderItems;
            
            // Calculate total amount
            order.TotalAmount = command.OrderItems.Sum(item => item.Price * item.Quantity);
            
            // Set initial status
            order.OrderStatus = "Нове";
            order.CreatedAt = DateTime.UtcNow;

            await _orderRepo.AddAsync(order);

            // Send Telegram notification
            var telegramMessage = FormatOrderMessage(order);
            await _telegramBot.SendOrderNotificationAsync(telegramMessage);

            return _mapper.Map<OrderDto>(order);
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

        private string FormatOrderMessage(Order order)
        {
            var sb = new StringBuilder();

            sb.AppendLine("🛍️ <b>НОВЕ ЗАМОВЛЕННЯ</b>");
            sb.AppendLine("────────────────────");

            // Основна інформація
            sb.AppendLine($"🆔 <b>Номер:</b> <code>#{order.Id}</code>");
            sb.AppendLine($"👤 <b>Замовник:</b> {order.CustomerFullName}");
            sb.AppendLine($"📞 <b>Телефон:</b> {order.PhoneNumber}");
            sb.AppendLine();

            
            sb.AppendLine("🚚 <b>ДОСТАВКА</b>");
            sb.AppendLine($"📦 <b>Метод:</b> {order.DeliveryMethod}");
            sb.AppendLine($"📦 <b>Оплата:</b> {order.PaymentMethod}");


            if (!string.IsNullOrEmpty(order.PostService))
            {
                sb.AppendLine($"📮 <b>Сервіс:</b> {order.PostService}");
            }

            sb.AppendLine($"📍 <b>Адреса:</b> {order.DeliveryAddress}");
            sb.AppendLine();

            sb.AppendLine("🛒 <b>ТОВАРИ:</b>");

            foreach (var item in order.OrderItems)
            {
                sb.AppendLine($"🔹 <b>{item.ProductName}</b>");
                sb.AppendLine($"   └ {item.Size} | {item.Colour} | {item.Quantity} шт. × {item.Price:N0} грн");
            }

            sb.AppendLine("────────────────────");

            sb.AppendLine($"💰 <b>ЗАГАЛЬНА СУМА: {order.TotalAmount:N2} грн</b>");
            sb.AppendLine();
            sb.AppendLine($"📅 <i>{order.CreatedAt:dd.MM.yyyy HH:mm}</i>");

            return sb.ToString();
        }
    }
}
