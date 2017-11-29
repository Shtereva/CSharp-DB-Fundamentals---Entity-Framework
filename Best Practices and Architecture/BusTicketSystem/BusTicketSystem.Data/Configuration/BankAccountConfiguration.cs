using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketSystem.Data.Configuration
{
    internal class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.AccountNumber)
                .IsRequired();

            builder.Property(e => e.Balance)
                .HasDefaultValue(0);

            builder.HasOne(b => b.Customer)
                .WithOne(c => c.BankAccount)
                .HasForeignKey<BankAccount>(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
