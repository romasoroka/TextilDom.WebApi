using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Luzanov.Domain.Models;

namespace Luzanov.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            // Primary key
            builder.HasKey(p => p.Id);

            // Name
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            // Discount
            builder.Property(p => p.Discount)
                   .HasColumnType("numeric(18,2)")
                   .HasDefaultValue(0);

            // Description
            builder.Property(p => p.Description)
                   .HasMaxLength(2000);

            // Variants (Size + Price) 
            builder.Property(p => p.VariantsJson)
                   .HasColumnName("Variants")
                   .HasColumnType("text");

            builder.Property(p => p.ProductImagesJson)
                   .IsRequired()
                   .HasColumnType("text");

            // Flags
            builder.Property(p => p.IsSpecialOffer)
                   .HasDefaultValue(false);

            builder.Property(p => p.IsTop)
                   .HasDefaultValue(false);

            builder.Ignore(p => p.Variants);
            builder.Ignore(p => p.ProductImages);
        }
    }
}
