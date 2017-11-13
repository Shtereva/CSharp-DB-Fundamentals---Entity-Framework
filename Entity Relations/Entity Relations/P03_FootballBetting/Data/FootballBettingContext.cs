using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {
        }

        public FootballBettingContext(DbContextOptions options)
            :base (options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Bet> Bets { get; set; }

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
            modelBuilder.Entity<Country>()
                .HasMany(c => c.Towns)
                .WithOne(t => t.Country)
                .HasForeignKey(c => c.CountryId);

            modelBuilder.Entity<Town>()
                .HasMany(t => t.Teams)
                .WithOne(te => te.Town)
                .HasForeignKey(t => t.TownId);

            modelBuilder.Entity<Position>()
                .HasMany(p => p.Players)
                .WithOne(pl => pl.Position)
                .HasForeignKey(p => p.PositionId);

            modelBuilder.Entity<Color>(entity =>
            {
                entity
                    .HasMany(c => c.PrimaryKitTeams)
                    .WithOne(t => t.PrimaryKitColor)
                    .HasForeignKey(c => c.PrimaryKitColorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasMany(c => c.SecondaryKitTeams)
                    .WithOne(t => t.SecondaryKitColor)
                    .HasForeignKey(c => c.SecondaryKitColorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity
                    .HasMany(t => t.Players)
                    .WithOne(p => p.Team)
                    .HasForeignKey(t => t.TeamId);

                entity
                    .HasMany(t => t.HomeGames)
                    .WithOne(g => g.HomeTeam)
                    .HasForeignKey(t => t.HomeTeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasMany(t => t.AwayGames)
                    .WithOne(g => g.AwayTeam)
                    .HasForeignKey(t => t.AwayTeamId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PlayerStatistic>()
                .HasKey(pl => new
                {
                    pl.PlayerId,
                    pl.GameId
                });

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Bets)
                .WithOne(b => b.Game)
                .HasForeignKey(g => g.GameId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Bets)
                .WithOne(b => b.User)
                .HasForeignKey(u => u.UserId);
        }
    }
}
