namespace Luzanov.Application.Services.Abstractions
{
    public interface ITelegramBotService
    {
        Task SendOrderNotificationAsync(string message);
    }
}
