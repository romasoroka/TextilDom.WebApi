namespace Textildom.Application.Users.Dtos
{
    /// <summary>
    /// DTO для представлення користувача
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// ID користувача
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ім'я користувача
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Роль користувача (Admin або User)
        /// </summary>
        public string Role { get; set; } = string.Empty;
    }
}
