namespace Textildom.Domain.Models
{
    /// <summary>
    /// Представляє коментар користувача з оцінкою для конкретного товару
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Унікальний ідентифікатор коментаря
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ім'я коментатора
        /// </summary>
        public string CommenterName { get; set; } = string.Empty;

        /// <summary>
        /// Оцінка від 1 до 5
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Текст коментаря
        /// </summary>
        public string CommentText { get; set; } = string.Empty;

        /// <summary>
        /// Дата створення коментаря
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ID товару, до якого відноситься коментар
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Навігаційна властивість до товару
        /// </summary>
        public Product? Product { get; set; }
    }
}
