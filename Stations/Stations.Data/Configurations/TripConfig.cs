using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stations.Models;

namespace Stations.Data.Configurations
{
    public class TripConfig : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.DepartureTime).IsRequired();
            builder.Property(e => e.ArrivalTime).IsRequired();

            builder.Property(e => e.Status).IsRequired().HasDefaultValue(TripStatus.OnTime);

            builder.HasOne(t => t.OriginStation)
                .WithMany(os => os.TripsFrom)
                .HasForeignKey(os => os.OriginStationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.DestinationStation)
                .WithMany(ds => ds.TripsTo)
                .HasForeignKey(ds => ds.DestinationStationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Train)
                .WithMany(tr => tr.Trips)
                .HasForeignKey(tr => tr.TrainId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
