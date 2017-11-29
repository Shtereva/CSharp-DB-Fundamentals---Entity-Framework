using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketSystem.Data.Configuration
{
    internal class BusStationConfiguration : IEntityTypeConfiguration<BusStation>
    {
        public void Configure(EntityTypeBuilder<BusStation> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(20);

            builder.HasMany(b => b.Departures)
                .WithOne(d => d.OriginBusStation)
                .HasForeignKey(d => d.OriginBusStationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.Arrivals)
                .WithOne(a => a.DestinationBusStation)
                .HasForeignKey(a => a.DestinationBusStationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
