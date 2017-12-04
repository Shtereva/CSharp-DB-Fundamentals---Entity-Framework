using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Models;

namespace Products.Data.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).IsRequired();

            builder.Property(e => e.Price).IsRequired();

            builder.HasOne(p => p.Buyer)
                .WithMany(b => b.BoughtProducts)
                .HasForeignKey(b => b.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Seller)
                .WithMany(b => b.SoldProducts)
                .HasForeignKey(b => b.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
