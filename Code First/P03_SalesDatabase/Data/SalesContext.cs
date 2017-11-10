using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public SalesContext()
        {
        }

        public SalesContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(p =>
            {
                p.Property(n => n.Name).HasColumnType("nvarchar(50)");
                p.Property(q => q.Quantity).HasColumnType("decimal(18, 2)");
                p.Property(d => d.Description).HasColumnType("varchar(250)").HasDefaultValue("No description");
            });

            modelBuilder.Entity<Customer>(c =>
            {
                c.Property(n => n.Name).HasColumnType("nvarchar(100)");
                c.Property(e => e.Email).HasColumnType("varchar(80)");
            });

            modelBuilder.Entity<Store>()
                .Property(n => n.Name).HasColumnType("nvarchar(80)");

            modelBuilder.Entity<Sale>()
                .Property(d => d.Date).HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Sales)
                .WithOne(s => s.Product)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Sales)
                .WithOne(s => s.Customer)
                .HasForeignKey(c => c.CustomerId);

            modelBuilder.Entity<Store>()
                .HasMany(s => s.Sales)
                .WithOne(sa => sa.Store)
                .HasForeignKey(s => s.StoreId);
        }
    }
}
