using Employees.Models;
using Microsoft.EntityFrameworkCore;

namespace Employees.Data
{
    public class EmployeesContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Employee>(e =>
            {
                e.HasKey(em => em.Id);

                e.Property(n => n.FirstName).IsRequired();

                e.Property(n => n.LastName).IsRequired();

                e.Property(n => n.Salary).IsRequired();

            });

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany(m => m.ManagedEmployees)
                .HasForeignKey(m => m.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
