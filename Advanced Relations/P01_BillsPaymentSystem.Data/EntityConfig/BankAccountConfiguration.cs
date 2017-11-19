using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Data.EntityConfig
{
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.HasKey(x => x.BankAccountId);

            builder.Property(bn => bn.BankName).HasColumnType("nvarchar(50)").IsRequired();

            builder.Property(s => s.SwiftCode).HasColumnType("varchar(20)").IsRequired();

            builder.Ignore(x => x.PaymentId);
        }
    }
}
