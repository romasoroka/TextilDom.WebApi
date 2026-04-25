using Textildom.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Textildom.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(u => u.Role)
                .IsRequired()
                .HasColumnType("varchar(50)");
        }
    }
}