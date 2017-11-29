using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketSystem.Data.Configuration
{
    internal class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.DepartureTime)
                .IsRequired();

            builder.Property(e => e.ArrivalTime)
                .IsRequired();

            builder.Property(e => e.Status)
                .IsRequired();

        }
    }
}
