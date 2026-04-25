using Textildom.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Textildom.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Description)
                .HasMaxLength(5000); // збільшили бо опис з Excel довгий

            builder.Property(p => p.VariantsJson)
                .HasColumnName("Variants")
                .HasColumnType("jsonb");

            builder.Property(p => p.ProductImagesJson)
                .IsRequired()
                .HasColumnType("jsonb");

            builder.Property(p => p.IsSpecialOffer)
                .HasDefaultValue(false);

            builder.Property(p => p.IsTop)
                .HasDefaultValue(false);

            builder.Ignore(p => p.Variants);
            builder.Ignore(p => p.ProductImages);

            // SubCategoryId видалено, CategoryId залишається
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}