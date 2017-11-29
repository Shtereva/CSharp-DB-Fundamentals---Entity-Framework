using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketSystem.Data.Configuration
{
    internal class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Content)
                .IsUnicode()
                .HasMaxLength(200);

            builder.Property(e => e.Grade)
                .IsRequired();

            builder.HasOne(r => r.BusCompany)
                .WithMany(c => c.Reviews)
                .HasForeignKey(c => c.BusCompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
