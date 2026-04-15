using Luzanov.Application.Comments.Commands;
using Luzanov.Application.Comments.Dtos;

namespace Luzanov.Application.Services.Abstractions
{
    /// <summary>
    /// Інтерфейс сервісу для роботи з коментарями
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// Отримати коментар за ID
        /// </summary>
        /// <param name="id">ID коментаря</param>
        /// <returns>DTO коментаря або null</returns>
        Task<CommentDto?> GetByIdAsync(int id);

        /// <summary>
        /// Отримати всі коментарі
        /// </summary>
        /// <returns>Список всіх коментарів</returns>
        Task<IEnumerable<CommentDto>> GetAllAsync();

        /// <summary>
        /// Отримати всі коментарі для конкретного товару
        /// </summary>
        /// <param name="productId">ID товару</param>
        /// <returns>Список коментарів товару</returns>
        Task<IEnumerable<CommentDto>> GetByProductIdAsync(int productId);

        /// <summary>
        /// Створити новий коментар
        /// </summary>
        /// <param name="command">Команда створення коментаря</param>
        /// <returns>Створений коментар</returns>
        Task<CommentDto> CreateAsync(CreateCommentCommand command);

        /// <summary>
        /// Видалити коментар (тільки для адміністраторів)
        /// </summary>
        /// <param name="id">ID коментаря</param>
        /// <returns>True якщо успішно видалено, false якщо не знайдено</returns>
        Task<bool> DeleteAsync(int id);
    }
}
