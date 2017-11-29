using System.ComponentModel.DataAnnotations.Schema;
using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketSystem.Data.Configuration
{
    internal class BusCompanyConfiguration : IEntityTypeConfiguration<BusCompany>
    {
        public void Configure(EntityTypeBuilder<BusCompany> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(n => n.Name)
                .IsRequired()
                .IsUnicode();

            builder.Property(nat => nat.Nationality)
                .IsRequired();

            builder.HasMany(b => b.Trips)
                .WithOne(t => t.BusCompany)
                .HasForeignKey(t => t.BusCompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
