namespace Luzanov.Application.Comments.Dtos
{
    /// <summary>
    /// DTO для представлення коментаря
    /// </summary>
    public class CommentDto
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
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// ID товару, до якого відноситься коментар
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Назва товару (опціонально)
        /// </summary>
        public string? ProductName { get; set; }
    }
}
