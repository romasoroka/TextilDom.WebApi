using Luzanov.Application.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Luzanov.Application.Services
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly string _botToken;
        private readonly string _chatId;
        private readonly HttpClient _httpClient;

        public TelegramBotService(IConfiguration configuration, HttpClient httpClient)
        {
            _botToken = configuration["TelegramBot:BotToken"] ?? throw new ArgumentNullException("Telegram Bot Token is missing");
            _chatId = configuration["TelegramBot:ChatId"] ?? throw new ArgumentNullException("Telegram Chat ID is missing");
            _httpClient = httpClient;
        }

        public async Task SendOrderNotificationAsync(string message)
        {
            try
            {
                var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";
                var content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(new
                    {
                        chat_id = _chatId,
                        text = message,
                        parse_mode = "HTML"
                    }),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                // Log the error but don't fail the order creation
                Console.WriteLine($"Failed to send Telegram notification: {ex.Message}");
            }
        }
    }
}
