using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketSystem.Data.Configuration
{
    internal class TownConfiguration : IEntityTypeConfiguration<Town>
    {
        public void Configure(EntityTypeBuilder<Town> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired();

            builder.Property(e => e.Country)
                .IsRequired();

            builder.HasMany(t => t.People)
                .WithOne(p => p.HomeTown)
                .HasForeignKey(p => p.HomeTownId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.BusStations)
                .WithOne(b => b.Town)
                .HasForeignKey(b => b.TownId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
