using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stations.Models;

namespace Stations.Data.Configurations
{
    public class TrainSeatConfig : IEntityTypeConfiguration<TrainSeat>
    {
        public void Configure(EntityTypeBuilder<TrainSeat> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Quantity).IsRequired();

            builder.HasOne(ts => ts.Train)
                .WithMany(t => t.TrainSeats)
                .HasForeignKey(t => t.TrainId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ts => ts.SeatingClass)
                .WithMany(c => c.TrainSeats)
                .HasForeignKey(c => c.SeatingClassId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
