using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketSystem.Data.Configuration
{
    internal class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Price)
                .IsRequired();

            builder.Property(e => e.Seat)
                .IsRequired();

            builder.HasOne(t => t.Trip)
                .WithMany(tr => tr.Tickets)
                .HasForeignKey(tr => tr.TripId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Customer)
                .WithOne(c => c.Ticket)
                .HasForeignKey<Ticket>(t => t.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
