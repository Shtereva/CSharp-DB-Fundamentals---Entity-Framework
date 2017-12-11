using FastFood.Models;
using FastFood.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastFood.Data.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Customer).IsRequired();
            builder.Property(e => e.DateTime).IsRequired();
            builder.Property(e => e.Type).IsRequired().HasDefaultValue(OrderType.ForHere);

            builder.Ignore(e => e.TotalPrice);
        }
    }
}
