using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Luzanov.Domain.Models;

namespace Luzanov.Infrastructure.Configurations
{
    /// <summary>
    /// Конфігурація Entity Framework для сутності Comment
    /// </summary>
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");

            // Primary key
            builder.HasKey(c => c.Id);

            // CommenterName - обов'язкове поле, максимум 100 символів
            builder.Property(c => c.CommenterName)
                   .IsRequired()
                   .HasMaxLength(100);

            // Rating - обов'язкове поле, має бути між 1 та 5
            builder.Property(c => c.Rating)
                   .IsRequired();

            // CommentText - обов'язкове поле, максимум 500 символів
            builder.Property(c => c.CommentText)
                   .IsRequired()
                   .HasMaxLength(500);

            // CreatedAt - дата створення з дефолтним значенням
            builder.Property(c => c.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("NOW()");

            // ProductId - Foreign Key до Product
            builder.Property(c => c.ProductId)
                   .IsRequired();

            // Налаштування зв'язку Comment ? Product (Many-to-One)
            builder.HasOne(c => c.Product)
                   .WithMany()
                   .HasForeignKey(c => c.ProductId)
                   .OnDelete(DeleteBehavior.Cascade); // При видаленні товару видаляються всі коментарі

            // Індекс для швидкого пошуку коментарів за ProductId
            builder.HasIndex(c => c.ProductId);
        }
    }
}
