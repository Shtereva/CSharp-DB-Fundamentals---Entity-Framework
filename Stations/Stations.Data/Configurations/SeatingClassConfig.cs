using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stations.Models;

namespace Stations.Data.Configurations
{
    public class SeatingClassConfig : IEntityTypeConfiguration<SeatingClass>
    {
        public void Configure(EntityTypeBuilder<SeatingClass> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasAlternateKey(e => e.Name);

            builder.Property(e => e.Abbreviation).HasColumnType("nchar(2)").IsRequired();

            builder.HasAlternateKey(e => e.Abbreviation);
        }
    }
}
