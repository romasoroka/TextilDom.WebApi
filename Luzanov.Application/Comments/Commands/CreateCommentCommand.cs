namespace Luzanov.Application.Comments.Commands
{
    /// <summary>
    /// Команда для створення нового коментаря
    /// </summary>
    public class CreateCommentCommand
    {
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
        /// ID товару, до якого відноситься коментар
        /// </summary>
        public int ProductId { get; set; }
    }
}
