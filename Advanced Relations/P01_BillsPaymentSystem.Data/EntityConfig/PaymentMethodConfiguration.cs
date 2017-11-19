using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Data.EntityConfig
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(p => p.CreditCard)
                .WithOne(c => c.PaymentMethod)
                .HasForeignKey<PaymentMethod>(p => p.CreditCardId);

            builder
                .HasOne(p => p.BankAccount)
                .WithOne(b => b.PaymentMethod)
                .HasForeignKey<PaymentMethod>(p => p.BankAccountId);

            builder.Property(b => b.BankAccountId).IsRequired(false);

            builder.Property(c => c.CreditCardId).IsRequired(false);

            builder.HasIndex(pm => new
            {
                pm.UserId,
                pm.BankAccountId,
                pm.CreditCardId
            })
            .IsUnique();
        }
    }
}
