using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stations.Models;

namespace Stations.Data.Configurations
{
    public class TicketConfig : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Price).IsRequired();

            builder.Property(e => e.SeatingPlace).IsRequired().HasMaxLength(8);

            builder.HasOne(t => t.Trip)
                .WithMany(tr => tr.Tickets)
                .HasForeignKey(tr => tr.TripId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.CustomerCard)
                .WithMany(c => c.BoughtTickets)
                .HasForeignKey(c => c.CustomerCardId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
