using Instagraph.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagraph.Data.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Username).IsRequired().HasMaxLength(30);

            builder.HasAlternateKey(e => e.Username);

            builder.Property(e => e.Password).IsRequired().IsRequired().HasMaxLength(20);

            builder.HasOne(u => u.ProfilePicture)
                .WithMany(p => p.Users)
                .HasForeignKey(p => p.ProfilePictureId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
