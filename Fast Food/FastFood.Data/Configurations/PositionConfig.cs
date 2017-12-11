using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastFood.Data.Configurations
{
    public class PositionConfig : IEntityTypeConfiguration<Models.Position>
    {
        public void Configure(EntityTypeBuilder<Models.Position> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).IsRequired().HasMaxLength(30);

            builder.HasAlternateKey(e => e.Name);

        }
    }
}
