using Luzanov.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Luzanov.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.CustomerFullName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("varchar(200)");

            builder.Property(o => o.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("varchar(20)");

            builder.Property(o => o.DeliveryMethod)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder.Property(o => o.PostService)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(o => o.DeliveryAddress)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("varchar(500)");

            builder.Property(o => o.PaymentMethod)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.OrderStatus)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .HasDefaultValue("Нове");

            builder.Property(o => o.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            builder.Property(o => o.Comment)
                .HasMaxLength(1000)
                .HasColumnType("varchar(1000)");

            builder.Property(o => o.OrderItemsJson)
                .IsRequired()
                .HasColumnType("jsonb");
        }
    }
}
