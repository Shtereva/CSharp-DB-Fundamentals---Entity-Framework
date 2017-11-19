using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Data.EntityConfig
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(u => u.PaymentMethods)
                .WithOne(pm => pm.User)
                .HasForeignKey(u => u.UserId);

            builder.Property(fn => fn.FirstName).HasColumnType("nvarchar(50)").IsRequired();

            builder.Property(ln => ln.LastName).HasColumnType("nvarchar(50)").IsRequired();

            builder.Property(e => e.Email).HasColumnType("varchar(80)").IsRequired();

            builder.Property(p => p.Password).HasColumnType("varchar(25)").IsRequired();

        }
    }
}
