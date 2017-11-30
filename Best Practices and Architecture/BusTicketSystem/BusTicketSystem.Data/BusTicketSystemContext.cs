using BusTicketSystem.Data.Configuration;
using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BusTicketSystem.Data
{
    public class BusTicketSystemContext : DbContext
    {
        public BusTicketSystemContext() { }

        public BusTicketSystemContext(DbContextOptions options)
            : base(options) { }

        public DbSet<BusCompany> BusCompanies { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<BankAccount> BankAccounts { get; set; }

        public DbSet<BusStation> BusStations { get; set; }

        public DbSet<Customer> Customers{ get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<ArrivedTrip> ArrivedTrips { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ServerConfig.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BusCompanyConfiguration());

            modelBuilder.ApplyConfiguration(new TownConfiguration());

            modelBuilder.ApplyConfiguration(new CustomerConfiguration());

            modelBuilder.ApplyConfiguration(new BankAccountConfiguration());

            modelBuilder.ApplyConfiguration(new BusStationConfiguration());

            modelBuilder.ApplyConfiguration(new TripConfiguration());

            modelBuilder.ApplyConfiguration(new ReviewConfiguration());

            modelBuilder.ApplyConfiguration(new TicketConfiguration());
        }
    }
}
