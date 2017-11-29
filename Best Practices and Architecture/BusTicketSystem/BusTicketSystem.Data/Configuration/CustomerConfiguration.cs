using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketSystem.Data.Configuration
{
    internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FirstName)
                .IsRequired()
                .IsUnicode();

            builder.Property(e => e.LastName)
                .IsRequired()
                .IsUnicode();

            builder.Property(e => e.Gender)
                .HasDefaultValue(Gender.NotSpecified);

            builder.Ignore(e => e.BankAccountId);
            builder.Ignore(e => e.TicketId);

            builder.HasMany(c => c.Reviews)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
